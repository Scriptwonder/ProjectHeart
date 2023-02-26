using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class EventTrigger3rdLvl : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public SpriteRenderer sprite;
    private int totalPlayer = 0;
    public GameObject image;
    public GameObject character;

    public GameObject thankYouText;
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

    void OnTriggerEnter2D(Collider2D col)
    {

        //playEvent

        videoPlayer.enabled = true;
        videoPlayer.Play();
        character.GetComponent<CharacterController>().enabled = false;
        character.GetComponent<CharacterController>().StopMovement();
        //character.GetComponent<Animator>().SetTrigger("Idle");
        col.gameObject.SetActive(false);
        StartCoroutine(justTwoSecs());
        //sprite.enabled = true;
        //sprite.color = new Color(1f,1f,1f,1f);
    }

    IEnumerator justTwoSecs()
    {
        yield return new WaitForSeconds(1.25f);
        sprite.enabled = true;
        yield return new WaitForSeconds(6f);
        image.SetActive(true);
        yield return new WaitForSeconds(3f);
        thankYouText.SetActive(true);
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("MainMenu");
    }


}

