using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndItem : MonoBehaviour
{
    public GameObject key;
    
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            Destroy(key);

            //go into cutScene

            //play music
        }

    }
}
