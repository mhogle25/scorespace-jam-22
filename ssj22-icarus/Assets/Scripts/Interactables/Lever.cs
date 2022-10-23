using UnityEngine;
using UnityEngine.Events;
using System;

public class Lever : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Sprite up = null;
    [SerializeField] private Sprite down = null;
    [SerializeField] private AudioSource audioSource = null;

    public UnityEvent onClick = new();
    public bool interactive = false;

    private bool flag = true;

    public void Pull()
    {
        audioSource.Play();
        flag = !flag;
        spriteRenderer.sprite = flag ? up : down;
    }

    private void OnMouseDown()
    {
        if (interactive)
        {
            onClick?.Invoke();
            interactive = false;
        }
    }
}
