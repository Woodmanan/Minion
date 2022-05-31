using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GCSLogo : MonoBehaviour
{
    public Sprite gcsLogo;
    public Sprite secondaryLogo;

    [Range(0, 100)]
    public float percentOfScreenUsed = 60f;

    public bool matchSplashScreenColor = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunAnim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator RunAnim()
    {
        yield break;
    }

    
    void EditorOnlyUpdates()
    {
#if UNITY_EDITOR
        if (gcsLogo == null)
        {
            Debug.LogError("Please attach an image to the GCS Logo object!");
        }

        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Please create a canvas in the scene! It can be empty.");
            return;
        }

        if (canvas.transform.childCount == 0)
        {
            Debug.Log("Detected that canvas was not set up. Adding those elements now.");
            GameObject sizeContainer = new GameObject("Sizing Container", typeof(RectTransform));
            sizeContainer.transform.parent = canvas.transform;

            GameObject aspectContainer = new GameObject("Aspect Container", typeof(RectTransform), typeof(AspectRatioFitter));
            aspectContainer.transform.parent = sizeContainer.transform;
        }


        RectTransform sizingRect = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        {
            
            float offset = (1 - (percentOfScreenUsed / 100f)) / 2;
            sizingRect.anchorMin = Vector2.one * offset;
            sizingRect.anchorMax = Vector2.one - sizingRect.anchorMin;

            sizingRect.sizeDelta = Vector2.zero;

        }

        if (matchSplashScreenColor && Camera.main.backgroundColor != PlayerSettings.SplashScreen.backgroundColor)
        {
            Debug.Log("The camera background was set to something different than the splash scene color. Correcting that now.");
            Camera.main.backgroundColor = PlayerSettings.SplashScreen.backgroundColor;
        }
        

#endif
    }

    private void OnValidate()
    {
        EditorOnlyUpdates();
    }

}
