using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

class HighScoresManager : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private HighScoreUIElement highScorePrefab;

    private void Awake()
    {
        List<Score> scores = GlobalManager.Instance.Scores;
        PopulateHighScoresPanel(scores);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    private void PopulateHighScoresPanel(List<Score> scores)
    {
        int i = 0;
        foreach (Score score in scores)
        {
            Color newColor = Color.white;
            if (i == 0)
                newColor = new Color32(204, 171, 94, 255);
            InstantiateHighScore(score, newColor);
            i++;
        }
    }

    private void InstantiateHighScore(Score score, Color color)
    {
        HighScoreUIElement highScoreElement = Instantiate(this.highScorePrefab);
        highScoreElement.SetName(score.name, color);
        highScoreElement.SetScore(score.score, color);
        highScoreElement.transform.SetParent(this.content);
        highScoreElement.transform.localScale = Vector3.one;
    }
}