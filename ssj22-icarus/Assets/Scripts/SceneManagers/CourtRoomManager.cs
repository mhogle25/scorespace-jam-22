using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;

public class CourtRoomManager : MonoBehaviour
{
    [Header("Fallen")]
    [SerializeField] private Fallen initFallenPrefab = null;
    [SerializeField] private List<Fallen> fallenPrefabs = new();
    [Header("Scene References")]
    [SerializeField] private BF2D.UI.DialogTextbox dialogTextbox;
    [SerializeField] private Transform background = null;
    [SerializeField] private Lever lever = null;
    [SerializeField] private TextMeshProUGUI scoreDisplay = null;
    [Header("Misc")]
    [SerializeField] private Image overlay = null;
    [SerializeField] private float overlayFadeRate = 0.001f;

    private Queue<Fallen> fallenQueue = new();

    public Score GetScore
    {
        get
        {
            return this.currentScore;
        }
    }
    private Score currentScore = new();

    struct ResponseAction
    {
        public int score;
        public int morale;
        public int bribe;
    }

    Action state;

    private void Awake()
    {
        state = StateFadeIn;
    }

    private void Update()
    {
        state?.Invoke();
        this.scoreDisplay.text = $"Score: {this.currentScore.score}\nBribes: {this.currentScore.bribes}";
    }

    private void StateFadeIn()
    {
        if (!this.overlay.gameObject.activeSelf) this.overlay.gameObject.SetActive(true);

        if (this.overlay.color.a < this.overlayFadeRate)
        {
            this.overlay.gameObject.SetActive(false);
            this.state = null;
            StartCoroutine(InitBegin());
            return;
        }

        this.overlay.color -= new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, this.overlayFadeRate);
    }

    private IEnumerator InitBegin()
    {
        yield return new WaitForSeconds(3);
        dialogTextbox.Message("[N:Inquisitor][S:0.1]Have the first fallen enter.", () =>
        {
            this.fallenQueue.Enqueue(this.initFallenPrefab);
            System.Random rand = new System.Random();
            List<Fallen> shuffled = this.fallenPrefabs.OrderBy(_ => rand.Next()).ToList();
            foreach (Fallen f in fallenPrefabs)
            {
                this.fallenQueue.Enqueue(f);
            }

            StartNextFallen();
        });
        dialogTextbox.UtilityInitialize();
    }

    private IEnumerator Begin()
    {
        yield return new WaitForSeconds(5);
        this.lever.Pull();
        yield return new WaitForSeconds(2);
        dialogTextbox.Message("[N:Inquisitor][S:0.1]Have the next fallen enter.", () =>
        {
            StartNextFallen();
        });
        dialogTextbox.UtilityInitialize();
    }

    private void StartNextFallen()
    {
        if (this.fallenQueue.Count < 1)
        {
            BeginGodEncounter();
            return;
        }

        Fallen fallen = this.fallenQueue.Dequeue();
        fallen = Instantiate(fallen);
        fallen.transform.SetParent(this.background);
        fallen.transform.localScale = Vector3.one;
        fallen.Begin(dialogTextbox, lever, () =>
        {
            PullLever(fallen);
        });
    }

    public void UpdateScore(string actionItem) {
        ResponseAction action = JsonConvert.DeserializeObject<ResponseAction>(actionItem);
        currentScore.score += action.score;
        currentScore.bribes += action.bribe;
        currentScore.morale += action.morale;
    }

    public void UpdateScore(int value)
    {
        if (currentScore.score < 1 && value < 0)
            return;

        currentScore.score += value;
    }

    private void PullLever(Fallen fallen)
    {
        this.lever.Pull();
        fallen.Fall();
        StartCoroutine(Begin());
    }

    private void BeginGodEncounter()
    {

    }

    private void FinalizeGame()
    {

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}