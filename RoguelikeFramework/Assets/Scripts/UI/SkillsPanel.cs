using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsPanel : RogueUIPanel
{
    public SkillTree treePrefab;
    [HideInInspector] public SkillTree tree;
    public Transform skillUIParent;
    public GameObject skillUIObject;
    private bool setup = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!setup) Setup();
        tree.InitializeAvailableSkills();
        ReloadAvailableSkills();
    }

    void Setup()
    {
        if (setup) return;
        setup = true;
        tree = treePrefab.GetCopy();
        Debug.Log("Tree is " + tree);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        if (!setup) Setup();
        ReloadAvailableSkills();
        AudioManager.i.Pause();
    }

    private void OnDisable()
    {
        EraseAvailableSkills();
        AudioManager.i.UnPause();
        Player.player.abilities?.CheckAvailability();
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
