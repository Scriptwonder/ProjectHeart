using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGameButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ConjuctionScene");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
