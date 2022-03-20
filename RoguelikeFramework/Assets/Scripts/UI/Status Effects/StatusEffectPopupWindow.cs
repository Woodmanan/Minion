using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatusEffectPopupWindow : MonoBehaviour
{
    public TextMeshProUGUI nameBox;
    public TextMeshProUGUI turnsRemainingBox;
    public TextMeshProUGUI descriptionBox;

    public Effect effect;

    private static StatusEffectPopupWindow Singleton;
    public static StatusEffectPopupWindow singleton 
    {
        get
        {
            if (!Singleton)
            {
                Singleton = GameObject.Find("Status Effect Tooltip").GetComponent<StatusEffectPopupWindow>();
            }
            return Singleton;
        }
    }

    public CanvasGroup group;

    private void Awake()
    {
        Singleton = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (effect)
        {
            nameBox.text = effect.name;
            turnsRemainingBox.text = effect.name;   // Should be turns remaining instead
            descriptionBox.text = effect.description;
        }
    }
}
