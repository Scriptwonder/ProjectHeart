using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public GameObject character;
    public GameObject levelOneBar;
    public GameObject levelTwoBar;
    private int characterId;

    void Start()
    {
        characterId = character.GetComponent<CharacterSystem>().characterId;
        updateBar(characterId);
    }

    void Update()
    {
        characterId = character.GetComponent<CharacterSystem>().characterId;
        updateBar(characterId);
    }

    private void updateBar(int id)
    {
        if (id == 0)
        {
            levelOneBar.SetActive(false);
            levelTwoBar.SetActive(true);
        }
        else if (id == 1)
        {
            levelOneBar.SetActive(false);
            levelTwoBar.SetActive(true);
        }
    }
}
