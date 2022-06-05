using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;

[CreateAssetMenu]
public class SkillTree : NodeGraph {

    public int maxToDisplayOnLevelUp = 5;
    public HashSet<SkillNode> availableSkills = new HashSet<SkillNode>();

    [SerializeField] private bool resetSkillLevelsOnRestart = true;

    public void InitializeAvailableSkills()
    {
        // Initializing the queue and dictionaries with the root nodes
        foreach (SkillNode skillNode in nodes)
        {
            if (resetSkillLevelsOnRestart) skillNode.ResetSkillLevel();

            if (skillNode.IsRoot())
            {
                // Only used for roots right now, probably implement this as a callback as well
                availableSkills.Add(skillNode);
            }
        }
    }

    public SkillNode[] GetAvailableSkillNodes()
    {
        System.Random r = new System.Random();
        return availableSkills.OrderBy(skillNode => r.Next()).Take(maxToDisplayOnLevelUp).ToArray();
    }

    public void AddToAvailableSkills(SkillNode node)
    {
        availableSkills.Add(node);
    }

    public void RemoveFromAvailableSkills(SkillNode node)
    {
        availableSkills.Remove(node);
    }

    public SkillTree GetCopy()
    {
        return this.Copy() as SkillTree;
    }
}