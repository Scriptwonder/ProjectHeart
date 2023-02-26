using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject levelOnePause;
    public GameObject levelTwoPause;
    public GameObject levelThreePause;
    private GameObject character;
    private int characterId;

    void Start()
    {
        setActiveCharacter();
        characterId = getActiveId();
        updateBar(characterId);
    }

    void Update()
    {
        setActiveCharacter();
        characterId = getActiveId();
        updateBar(characterId);
    }

    public void RestartLevelButton()
    {
        character.GetComponent<CharacterSystem>().restart();
    }

    public void ExitLevelButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void setActiveCharacter()
    {
        character = CameraFollow.instance.target.gameObject;
    }

    private int getActiveId()
    {
        return character.GetComponent<CharacterSystem>().characterId;
    }

    private void updateBar(int id)
    {
        if (id == 0)
        {
            levelOnePause.SetActive(true);
            levelTwoPause.SetActive(false);
            levelThreePause.SetActive(false);
        }
        else if (id == 1)
        {
            levelOnePause.SetActive(false);
            levelTwoPause.SetActive(true);
            levelThreePause.SetActive(false);
        }
        else if (id == 2)
        {
            levelOnePause.SetActive(false);
            levelTwoPause.SetActive(false);
            levelThreePause.SetActive(true);
        }
    }
}
