using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EventTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private int totalPlayer = 0;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.enabled = false;
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
            Debug.Log(1);
            videoPlayer.enabled = true;
            videoPlayer.Play();
        }
    }
}
