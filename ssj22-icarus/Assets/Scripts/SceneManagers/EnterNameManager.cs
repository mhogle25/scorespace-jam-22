using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterNameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private int characterCap = 3;

    public void OnEndEdit(string value)
    {
        GlobalManager.Instance.newScore.name = value;
        GlobalManager.Instance.AddNewScore();
        GlobalManager.Instance.SaveGame();
        SceneManager.LoadScene("High Scores");
    }

    public void OnValueChanged(string value)
    {
        if (value.Length > characterCap)
        {
            this.inputField.text = value.Substring(0, characterCap);
        }
    }
}
