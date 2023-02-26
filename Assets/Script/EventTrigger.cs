using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EventTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public SpriteRenderer sprite;
    private int totalPlayer = 0;
    public GameObject image;
    public GameObject character1;
    public GameObject character2;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        //sprite.color = new Color(1f,1f,1f,0f);
        sprite.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player" && totalPlayer != 1) {
            CharacterController characterSystem = col.gameObject.GetComponent<CharacterController>();
            characterSystem.enabled = false;
            characterSystem.resetSpeed();
            //swap
            CameraFollow.instance.target2.gameObject.GetComponent<CharacterController>().canSwap = false;
            CameraFollow.instance.swapTarget();
            totalPlayer++;
        } else {
            //playEvent

            videoPlayer.enabled = true;
            videoPlayer.Play();
            character1.GetComponent<CharacterController>().enabled = false;
            character1.GetComponent<CharacterController>().StopMovement();
            character2.GetComponent<CharacterController>().enabled = false;
            character2.GetComponent<CharacterController>().StopMovement();
            StartCoroutine(justTwoSecs());
            //sprite.enabled = true;
            //sprite.color = new Color(1f,1f,1f,1f);
        }
    }

    IEnumerator justTwoSecs() {
        yield return new WaitForSeconds(1.25f);
        sprite.enabled = true;
        yield return new WaitForSeconds(12f);
        image.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("3rd Level");
    }


}
