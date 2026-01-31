using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void EnterGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
