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
        this.interactive = false;
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
        this.leverHighlight.gameObject.SetActive(unpulled && interactive);
    }

    void OnMouseExit()
    {
        this.leverHighlight.gameObject.SetActive(false);
    }

}
