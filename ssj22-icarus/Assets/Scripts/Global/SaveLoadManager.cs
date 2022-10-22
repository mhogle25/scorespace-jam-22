using UnityEngine;
using System.Collections.Generic;
using DataStructures.PriorityQueue;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;

public class SaveLoadManager
{
    [Serializable]
    private class SaveData
    {
        [JsonProperty] private readonly List<Score> scores = new();

        [JsonIgnore]
        public Score HighScore
        {
            get
            {
                Score max = new()
                {
                    score = int.MinValue,
                };
                foreach (Score score in this.scores)
                {
                    if (score.score > max.score) max = score;
                }
                return max;
            }
        }

        [JsonIgnore]
        public List<Score> Scores
        {
            get
            {
                this.Scores.Sort((x, y) =>
                {
                    return x.score.CompareTo(y.score);
                });

                return scores;
            }
        }

        public void AddScore(string name, int score)
        {
            this.scores.Add(new Score
            {
                name = name,
                score = score
            });
        }
    }

    public Score HighScore
    {
        get
        {
            return this.saveData.HighScore;
        }
    }

    public List<Score> Scores
    {
        get
        {
            return this.saveData.Scores;
        }
    }

    private SaveData saveData = new();

    public void SaveGame()
    {
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "save.json"), JsonConvert.SerializeObject(this.saveData));
    }

    public void LoadGame()
    {;
        if (File.Exists(Path.Combine(Application.persistentDataPath, "save.json")))
        {
            this.saveData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "save.json")));
        } 
        else
        {
            SaveGame();
        }
    }

    public void NewScore(string name, int score)
    {
        this.saveData.AddScore(name, score);
    }
}