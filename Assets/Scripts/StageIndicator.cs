using System;
using UnityEngine;
using UnityEngine.UI;

public class StageIndicator : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Vector3 _initialPosition;
    [SerializeField] private bool _isMoving;
    
    private void Start()
    {
        _initialPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (_isMoving)
        {
            transform.position += transform.right * -_speed;
        }
    }

    public void Move(string text)
    {
        _isMoving = true;
        GetComponent<TextMesh>().text = text;
        
        Invoke("StopMoving", 5f);
    }

    void StopMoving()
    {
        _isMoving = false;
        transform.position = _initialPosition;
    }
}
