using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

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
        
        Destroy(other.gameObject);
        
        _gameController.OnEnemyAttack();
    }
}
