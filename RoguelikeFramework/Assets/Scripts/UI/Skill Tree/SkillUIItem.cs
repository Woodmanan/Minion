using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUIItem : MonoBehaviour
{
    public SkillNode skillNode;

    public Image skillImage;
    public TextMeshProUGUI name;
    public TextMeshProUGUI desc;
    public Button btn;
    public TextMeshProUGUI count;

    // Start is called before the first frame update
    void Start()
    {
        count.text = skillNode.skillLevel + "/" + skillNode.skills.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (skillNode.GetCurrentSkill())
        {
            skillImage.sprite = skillNode.GetCurrentSkill().icon;
            name.text = skillNode.GetCurrentSkill().name;
            desc.text = skillNode.GetCurrentSkill().description;
            btn.interactable = (bool) skillNode.GetValue(skillNode.GetPort("canBePurchased"));
        }
    }

    public void Purchase()
    {
        skillNode.Purchase();
        count.text = skillNode.skillLevel + "/" + skillNode.skills.Length;
    }
}
