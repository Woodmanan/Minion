using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterAnimator : MonoBehaviour
{
    public enum LetterColor {Red, Blue};
    public Sprite redSprite;
    public Sprite blueSprite;

    public RectTransform logoSizing;
    RectTransform rTransform;

    public Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        switch (MainController.instance.colorProfile)
        {
            case MainController.ColorProfile.Light:
                GetComponent<Image>().sprite = redSprite;
                break;
            case MainController.ColorProfile.Dark:
                GetComponent<Image>().sprite = blueSprite;
                break;
        }

        //Ensure letter image enabled
        GetComponent<Image>().enabled = true;

        rTransform = transform as RectTransform;
        rigid = GetComponent<Rigidbody2D>();
        Resize();
    }

    // Update is called once per frame
    void Update()
    {
        Resize();
    }

    private void Resize()
    {
        if (logoSizing == null)
        {
            Debug.LogError("Need a sizing object!", this);
            return;
        }

        rTransform.localScale = Vector3.one * (logoSizing.rect.width / 834.8f);
    }

    /*private void OnValidate()
    {
        rTransform = transform as RectTransform;
        Resize();
    }*/

    public void DisableCollider()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void PlayNoise()
    {
        GetComponent<AudioSource>().Play();
    }
}
