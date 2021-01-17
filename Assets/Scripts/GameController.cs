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
    
    void Start()
    {
        InvokeRepeating("SpawnFalling", 0f, 1f / 2);

        _playerHp = _maxPlayerHp;
        _enemyHp = _maxEnemyHp;
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
    }

    public void OnPlayerAttack()
    {
        _enemyHp--;

        _combo++;

        _score += _combo * 10;
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
