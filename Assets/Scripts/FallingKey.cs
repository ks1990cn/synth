using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FallingKey : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private List<Sprite> _spriteArray = new List<Sprite>();

    public float FallingSpeed = 10f;
    public FallingType Type;
    
    void Start()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        Type = (FallingType)Mathf.Ceil(Random.Range(1, 4)) - 1;

        GetComponent<Image>().sprite = _spriteArray[(int)Type];
    }

    private void FixedUpdate()
    {
        _rectTransform.localPosition -= new Vector3(0f, FallingSpeed, 0f);
    }
}

public enum FallingType
{
    Up,
    Right,
    Down,
    Left
}