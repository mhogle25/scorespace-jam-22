using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallen : MonoBehaviour
{
    [SerializeField] private string dialogFileName = string.Empty;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    private CourtRoomManager courtRoomManager = null;
    private BF2D.UI.DialogTextbox dialogTextbox = null;
    private Action callback = null;

    public void Begin(CourtRoomManager courtRoomManager, BF2D.UI.DialogTextbox dialogTextbox, Action callback)
    {
        this.courtRoomManager = courtRoomManager;
        this.dialogTextbox = dialogTextbox;
        this.callback = callback;
    }
}
