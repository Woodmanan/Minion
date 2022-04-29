using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMatching : MonoBehaviour
{
    public TextMeshProUGUI other;
    public string referenceText;

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.fontSize = other.fontSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (!other.text.Equals(referenceText))
        {
            other.text = referenceText;
        }
        if (text.fontSize != other.fontSize)
        {
            text.fontSize = other.fontSize;
        }
    }
}
