using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;

public class SaveLoadManager
{
    [Serializable]
    private class SaveData
    {
        [JsonProperty] private bool firstTime = true;
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
        public bool FirstTime
        {
            get
            {
                return this.firstTime;
            }

            set
            {
                this.firstTime = value;
            }
        }

        [JsonIgnore]
        public List<Score> Scores
        {
            get
            {
                this.scores.Sort((x, y) =>
                {
                    return y.score.CompareTo(x.score);
                });

                return scores;
            }
        }

        public void AddScore(Score score)
        {
            this.scores.Add(score);
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

    public void NewScore(Score score)
    {
        this.saveData.AddScore(score);
    }
}