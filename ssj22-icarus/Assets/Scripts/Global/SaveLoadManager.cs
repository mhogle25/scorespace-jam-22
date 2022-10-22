using UnityEngine;
using System.Collections.Generic;
using DataStructures.PriorityQueue;
using Newtonsoft.Json;
using System.IO;
using System;

public class SaveLoadManager
{
    [Serializable]
    private class SaveData
    {
        [JsonIgnore] public readonly PriorityQueue<Score, int> highScoresQueue = new(0);
        [JsonIgnore] public int queueCount = 0;
        [JsonProperty] public readonly List<Score> scores = new();

        [JsonIgnore] public Score HighScore
        {
            get
            {
                if (this.queueCount < 1)
                {
                    FillQueue();
                }

                return this.highScoresQueue.Top();
            }
        }

        public void FillQueue()
        {
            foreach (Score score in this.scores)
            {
                this.highScoresQueue.Insert(score, score.score);
            }
        }

        public Score PopQueue()
        {
            if (this.queueCount < 1)
            {
                FillQueue();
            }

            return this.highScoresQueue.Pop();
        }
    }

    public Score HighScore
    {
        get
        {
            return this.saveData.HighScore;
        }
    }

    private SaveData saveData = new();

    public void SaveGame()
    {
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "save", "save.json"), JsonConvert.SerializeObject(this.saveData));
    }

    public void LoadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, "save", "save.json");
        if (File.Exists(path))
        {
            this.saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(path));
        } 
        else
        {
            SaveGame();
        }
    }

    public Score PopScore()
    {
        return this.saveData.PopQueue();
    }

    public void NewScore(string name, int score)
    {
        Score newScore = new Score
        {
            name = name,
            score = score
        };

        this.saveData.highScoresQueue.Insert(newScore, score);
        this.saveData.scores.Add(newScore);
    }
}