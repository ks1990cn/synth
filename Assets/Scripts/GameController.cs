using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FallingKeyPrefab;
    public Canvas MainCanvas;

    [Header("Player Health")]
    [SerializeField] private int _maxPlayerHp = 10;
    [SerializeField] private int _playerHp;

    [Header("Enemy Health")]
    [SerializeField] private int _maxEnemyHp = 100;
    [SerializeField] private int _enemyHp;

    [Header("Score")]
    [SerializeField] private int _combo;
    [SerializeField] private int _score;
    [SerializeField] private int _caught;

    [Header("Animated Objects")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    [Header("UI")]
    [SerializeField] private Image _playerBar;
    [SerializeField] private Image _enemyBar;
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _comboText;

    [Header("Progress")]
    [SerializeField] private int _stage;
    [SerializeField] private int _fallingDensity = 10;
    [SerializeField] private int _spawnCounter;
    [SerializeField] private int _spawnRate = 10;
    
    [Header("Audio")]
    [SerializeField] private StudioEventEmitter _battleMusicEventEmitter;
    [SerializeField] private PostProcessVolume _postProcessVolume;
    
    [Header("Misc")]
    [SerializeField] private float _prepareTimer = 5f;
    [SerializeField] private StageIndicator _stageIndicator;
    [SerializeField] private GameObject _fadeIn;
    
    private void Start()
    {
        InvokeRepeating("SpawnFalling", 0f, 1f / _spawnRate);
        InvokeRepeating("DecreaseTimer", 0f, 1f / 10);

        _playerHp = _maxPlayerHp;
        _enemyHp = _maxEnemyHp;

        if (_player == null) _player = GameObject.FindGameObjectWithTag("Player");

        if (_enemy == null) _enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (_stageIndicator == null)
            _stageIndicator = GameObject.FindGameObjectWithTag("StageIndicator").GetComponent<StageIndicator>();

        if (_battleMusicEventEmitter == null)
            _battleMusicEventEmitter =
                GameObject.FindGameObjectWithTag("AudioController").GetComponent<StudioEventEmitter>();

        if (_postProcessVolume == null)
            _postProcessVolume = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PostProcessVolume>();

        _enemy.GetComponent<Animator>().ResetTrigger("Attack");
        _enemy.GetComponent<Animator>().ResetTrigger("Hit");
        _player.GetComponent<Animator>().ResetTrigger("Attack");
        _player.GetComponent<Animator>().ResetTrigger("Hit");
    }

    private void FixedUpdate()
    {
        _battleMusicEventEmitter.SetParameter("Intensity", _caught);
        _battleMusicEventEmitter.SetParameter("Health", (float) _playerHp / _maxPlayerHp * 100f);
        _battleMusicEventEmitter.SetParameter("Combo", (float) _combo / _maxEnemyHp * 100f);

        ColorGrading colorGrading;
        _postProcessVolume.profile.TryGetSettings(out colorGrading);

        if (colorGrading != null && _playerHp <= _maxPlayerHp / 2)
            colorGrading.saturation.value = -100 + (float) _playerHp / _maxPlayerHp * 100f;

        ChromaticAberration chromaticAberration;
        _postProcessVolume.profile.TryGetSettings(out chromaticAberration);

        if (chromaticAberration != null && _playerHp <= _maxPlayerHp / 2)
            chromaticAberration.intensity.value = 1 - (float) _playerHp / _maxPlayerHp;

        _playerBar.rectTransform.sizeDelta = new Vector2(160f * ((float) _playerHp / _maxPlayerHp), 32f);
        _enemyBar.rectTransform.sizeDelta = new Vector2(160f * ((float) _enemyHp / _maxEnemyHp), 32f);

        _scoreText.text = "Score: " + _score;
        _comboText.text = "Combo! x" + _combo;

        if (_stage == 0 && _prepareTimer < 3 && _caught == 0)
        {
            _stageIndicator.Move("First Stage!");
            _prepareTimer = 5f;
            _stage = 1;
            _fallingDensity = 10;
            RuntimeManager.PlayOneShot("event:/Stages/FirstStage");
        }
        else if (_stage == 1 && _caught >= 20)
        {
            _stageIndicator.Move("Second Stage!");
            _prepareTimer = 5f;
            _stage = 2;
            _fallingDensity = 8;
            RuntimeManager.PlayOneShot("event:/Stages/SecondStage");
        }
        else if (_stage == 2 && _caught >= 50)
        {
            _stageIndicator.Move("Final Stage!");
            _prepareTimer = 5f;
            _stage = 3;
            _fallingDensity = 6;
            RuntimeManager.PlayOneShot("event:/Stages/LastStage");
        }
        else if (_stage == 3 && _caught >= 99)
        {
            _stageIndicator.Move("You Win!");
            _prepareTimer = 100f;
            _stage = 4;
            RuntimeManager.PlayOneShot("event:/Other/YouWin");
            Invoke("FadeToMainMenu", 5f);
        }

        if (_playerHp <= 0 && _stage != 4)
        {
            _stageIndicator.Move("Game Over!");
            _prepareTimer = 100f;
            _stage = 4;
            RuntimeManager.PlayOneShot("event:/Other/GameOver");
            Invoke("FadeToMainMenu", 5f);
        }
    }

    private void SpawnFalling()
    {
        if (_prepareTimer <= 0)
        {
            _spawnCounter--;

            if (_spawnCounter <= 0)
            {
                GameObject newObject = Instantiate(FallingKeyPrefab, MainCanvas.transform);
                newObject.transform.SetParent(MainCanvas.transform);
                _spawnCounter = _fallingDensity;
            }
        }
    }

    public void OnEnemyAttack()
    {
        _playerHp--;

        _combo = 0;

        _enemy.GetComponent<Animator>().SetTrigger("Attack");
        _player.GetComponent<Animator>().SetTrigger("Hit");
        RuntimeManager.PlayOneShot("event:/Other/Punch");
    }

    public void OnPlayerAttack()
    {
        _enemyHp--;

        _combo++;
        _caught++;

        _score += _combo * 10;

        _enemy.GetComponent<Animator>().SetTrigger("Hit");
        _player.GetComponent<Animator>().SetTrigger("Attack");
        RuntimeManager.PlayOneShot("event:/Other/Punch");

        if (_combo == 5)
            RuntimeManager.PlayOneShot("event:/Combo/Good");
        else if (_combo == 10)
            RuntimeManager.PlayOneShot("event:/Combo/Sweet");
        else if (_combo == 15)
            RuntimeManager.PlayOneShot("event:/Combo/Crazy");
        else if (_combo == 20)
            RuntimeManager.PlayOneShot("event:/Combo/Brutal");
        else if (_combo == 25)
            RuntimeManager.PlayOneShot("event:/Combo/Savage");
        else if (_combo == 30)
            RuntimeManager.PlayOneShot("event:/Combo/Stylish");
        else if (_combo == 35) RuntimeManager.PlayOneShot("event:/Combo/Apocalyptic");
    }

    private void DecreaseTimer()
    {
        _prepareTimer -= 0.1f;
    }

    private void FadeToMainMenu()
    {
        _fadeIn.SetActive(true);
        Invoke("GoToMainMenu", 3f);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}