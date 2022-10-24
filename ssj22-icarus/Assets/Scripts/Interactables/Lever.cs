using UnityEngine;
using UnityEngine.Events;
using System;

public class Lever : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite up = null;
    [SerializeField] private Sprite down = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private SpriteRenderer leverHighlight = null;

    public UnityEvent onClick = new();
    public bool interactive = false;
    private bool unpulled = true;

    public void Pull()
    {
        audioSource.Play();
        unpulled = !unpulled;
        spriteRenderer.sprite = unpulled ? up : down;
    }

    private void OnMouseDown()
    {
        if (interactive)
        {
            onClick?.Invoke();
            interactive = false;
        }
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        Debug.Log("Mouse is over GameObject.");
        this.leverHighlight.gameObject.SetActive(unpulled);
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        Debug.Log("Mouse is no longer on GameObject.");
        this.leverHighlight.gameObject.SetActive(false);
    }

}
