using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject FallingKeyPrefab;
    public Canvas MainCanvas;

    [SerializeField] private int _maxPlayerHp = 10;
    [SerializeField] private int _playerHp = 0;
    
    [SerializeField] private int _maxEnemyHp = 100;
    [SerializeField] private int _enemyHp = 0;

    [SerializeField] private int _combo = 0;
    [SerializeField] private int _score = 0;
    [SerializeField] private int _caught = 0;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    [SerializeField] private Image _playerBar;
    [SerializeField] private Image _enemyBar;

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _comboText;

    [SerializeField] private int _stage = 0;
    [SerializeField] private float _fallingSpeed = 1f;
    [SerializeField] private int _fallingDensity = 10;
    [SerializeField] private int _spawnCounter = 0;
    [SerializeField] private int _spawnRate = 10;

    [SerializeField] private float _prepareTimer = 10f;
    [SerializeField] private StageIndicator _stageIndicator;
    
    void Start()
    {
        InvokeRepeating("SpawnFalling", 0f, 1f / _spawnRate);
        InvokeRepeating("DecreaseTimer", 0f, 1f / 10);

        _playerHp = _maxPlayerHp;
        _enemyHp = _maxEnemyHp;

        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
        
        if (_enemy == null)
        {
            _enemy = GameObject.FindGameObjectWithTag("Enemy");
        }        
        
        if (_stageIndicator == null)
        {
            _stageIndicator = GameObject.FindGameObjectWithTag("StageIndicator").GetComponent<StageIndicator>();
        }
        
        _enemy.GetComponent<Animator>().ResetTrigger("Attack");
        _enemy.GetComponent<Animator>().ResetTrigger("Hit");
        _player.GetComponent<Animator>().ResetTrigger("Attack");
        _player.GetComponent<Animator>().ResetTrigger("Hit");
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
    }

    public void OnPlayerAttack()
    {
        _enemyHp--;

        _combo++;
        _caught++;

        _score += _combo * 10;
        
        _enemy.GetComponent<Animator>().SetTrigger("Hit");
        _player.GetComponent<Animator>().SetTrigger("Attack");
    }
    
    private void FixedUpdate()
    {
        if (_playerHp <= 0)
        {
            Debug.Log("Game over");
        }

        if (_enemyHp <= 0)
        {
            Debug.Log("Win");
        }
        
        _playerBar.rectTransform.sizeDelta = new Vector2(160f * ((float)_playerHp / (float)_maxPlayerHp), 32f);
        _enemyBar.rectTransform.sizeDelta = new Vector2(160f * ((float)_enemyHp / (float)_maxEnemyHp), 32f);

        _scoreText.text = "Score: " + _score;
        _comboText.text = "Combo! x" + _combo;

        if (_stage == 0 && _caught == 0)
        {
            _stageIndicator.Move("First Stage!");
            _prepareTimer = 5f;
            _stage = 1;
        } 
        else if (_stage == 1 && _caught > 10)
        {
            _stageIndicator.Move("Second Stage!");
            _prepareTimer = 5f;
            _stage = 2;
        }
        else if (_stage == 2 && _caught > 20)
        {
            _stageIndicator.Move("Third Stage!");
            _prepareTimer = 5f;
            _stage = 3;
        }
        else if (_stage == 3 && _caught > 30)
        {
            _stageIndicator.Move("Final Stage!");
            _prepareTimer = 5f;
            _stage = 4;
        }
    }

    void DecreaseTimer()
    {
        _prepareTimer -= 0.1f;
    }
}
