using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoverText : MonoBehaviour
{
    public Text textContent;
    public GameObject player;
    private Color color;
    public float radius = 10f;
    private bool enabled = false;

    void Start() {
        color = textContent.color;
        color.a = 0;
        textContent.color = color;
    }

    void Update() {
        if (enabled) {
            //show the text
            float dis = Vector3.Distance(player.transform.position, this.transform.position);
            if (dis > 2) {
                color.a = Mathf.Lerp(1.0f, 0.0f, dis/radius);
            } else {
                color.a = 1;
            }
            textContent.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            player = col.gameObject;
            color.a = 0;
            textContent.color = color;
            enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            color.a = 0;
            textContent.color = color;
            enabled = false;
        }
    }
}
