using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainController : MonoBehaviour
{
    public static MainController instance;
    public static MainController Instance { get { return instance; } }

    private Camera mainCamera;
    public enum ColorProfile {Light, Dark};
    public ColorProfile colorProfile;
    private GameObject mainCanvas;
    public Sprite gameOrTeamIcon;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCanvas = GameObject.Find("MainCanvas");

        mainCamera = Camera.main;
        switch (colorProfile)
        {
            case ColorProfile.Light:
                mainCamera.backgroundColor = Color.white;
                mainCanvas.GetComponent<GCSLogo>().textColor = new Color(35f / 255f, 35f / 255f, 35f / 255f);
                break;
            case ColorProfile.Dark:
                mainCamera.backgroundColor = new Color(32f / 255f, 44f / 255f, 62 / 255f);
                mainCanvas.GetComponent<GCSLogo>().textColor = new Color(95f / 255f, 163f / 255f, 212f / 255f);
                break;
        }
    }
}
