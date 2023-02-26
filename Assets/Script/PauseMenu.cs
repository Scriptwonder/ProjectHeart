using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void ExitLevelButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
