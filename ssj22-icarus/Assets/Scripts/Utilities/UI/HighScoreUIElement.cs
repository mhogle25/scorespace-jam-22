using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class HighScoreUIElement :MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameField = null;
    [SerializeField] private TextMeshProUGUI scoreField = null;

    public void SetScore(int score, Color color)
    {
        this.scoreField.text = $"Score: {score}";
        this.scoreField.color = color;
    }

    public void SetName(string name, Color color)
    {
        this.nameField.text = $"Name: {name}";
        this.nameField.color = color;
    }
}
