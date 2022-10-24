using BF2D.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class God : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private float fadeRate = 0.001f;
    [SerializeField] private DialogTextbox dialogTextbox = null;

    Action state;

    private void Update()
    {
        state?.Invoke();
    }

    public void Begin(Action callback)
    {
        this.state = StateFadeIn;
    }

    private void CallIntroDialog()
    {
        this.dialogTextbox.Dialog(new List<string>
        {
            "[N:?][V:medium][S:0.1]I have arrived.",
            "Be not afraid."
        },
        0,
        () => {

        });
    }

    private void StateFadeIn()
    {
        if (this.spriteRenderer.color.a >  1 - fadeRate)
        {
            this.state = null;

        }
        this.spriteRenderer.color += new Color(0, 0, this.fadeRate);
    }

    private void CallGodsDialog()
    {

    }
}
