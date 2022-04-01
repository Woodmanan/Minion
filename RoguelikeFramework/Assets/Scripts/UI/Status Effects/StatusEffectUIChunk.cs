using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StatusEffectUIChunk : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Effect statusEffect;
    public Image statusEffectImage;
    private EffectDisplayInfo info;
    public StatusImageListObject imageListObject;

    // Start is called before the first frame update
    void Start()
    {
        info = statusEffect.GetDisplayInfo();
        if (info == null) Destroy(gameObject);
        else
        {
            statusEffectImage.sprite = imageListObject.statusSprites[info.imageID];
            print(imageListObject.statusSprites[info.imageID]);
            Player.player.connections.OnTurnStartLocal.AddListener(1000, UpdateMe);
        }
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

    private void UpdateMe()
    {
        info = statusEffect.GetDisplayInfo();
        statusEffectImage.sprite = imageListObject.statusSprites[info.imageID];
    }

    private void OnDestroy()
    {
        Player.player.connections.OnTurnStartLocal.RemoveListener(UpdateMe);
    }
}
