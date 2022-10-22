using UnityEngine;
using UnityEngine.Events;
using System;

public class PauseMenu : MonoBehaviour
{
    private void Awake()
    {
        listeners += OptionsListener;
        paused = false;
    }
    private void Update()
    {
        listeners?.Invoke();
    }

    [SerializeField] private UnityEvent<bool> onPauseKey = new();

    private static bool paused = false;
    Action listeners;

    public void Pause()
    {
        PauseMenu.paused = !PauseMenu.paused;
        onPauseKey.Invoke(paused);
    }

    private void OptionsListener()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }
}
