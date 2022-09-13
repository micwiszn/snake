using UnityEngine;
public class ScoreCounter : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI display;

    private int _score = 0;
    public int Score
    {
        get => _score;
        set => _score = value;
    }

    private void Awake()
    {
        UpdateDisplay();
    }
    public void IncreaseScore()
    {
        Score++;
        UpdateDisplay();
    }

    public void Reset()
    {
        Score = 0;
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        display.text = Score.ToString();
    }
}
