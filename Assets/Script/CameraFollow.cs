using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;

    public static CameraFollow instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void swapTarget() {
        target.gameObject.GetComponent<CharacterController>().enabled = false;
        target2.gameObject.GetComponent<CharacterController>().enabled = true;
        // target.gameObject.SetActive(false);
        // target2.gameObject.SetActive(true);
        Transform temp = target2;
        target2 = target;
        target = temp;
    }
}
