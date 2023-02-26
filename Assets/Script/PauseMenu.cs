using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject character;
    public GameObject levelOnePause;
    public GameObject levelTwoPause;
    private CharacterSystem system;
    private int characterId;

    void Start()
    {
        system = character.GetComponent<CharacterSystem>();
        characterId = character.GetComponent<CharacterSystem>().characterId;
        updateBar(characterId);
    }

    void Update()
    {
        characterId = character.GetComponent<CharacterSystem>().characterId;
        updateBar(characterId);
    }

    public void RestartLevelButton()
    {
        system.restart();
    }

    public void ExitLevelButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void updateBar(int id)
    {
        if (id == 0)
        {
            levelOnePause.SetActive(true);
            levelTwoPause.SetActive(false);
        }
        else if (id == 1)
        {
            levelOnePause.SetActive(false);
            levelTwoPause.SetActive(true);
        }
    }
}
