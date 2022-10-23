using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallen : MonoBehaviour
{
    [SerializeField] private string dialogFileName = string.Empty;
    [SerializeField] private float speed = 8f;

    //private CourtRoomManager courtRoomManager = null;
    private BF2D.UI.DialogTextbox dialogTextbox = null;
    private Action callback = null;

    private Action state;
    private void Update()
    {
        this.state?.Invoke();    
    }

    public void Begin(CourtRoomManager courtRoomManager, BF2D.UI.DialogTextbox dialogTextbox, Action callback)
    {
        //this.courtRoomManager = courtRoomManager;
        this.dialogTextbox = dialogTextbox;
        this.callback = callback;

        this.transform.localPosition = Vector3.zero;
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
            Destroy(this.gameObject);
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
        this.dialogTextbox.Dialog(dialogFileName, 0, () =>
        {
            FinalizeFallen();
        });
        this.dialogTextbox.UtilityInitialize();
    }

    private void FinalizeFallen()
    {
        this.callback?.Invoke();
    }
}
