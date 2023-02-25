using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerPoint : MonoBehaviour
{
    public bool isEndPoint = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D (Collider2D col) {
        int sceneId = CharacterSystem.instance.sceneId;
        if (isEndPoint && (col.gameObject.tag == "Player")) {
            if (sceneId == 0 && sceneId == 1) {
                SceneManager.LoadScene("scene" + (sceneId+1));
            } else {
                //play animation and show the transformation
                //TODO
            }

        }
    }
}
