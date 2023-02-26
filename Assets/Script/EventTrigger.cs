using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    private int totalPlayer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player" && totalPlayer != 1) {
            CharacterController characterSystem = col.gameObject.GetComponent<CharacterController>();
            characterSystem.enabled = false;
            //swap
            CameraFollow.instance.target2.gameObject.GetComponent<CharacterController>().canSwap = false;
            CameraFollow.instance.swapTarget();
            totalPlayer++;
        } else {
            //playEvent

        }
    }
}
