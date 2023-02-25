using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CharacterSystem : MonoBehaviour
{
    public int sceneId = 0;
    public int characterId = 0;//0 dash; 1 double-jump; 2 dash/double-jump

    public bool playerHit = false;
    public CharacterController characterController;

    public Transform startPoint;

    public static CharacterSystem instance = null;
    private Collider2D hitCollider;

    void Awake() {
        // if (instance == null) {
        //     instance = this;
        // } else if (instance != this) {
        //     Destroy(gameObject);
        // }
    }

    void Start()
    {
        
    }

    void update() {
        if (playerHit) {
            playerHit = false;
            //play animation
            restart();
        }
    }

    public void restart() {
        //restart to the spawn point
        this.gameObject.transform.position = startPoint.position;
    }
}
