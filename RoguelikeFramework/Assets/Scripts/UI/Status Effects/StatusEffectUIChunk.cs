using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatusEffectUIChunk : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Effect statusEffect;
    public Image statusEffectImage;

    // Start is called before the first frame update
    void Start()
    {
        if (statusEffect.statusIcon) statusEffectImage.sprite = statusEffect.statusIcon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StatusEffectPopupWindow.singleton.effect = statusEffect;
        StatusEffectPopupWindow.singleton.group.alpha = 1;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StatusEffectPopupWindow.singleton.effect = null;
        StatusEffectPopupWindow.singleton.group.alpha = 0;
    }
}
