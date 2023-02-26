using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject levelOnePause;
    public GameObject levelTwoPause;
    public GameObject levelThreePause;
    public TMP_Text pauseText;
    private GameObject character;
    private int characterId;

    void Start()
    {
        SetActiveCharacter();
        characterId = GetActiveId();
        UpdateBar(characterId);
    }

    void Update()
    {
        SetActiveCharacter();
        characterId = GetActiveId();
        UpdateBar(characterId);
    }

    public void RestartLevelButton()
    {
        character.GetComponent<CharacterSystem>().restart();
        character.GetComponent<CharacterController>().enabled = true;
    }

    public void ExitLevelButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ToggleCharacterMovement()
    {
        character.GetComponent<CharacterController>().enabled = !character.GetComponent<CharacterController>().enabled;
    }

    public void StopCharacterMovement()
    {
        character.GetComponent<CharacterController>().StopMovement();
    }

    private void SetActiveCharacter()
    {
        character = CameraFollow.instance.target.gameObject;
    }

    private int GetActiveId()
    {
        return character.GetComponent<CharacterSystem>().characterId;
    }

    private void UpdateBar(int id)
    {
        if (id == 0)
        {
            levelOnePause.SetActive(true);
            levelTwoPause.SetActive(false);
            levelThreePause.SetActive(false);
            pauseText.GetComponent<TextMeshProUGUI>().faceColor = new Color32(197, 237, 213, 255);
        }
        else if (id == 1)
        {
            levelOnePause.SetActive(false);
            levelTwoPause.SetActive(true);
            levelThreePause.SetActive(false);
            pauseText.GetComponent<TextMeshProUGUI>().faceColor = new Color32(174, 174, 226, 255);
        }
        else if (id == 2)
        {
            levelOnePause.SetActive(false);
            levelTwoPause.SetActive(false);
            levelThreePause.SetActive(true);
            pauseText.GetComponent<TextMeshProUGUI>().faceColor = new Color32(255, 249, 108, 255);
        }
    }
}
