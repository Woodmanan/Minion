using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public string mouseoverSfxPath;
    public string selectSfxPath;

    public void Start() {
        if(mouseoverSfxPath == "") {
            mouseoverSfxPath = "event:/SFX/UI/Mouseover";
        }
        if(selectSfxPath == "") {
            selectSfxPath = "event:/SFX/UI/Select";
        }
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        FMODUnity.RuntimeManager.PlayOneShot(mouseoverSfxPath, transform.position);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        FMODUnity.RuntimeManager.PlayOneShot(selectSfxPath, transform.position);
    }
    
}
