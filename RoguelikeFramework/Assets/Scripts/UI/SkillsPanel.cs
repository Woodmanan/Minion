using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsPanel : MonoBehaviour
{
    public SkillTree tree;
    public Transform skillUIParent;
    public GameObject skillUIObject;

    // Start is called before the first frame update
    void Start()
    {
        tree.InitializeAvailableSkills();
        ReloadAvailableSkills();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        ReloadAvailableSkills();
    }

    private void OnDisable()
    {
        EraseAvailableSkills();
    }

    public void ReloadAvailableSkills()
    {
        foreach (SkillNode skillNode in tree.GetAvailableSkillNodes())
        {
            GameObject skillDisplayObject = Instantiate(skillUIObject, skillUIParent);
            SkillUIItemChunk chunk = skillDisplayObject.GetComponent<SkillUIItemChunk>();
            chunk.skillNode = skillNode;
            chunk.parentSkillsPanel = this;
        }
    }

    private void EraseAvailableSkills()
    {
        foreach (Transform skillChild in skillUIParent) Destroy(skillChild.gameObject);
    }
}
