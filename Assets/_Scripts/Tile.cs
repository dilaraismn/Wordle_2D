using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    private TextMeshProUGUI letterText;
    public char letter { get; private set; }

    private void Awake()
    {
        letterText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetLetter(char letter)
    {
        this.letter = letter;
        letterText.text = letter.ToString();    
    }
}
