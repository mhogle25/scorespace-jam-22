using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        GlobalManager.Instance.SaveGame();
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Court Room");
    }
}
