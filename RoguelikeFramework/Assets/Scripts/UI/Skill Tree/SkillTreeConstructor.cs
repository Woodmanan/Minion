using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeConstructor : MonoBehaviour
{
    public SkillTree skillTree;
    public GameObject skillUIItem;

    int yPos = 250;

    // Start is called before the first frame update
    void Start()
    {
        foreach (SkillNode skillNode in skillTree.nodes)
        {
            GameObject uiItem = Instantiate(skillUIItem, transform);
            uiItem.transform.localPosition = new Vector3(0, yPos, 0);
            yPos -= 120;
            uiItem.GetComponent<SkillUIItem>().skillNode = skillNode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
