using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;

public class CourtRoomManager : MonoBehaviour
{
    [SerializeField] private God god = null;
    [Header("Fallen")]
    [SerializeField] private Fallen fallenPrefab = null;
    [SerializeField] private BF2D.SpriteCollection fallenSpriteCollection = null;
    [Header("Graphic References")]
    [SerializeField] private BF2D.UI.DialogTextbox dialogTextbox;
    [SerializeField] private Transform background = null;
    [SerializeField] private Lever lever = null;
    [SerializeField] private TextMeshProUGUI scoreDisplay = null;
    [SerializeField] private TextMeshProUGUI scoreIndicator = null;
    [SerializeField] private TextMeshProUGUI bribeIndicator = null;
    [SerializeField] private DisplayOverlay displayOverlay = null;
    [Header("Audio")]
    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private AudioClip song = null;
    [Header("Misc")]
    [SerializeField] private Image overlay = null;
    [SerializeField] private float overlayFadeRate = 0.01f;

    private Register register = new();
    private readonly Queue<FallenData> fallenQueue = new();
    private readonly Stack<Fallen> fallenBin = new();

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

    struct FallenData
    {
        public string dialogFileName;
        public float speed;
        public string spriteKey;
    }

    struct Register
    {
        public int amountToQueue;
        public FallenData initFallen;
        public List<FallenData> fallen;
    }

    Action state;

    private void Awake()
    {
        state = StateFadeIn;

        string json = BF2D.Utilities.TextFile.LoadFile(Path.Combine(Application.streamingAssetsPath, "Dialog", "Dialogs", "register.json"));
        this.register = JsonConvert.DeserializeObject<Register>(json);
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

    private void StateFadeOut()
    {
        if (!this.overlay.gameObject.activeSelf) this.overlay.gameObject.SetActive(true);

        if (this.overlay.color.a > 1 - this.overlayFadeRate)
        {
            SceneManager.LoadScene("End");
        }

        this.overlay.color += new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, this.overlayFadeRate);
    }

    private IEnumerator InitBegin()
    {
        yield return new WaitForSeconds(2);
        dialogTextbox.Message("[V:medium][N:Inquisitor][S:0.1]Have the first fallen enter.", () =>
        {
            this.fallenQueue.Enqueue(this.register.initFallen);
            System.Random rand = new System.Random();
            List<FallenData> shuffled = this.register.fallen.OrderBy(_ => rand.Next()).ToList();
            for (int i = 0; i < this.register.amountToQueue; i++)
            {
                this.fallenQueue.Enqueue(shuffled[i]);
            }
            this.musicSource.clip = this.song;
            StartNextFallen();
        });
        dialogTextbox.UtilityInitialize();
    }

    private IEnumerator TransitionBetweenFallen()
    {
        this.displayOverlay.gameObject.SetActive(true);
        this.displayOverlay.ChangeToOpenHatch();
        yield return new WaitForSeconds(1);
        this.displayOverlay.ChangeToClosedHatch();
        yield return new WaitForSeconds(1);
        this.displayOverlay.ChangeToFalling();
        yield return new WaitForSeconds(5);
        this.lever.Pull();
        yield return new WaitForSeconds(2);

        if (this.fallenQueue.Count < 1)
        {
            BeginGodEncounter();
            yield break;
        }

        dialogTextbox.Message("[V:medium][N:Inquisitor][S:0.1]Have the next fallen enter.", () =>
        {
            StartNextFallen();
        });
        dialogTextbox.UtilityInitialize();
    }

    public void Continue()
    {
        StartCoroutine(TransitionBetweenFallen());
    }

    private void StartNextFallen()
    {
        Fallen fallen = Instantiate(this.fallenPrefab);
        FallenData data = this.fallenQueue.Dequeue();
        fallen.DialogFileName = data.dialogFileName;
        fallen.Speed = data.speed;
        fallen.SpriteRenderer.sprite = this.fallenSpriteCollection[data.spriteKey];
        this.fallenBin.Push(fallen);
        fallen.transform.SetParent(this.background);
        fallen.transform.localScale = Vector3.one;
        this.musicSource.Play();
        fallen.onDeath.AddListener(() =>
        {
            Continue();
        });
        fallen.Begin(dialogTextbox, lever, () =>
        {
            PullLever();
        });
    }

    public void UpdateScore(string actionItem) {
        ResponseAction action = JsonConvert.DeserializeObject<ResponseAction>(actionItem);
        IncrementScore(action.score);
        IncrementBribes(action.bribe);
        IncrementMorale(action.morale);
    }

    public void IncrementScore(int value)
    {
        if (currentScore.score <= 0 && value < 0)
            return;

        EnableIndicator(scoreIndicator, value);
        currentScore.score += value;
    }

    public void IncrementBribes(int value)
    {
        if (currentScore.bribes <= 0 && value < 0)
            return;

        if (value != 0) EnableIndicator(bribeIndicator, value);
        currentScore.bribes += value;
    }

    public void IncrementMorale(int value)
    {
        if (currentScore.morale <= 0 && value < 0)
            return;

        currentScore.morale += value;
    }

    private void EnableIndicator(TextMeshProUGUI uGUI, int value)
    {
        uGUI.gameObject.SetActive(true);
        uGUI.text = value > 0 ? $"+{value}" : $"-{value}";
        StartCoroutine(HideIndicator(uGUI));
    }

    private IEnumerator HideIndicator(TextMeshProUGUI uGUI)
    {
        yield return new WaitForSeconds(1);
        uGUI.text = string.Empty;
        uGUI.gameObject.SetActive(false);
    }

    private void PullLever()
    {
        this.lever.Pull();
        this.musicSource.Pause();
        this.fallenBin.Peek().Fall();
    }

    private void BeginGodEncounter()
    {
        Debug.Log("GOD ACTIVATE");
        this.god.Begin(
            this.currentScore,
            this.fallenBin,
            () =>
            {
                FinalizeGame();
            }
        );
    }

    private void FinalizeGame()
    {
        GlobalManager.Instance.newScore = this.currentScore;
        this.state = StateFadeOut;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}