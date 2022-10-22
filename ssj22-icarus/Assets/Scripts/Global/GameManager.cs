using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return GameManager.instance;
        }
    }
    private static GameManager instance = null;

    private readonly SaveLoadManager saveLoadManager = new();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (GameManager.instance != this && GameManager.instance != null)
            Destroy(GameManager.instance.gameObject);
        GameManager.instance = this;

        LoadGame();
    }

    public Score HighScore
    {
        get
        {
            return this.saveLoadManager.HighScore;
        }
    }

    public Score PopScore()
    {
        return this.saveLoadManager.PopScore();
    }

    public void NewScore(string name, int score)
    {
        this.saveLoadManager.NewScore(name, score);
    }

    public void SaveGame()
    {
        this.saveLoadManager.SaveGame();
    }

    private void LoadGame()
    {
        this.saveLoadManager.LoadGame();
    }
}
