using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOverlay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer overlay = null;
    [SerializeField] private Sprite openHatch = null;
    [SerializeField] private Sprite closedHatch = null;

    public void ChangeToOpenHatch()
    {
        this.overlay.sprite = openHatch;
    }

    public void ChangeToClosedHatch()
    {
        this.overlay.sprite = closedHatch;
    }

    public void ChangeToFalling()
    {

    }
}   
