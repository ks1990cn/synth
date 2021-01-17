using UnityEngine;
using UnityEngine.UI;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private FallingKey _fallingKey;

    private void Start()
    {
        if (_gameController == null)
        {
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Falling")) return;
        _fallingKey = other.GetComponent<FallingKey>();
        _fallingKey.gameObject.GetComponent<Image>().color = new Color32(255,255,255,255);
        gameObject.GetComponent<Image>().color = new Color32(255,255,255,255);
    }

    private void Update()
    {
        if (!Input.anyKeyDown || _fallingKey == null ) return;

        if ((Input.GetKeyDown(KeyCode.UpArrow) && _fallingKey.Type == FallingType.Up) ||
            (Input.GetKeyDown(KeyCode.RightArrow) && _fallingKey.Type == FallingType.Right) ||
            (Input.GetKeyDown(KeyCode.DownArrow) && _fallingKey.Type == FallingType.Down) ||
            (Input.GetKeyDown(KeyCode.LeftArrow) && _fallingKey.Type == FallingType.Left))
        {
            _gameController.OnPlayerAttack();
            DestroyFallingKey();
        }
        else
        {
            _gameController.OnEnemyAttack();
            DestroyFallingKey();
        }
    }

    private void DestroyFallingKey()
    {
        if (_fallingKey == null) return;
        
        Destroy(_fallingKey.gameObject);
        _fallingKey = null;
        gameObject.GetComponent<Image>().color = new Color32(255,255,255,125);
    }
}
