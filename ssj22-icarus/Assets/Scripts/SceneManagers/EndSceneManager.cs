using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneManager : MonoBehaviour
{
    [SerializeField] private Image overlay = null;
    [SerializeField] private SpriteRenderer white = null;
    [SerializeField] private SpriteRenderer black = null;
    [SerializeField] private float overlayFadeRate = 0.01f;

    Action state;

    private void Awake()
    {
        if (GlobalManager.Instance.newScore.morale > 2)
        {
            this.black.gameObject.SetActive(true);
        } else
        {
            this.white.gameObject.SetActive(true);
        }
        
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
            StartCoroutine(Delay());
            return;
        }

        this.overlay.color -= new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, this.overlayFadeRate);
    }

    private void StateFadeOut()
    {
        if (!this.overlay.gameObject.activeSelf) this.overlay.gameObject.SetActive(true);

        if (this.overlay.color.a > 1 - this.overlayFadeRate)
        {
            SceneManager.LoadScene("Enter Name");
        }

        this.overlay.color += new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, this.overlayFadeRate);
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3);
        this.state = StateFadeOut;
    }
}
