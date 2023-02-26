using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public bool inOrOut = false;
    public Image fadeOutImage;
    public float FadeRate = 0.01f;
     
    private bool isFading = false;

    // Start is called before the first frame update
    void Start()
    {
        Color colorImage = fadeOutImage.color;
        if (inOrOut) {
            colorImage.a = 1;
        } else {
            colorImage.a = 0;
        }
        fadeOutImage.color = colorImage;

        doFade();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void doFade() {
        if (inOrOut) {
            StartCoroutine(FadeIn(0f));
        } else {
            StartCoroutine(FadeIn(1f));
        }
    }

    IEnumerator FadeIn(float targetA) {
        Color curColor = fadeOutImage.color;
        while (Mathf.Abs(curColor.a - targetA) > 0.001f) {
            curColor.a = Mathf.Lerp(curColor.a, targetA, FadeRate * Time.deltaTime);
            fadeOutImage.color = curColor;
            yield return null;
        }
    }
}
