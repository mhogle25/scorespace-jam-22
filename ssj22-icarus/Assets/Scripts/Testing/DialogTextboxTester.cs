using BF2D.UI;
using System.Collections.Generic;
using UnityEngine;

public class DialogTextboxTester : MonoBehaviour
{
    public void CallTestDialogs(DialogTextbox dialogTextbox)
    {
        dialogTextbox.Dialog("test", 2);
        dialogTextbox.Message("[N:Mr. Cool Guy]Hey hi I'm Mr. Cool Guy.");
        dialogTextbox.Dialog(new List<string> {
                "[N:Jim]Hi",
                "[N:-1]Hello",
                "[N:Giuseppe]Whaddup[E]"
            },
            0
        );
    }
}
