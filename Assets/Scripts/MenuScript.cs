using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private bool _isMenuShown;

    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _tip;
    [SerializeField] private GameObject _continue;
    [SerializeField] private GameObject _fadeIn;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (_isMenuShown)
            {
                _fadeIn.SetActive(true);
                Invoke("LoadGameplay", 4f);
            }
            else
            {
                _isMenuShown = true;
                _background.SetActive(_isMenuShown);
                _tip.SetActive(_isMenuShown);
                _continue.SetActive(_isMenuShown);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isMenuShown = false;
            _background.SetActive(_isMenuShown);
            _tip.SetActive(_isMenuShown);
            _continue.SetActive(_isMenuShown);
        }
    }

    void LoadGameplay()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
