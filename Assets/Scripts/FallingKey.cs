using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FallingKey : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private List<Sprite> _spriteArray = new List<Sprite>();
    [SerializeField] private Image _image;
    
    public float FallingSpeed = 10f;
    public FallingType Type;
    
    void Start()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        if (_image == null)
        {
            _image = gameObject.GetComponent<Image>();
        }

        Type = (FallingType)Mathf.Ceil(Random.Range(1, 4)) - 1;

        GetComponent<Image>().sprite = _spriteArray[(int)Type];
    }

    private void FixedUpdate()
    {
        _rectTransform.localPosition -= new Vector3(0f, FallingSpeed, 0f);
        if (_image.color.a < 255)
        {
            _image.color = new Color(255, 255, 255, _image.color.a + 0.01f);
        }
    }
}

public enum FallingType
{
    Up,
    Right,
    Down,
    Left
}