using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOverlay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer overlay = null;
    [SerializeField] private Sprite openHatch = null;
    [SerializeField] private Sprite closedHatch = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private List<Sprite> fallingSprites = new();
    [SerializeField] private float fallingSpeed = 0.2f;

    Action state;

    private float timeAccumulator = 0;
    private void FixedUpdate()
    {
        if (Time.time > this.timeAccumulator)
        {
            this.timeAccumulator = Time.time + this.fallingSpeed;       //Implement time increment
            state?.Invoke();
        }
    }

    public void ChangeToOpenHatch()
    {
        this.overlay.sprite = openHatch;
    }

    public void ChangeToClosedHatch()
    {
        this.overlay.sprite = closedHatch;
        audioSource.Play();
    }

    public void ChangeToFalling()
    {
        state = StateAnimateFalling;
    }

    int i = 0;
    private void StateAnimateFalling()
    {
        if (i >= this.fallingSprites.Count)
        {
            i = 0;
            this.state = null;
            this.gameObject.SetActive(false);
            ChangeToOpenHatch();
        }
        this.overlay.sprite = this.fallingSprites[i];
        i++;
    }
}   
