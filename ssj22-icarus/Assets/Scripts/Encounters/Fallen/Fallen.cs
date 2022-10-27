using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Fallen : MonoBehaviour
{
    public string DialogFileName
    {
        get
        {
            return this.dialogFileName;
        }

        set
        {
            this.dialogFileName = value;
        }
    }

    public float Speed
    {
        get
        {
            return this.speed;
        }
        set
        {
            this.speed = value;
        }
    }

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return this.spriteRenderer;
        }
    }
    [SerializeField] private string dialogFileName = string.Empty;
    [SerializeField] private float speed = 8f;
    private SpriteRenderer spriteRenderer;

    public readonly UnityEvent onDeath = new();
    private BF2D.UI.DialogTextbox dialogTextbox = null;
    private Lever lever = null;
    private Action callback = null;

    private Action state;

    private void Awake()
    {
        this.spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this.state?.Invoke();    
    }

    public void Begin(BF2D.UI.DialogTextbox dialogTextbox, Lever lever, Action callback)
    {
        this.dialogTextbox = dialogTextbox;
        this.lever = lever;
        this.callback = callback;

        this.transform.localPosition = Vector3.zero - new Vector3(0, 0.5f, 0);
        this.transform.localPosition -= new Vector3(8f, 0f, 0f);
        this.state = StateEnter;
    }

    public void Fall()
    {
        //Fall, then destroy itself
        this.state = StateFalling;
    }

    private void StateFalling()
    {
        this.transform.localPosition -= new Vector3(0f, this.speed * Time.deltaTime, 0f);

        if (this.transform.localPosition.y < -4)
        {
            this.gameObject.SetActive(false);
            this.onDeath?.Invoke();
        }
    }

    private void StateEnter()
    {
        if (this.transform.localPosition.x < 0)
        {
            this.transform.localPosition += new Vector3(this.speed * Time.deltaTime, 0f, 0f);
            return;
        }

        StartConversation();
        this.state = null;
    }

    private void StartConversation()
    {
        lever.interactive = true;
        this.dialogTextbox.Dialog(dialogFileName, 0, () =>
        {
            FinalizeFallen();
        });
        this.dialogTextbox.UtilityInitialize();
    }

    public void FinalizeFallen()
    {
        this.callback?.Invoke();
    }
}
