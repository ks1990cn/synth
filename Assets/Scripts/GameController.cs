using UnityEngine;

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

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    void Start()
    {
        InvokeRepeating("SpawnFalling", 0f, 1f / 2);

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
        
        _enemy.GetComponent<Animator>().ResetTrigger("Attack");
        _enemy.GetComponent<Animator>().ResetTrigger("Hit");
        _player.GetComponent<Animator>().ResetTrigger("Attack");
        _player.GetComponent<Animator>().ResetTrigger("Hit");
    }

    private void SpawnFalling()
    {
        GameObject newObject = Instantiate(FallingKeyPrefab, MainCanvas.transform);
        newObject.transform.SetParent(MainCanvas.transform);
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
    }
}
