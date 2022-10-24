using UnityEngine;
using System;
using System.Collections.Generic;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance
    {
        get
        {
            return GlobalManager.instance;
        }
    }
    private static GlobalManager instance = null;

    private readonly SaveLoadManager saveLoadManager = new();

    public Score newScore = null;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (GlobalManager.instance != this && GlobalManager.instance != null)
            Destroy(GlobalManager.instance.gameObject);
        GlobalManager.instance = this;

        LoadGame();
    }

    public Score HighScore
    {
        get
        {
            return this.saveLoadManager.HighScore;
        }
    }

    public List<Score> Scores
    {
        get
        {
            return this.saveLoadManager.Scores;
        }
    }

    public bool FirstTime
    {
        get
        {
            return this.saveLoadManager.FirstTime;
        }

        set
        {
            this.saveLoadManager.FirstTime = value;
        }
    }

    public void AddNewScore()
    {
        this.saveLoadManager.NewScore(this.newScore);
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
