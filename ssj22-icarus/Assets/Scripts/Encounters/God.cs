using BF2D.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class God : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private DialogTextbox dialogTextbox = null;
    [SerializeField] private Timer timer = null;
    [Header("Audio")]
    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private AudioSource sfxSource = null;
    [SerializeField] private AudioClip song = null;
    [Header("Misc")]
    [SerializeField] private float spriteFadeRate = 0.5f;
    [SerializeField] private string voiceKey = "god";
    [SerializeField] private float timerDefaultSpeed = 0.1f;
    [SerializeField] private float bribeFactor = 0.5f;

    Action state;
    Score currentScore = null;
    Stack<Fallen> fallenBin = null;
    Action callback;


    private void Update()
    {
        state?.Invoke();
    }

    public void Begin(Score currentScore, Stack<Fallen> fallen, Action callback)
    {
        this.currentScore = currentScore;
        this.callback = callback;
        this.fallenBin = fallen;
        CallIntroDialog();
    }

    private void CallIntroDialog()
    {
        this.dialogTextbox.Dialog(new List<string>
        {
            $"[N:?][V:{this.voiceKey}][S:0.1]I have arrived.",
            "Be not afraid.[E]"
        },
        0,
        () => {
            this.gameObject.SetActive(true);
            this.sfxSource.Play();
            this.state = StateFadeIn;
        });
        this.dialogTextbox.UtilityInitialize();
    }

    private void StateFadeIn()
    {
        if (this.spriteRenderer.color.a >= 1f)
        {
            this.state = null;
            CallGodsDialog();
        }
        this.spriteRenderer.color += new Color(0, 0, 0, this.spriteFadeRate * Time.deltaTime);
    }

    private void CallGodsDialog()
    {
        this.timer.Speed = this.timerDefaultSpeed;
        this.musicSource.clip = this.song;
        this.musicSource.Play();

        this.dialogTextbox.Dialog(new List<string>
        {
            $"[N:God][V:{this.voiceKey}][S:0.1]Bring to me what you have gathered",
            "I hope I am not dissapointed[E]"
        },
        0,
        () =>
        {
            this.dialogTextbox.VoiceMuted = true;
        }
        );

        string plural = this.currentScore.bribes == 1 ? "" : "s";
        string message = this.currentScore.bribes < 1 ? "[N:-1][V:-1][S:-1](The response timer is faster now)" : $"(Since you took {this.currentScore.bribes} bribe{plural}, the response timer will be slower than usual)";
        this.dialogTextbox.Message(message, () =>
        {
            this.dialogTextbox.VoiceMuted = false;
        });
        for (int i = 0; i < this.currentScore.bribes; i++)
        {
            this.timer.Speed += this.bribeFactor;
        }

        while (this.fallenBin.Count > 0)
        {
            Fallen fallen = this.fallenBin.Pop();
            this.dialogTextbox.Dialog($"God-{fallen.DialogFileName}", 0);
            Destroy(fallen.gameObject);
        }

        this.dialogTextbox.OnEndOfQueuedDialogs.AddListener(() =>
        {
            FinalizeGod();
        });

        this.dialogTextbox.UtilityInitialize();
    }

    private void FinalizeGod()
    {

    }
}
