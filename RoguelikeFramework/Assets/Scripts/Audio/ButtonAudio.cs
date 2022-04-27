using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    
    public void Mouseover() {
        AudioManager.i.UIMouseover();
    }

    public void Select() {
        AudioManager.i.UISelect();
    }
}
