using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Slider))]
public class Timer : MonoBehaviour
{
    [SerializeField] private Image sliderFill = null;
    [SerializeField] private int scoreModifier = -1;
    public float TimerSpeed
    {
        get
        {
            return this.timerSpeed;
        }

        set
        {
            this.timerSpeed = value;
        }
    }
    [SerializeField] private float timerSpeed = 0.5f;
    [Serializable] private class OnTimerEvent : UnityEvent<int> { }
    [SerializeField] private OnTimerEvent onTimer = new OnTimerEvent();

    private Slider slider = null;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 1;
    }

    private float timeAccumulator = 0;
    private void Update()
    {
        if (Time.time > this.timeAccumulator)
        {
            this.timeAccumulator = Time.time + this.timerSpeed;       //Implement time increment
            this.slider.value -= 0.01f;
            this.sliderFill.color -= new Color(0, 0.01f, 0.01f, 0);
        }

        if (this.slider.value < 0.01f)
        {
            this.onTimer?.Invoke(this.scoreModifier);
            slider.value = 1;
            this.sliderFill.color = Color.white;
        }
    }
}