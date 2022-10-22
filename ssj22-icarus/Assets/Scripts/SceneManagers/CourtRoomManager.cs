using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class CourtRoomManager : MonoBehaviour
{
    [SerializeField] private Fallen initFallen = null;
    [SerializeField] private List<Fallen> fallenList = new();
    [SerializeField] private BF2D.UI.DialogTextbox dialogTextbox;
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

    Action state;

    private void Awake()
    {
        state = StateFadeIn;
    }

    private void Update()
    {
        state?.Invoke();
    }

    private void StateFadeIn()
    {
        if (!this.overlay.gameObject.activeSelf) this.overlay.gameObject.SetActive(true);

        if (this.overlay.color.a < this.overlayFadeRate)
        {
            this.overlay.gameObject.SetActive(false);
            this.state = null;
            StartCoroutine(Begin());
            return;
        }

        this.overlay.color -= new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, this.overlayFadeRate);
    }

    private IEnumerator Begin()
    {
        yield return new WaitForSeconds(3);
        dialogTextbox.Message("[N:Inquisitor][S:0.1]Have the first fallen enter.", () =>
        {
            this.fallenQueue.Enqueue(this.initFallen);
            System.Random rand = new System.Random();
            List<Fallen> shuffled = this.fallenList.OrderBy(_ => rand.Next()).ToList();
            foreach (Fallen f in fallenList)
            {
                this.fallenQueue.Enqueue(f);
            }

            StartNextFallen();
        });
        dialogTextbox.UtilityInitialize();
    }

    private void StartNextFallen()
    {
        Fallen fallen = this.fallenQueue.Dequeue();
        fallen.Begin(this, dialogTextbox, () =>
        {
            FinalizeEncounter();
        });
    }

    private void FinalizeEncounter()
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