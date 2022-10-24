using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Image overlay;
    [SerializeField] private float overlayFadeRate = 0.001f;
    [SerializeField] private SpriteRenderer city = null;
    [SerializeField] private float citySpeed = 0.2f;
    [SerializeField] private int cityMoveThreshold = 10000;
    [SerializeField] private AudioSource music = null;
    
    Action state;

    private void Awake()
    {
        this.state += StateMoveCity;
    }

    private void Update()
    {
        this.state?.Invoke();
    }

    public void QuitGame()
    {
        GlobalManager.Instance.SaveGame();
        Application.Quit();
    }

    public void StartGame()
    {
        this.state += StateFadeOut;
    }

    private void StateFadeOut()
    {
        if (!this.overlay.gameObject.activeSelf) this.overlay.gameObject.SetActive(true);

        if (this.overlay.color.a > 1f - this.overlayFadeRate)
        {
            this.state = null;
            SceneManager.LoadScene("Court Room");
            return;
        }

        this.overlay.color += new Color(this.overlay.color.r, this.overlay.color.g, this.overlay.color.b, this.overlayFadeRate);

        this.music.volume -= this.overlayFadeRate;
    }

    int timeAccumulator = 0;
    private void StateMoveCity()
    {
        this.city.transform.localPosition += new Vector3(this.citySpeed * Time.deltaTime, 0, 0);
        timeAccumulator++;
        if (timeAccumulator > this.cityMoveThreshold)
        {
            timeAccumulator = 0;
            this.citySpeed *= -1;
        }
    }

    public void LoadHighScoresScene()
    {
        SceneManager.LoadScene("High Scores");
    }
}
