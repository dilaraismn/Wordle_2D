using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public Color fillColor, outlineColor;
    }
    
    public State state { get; private set; }
    public char letter { get; private set; }
    
    
    private TextMeshProUGUI letterText;
    private Image imageFill;
    private Outline outline;

    private void Awake()
    {
        letterText = GetComponentInChildren<TextMeshProUGUI>();
        imageFill = GetComponent<Image>();
        outline = GetComponent<Outline>();
    }

    public void SetLetter(char letter)
    {
        this.letter = letter;
        letterText.text = letter.ToString();    
    }

    public void SetState(State state)
    {
        this.state = state;
        imageFill.color = state.fillColor;
        outline.effectColor = state.outlineColor;
    }
}
