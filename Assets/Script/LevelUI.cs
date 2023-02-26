using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    public GameObject levelOneBar;
    public GameObject levelTwoBar;
    private int characterId;

    void Start()
    {
        characterId = getActiveId();
        updateBar(characterId);
    }

    void Update()
    {
        characterId = getActiveId();
        updateBar(characterId);
    }

    private int getActiveId()
    {
        return CameraFollow.instance.target.gameObject.GetComponent<CharacterSystem>().characterId;
    }

    private void updateBar(int id)
    {
        if (id == 0)
        {
            levelOneBar.SetActive(true);
            levelTwoBar.SetActive(false);
        }
        else if (id == 1)
        {
            levelOneBar.SetActive(false);
            levelTwoBar.SetActive(true);
        }
    }
}
