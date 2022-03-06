using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUIItemChunk : MonoBehaviour
{
    public SkillNode skillNode;
    public Image skillImage;
    public TextMeshProUGUI skillName;

    public SkillsPanel parentSkillsPanel;

    void Start()
    {
        skillImage.sprite = skillNode.GetCurrentSkill().icon;
        skillName.text = skillNode.GetCurrentSkill().name;
    }

    public void Purchase()
    {
        skillNode.Purchase();
        parentSkillsPanel.gameObject.SetActive(false);
    }
}
