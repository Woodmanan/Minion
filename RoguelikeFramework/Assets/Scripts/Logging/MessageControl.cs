using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageControl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI messageTextComponent;

    public void SetMessageText(string text)
    {
        messageTextComponent.text = text;
        TextRevealer tr = GetComponent<TextRevealer>();
        if (tr.enabled) tr.TriggerRevealText(messageTextComponent);
    }
}
