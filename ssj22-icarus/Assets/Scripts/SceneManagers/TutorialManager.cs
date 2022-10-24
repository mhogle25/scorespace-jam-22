using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image overlay = null;
    [SerializeField] private SpriteRenderer fallen = null;
    [SerializeField] private SpriteRenderer openHatch = null;
    [SerializeField] private SpriteRenderer emptyDesk = null;
    [SerializeField] private SpriteRenderer whiteWings = null;
    [SerializeField] private float overlayFadeRate = 0.01f;
    [SerializeField] private AudioSource audioSource = null;

    Action state;

    private void Awake()
    {
        this.state = StateFadeIn;
    }

    private void Update()
    {
        this.state?.Invoke();
    }

    private void StateFadeIn()
    {
        if (!this.overlay.gameObject.activeSelf) this.overlay.gameObject.SetActive(true);

        if (this.overlay.color.a < this.overlayFadeRate)
        {
            this.overlay.gameObject.SetActive(false);
            this.state = null;
            this.audioSource.Play();
            StartCoroutine(Timeline());
            return;
        }

        this.overlay.color -= new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, this.overlayFadeRate);
    }

    private void StateFadeOut()
    {
        if (!this.overlay.gameObject.activeSelf) this.overlay.gameObject.SetActive(true);

        if (this.overlay.color.a > 1 - this.overlayFadeRate)
        {
            if (GlobalManager.Instance.FirstTime)
            {
                GlobalManager.Instance.FirstTime = false;
                SceneManager.LoadScene("Court Room");
            }
            else
            {
                SceneManager.LoadScene("Main Menu");
            }
        }

        this.overlay.color += new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, this.overlayFadeRate);
    }

    private IEnumerator Timeline()
    {
        yield return new WaitForSeconds(3.5f);
        this.fallen.gameObject.SetActive(true);
        yield return new WaitForSeconds(8f);
        this.openHatch.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        this.emptyDesk.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.5f);
        this.emptyDesk.gameObject.SetActive(false);
        this.openHatch.gameObject.SetActive(false);
        this.fallen.gameObject.SetActive(false);
        yield return new WaitForSeconds(4.5f);
        this.whiteWings.gameObject.SetActive(true);
        yield return new WaitForSeconds(5.5f);
        this.state = StateFadeOut;
    }
}
