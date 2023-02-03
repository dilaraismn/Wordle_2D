using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject language_UI, gameScreen;
    public static bool isTurkish;

    private void Start()
    {
        gameScreen.SetActive(false);
        language_UI.SetActive(true);
    }

    public void Button_ENG()
    {
        isTurkish = false;
        language_UI.SetActive(false);
        gameScreen.SetActive(true);
    }

    public void Button_TR()
    {
        isTurkish = true;
        language_UI.SetActive(false);
        gameScreen.SetActive(true);
    }
}
