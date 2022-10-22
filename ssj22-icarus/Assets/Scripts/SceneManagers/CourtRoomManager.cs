using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class CourtRoomManager : MonoBehaviour
{
    [SerializeField] private BF2D.UI.DialogTextbox dialogTextbox;
    [SerializeField] private Image overlay = null;
    [SerializeField] private float overlayFadeRate = 0.001f;
    [SerializeField] private UnityEvent<bool> onPauseKey = new();

    private static bool paused = false;

    Action state;
    Action listeners;

    private void Awake()
    {
        listeners += OptionsListener;
        state = StateFadeIn;
        paused = false;
    }

    private void Update()
    {
        state?.Invoke();
        listeners?.Invoke();
    }

    private void OptionsListener()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
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
            //Start the queue
        });
        dialogTextbox.UtilityInitialize();
    }

    public void Pause()
    {
        CourtRoomManager.paused = !CourtRoomManager.paused;
        onPauseKey.Invoke(paused);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}