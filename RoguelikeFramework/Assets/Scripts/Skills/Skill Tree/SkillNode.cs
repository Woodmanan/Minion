using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using System.Linq;

public class SkillNode : Node
{
	[Input] public bool canBePurchased;
	[Input] public bool purchaseAllRootsFirst;
	[Input] public Skill[] skills;
	[Input] public SkillFlag skillFlag;
	[Output] public bool hasBeenPurchased;

	private int _skillLevel;
	public int skillLevel {
		get { return _skillLevel; }
		set
		{
			if (value == 0) ((SkillTree)graph).AddToAvailableSkills(this);
			else if (value >= skills.Length) ((SkillTree)graph).RemoveFromAvailableSkills(this);
			_skillLevel = value;
		}
	}

	private bool hasBeenMadeAvailable;

	// Use this for initialization
	protected override void Init()
	{
		base.Init();

	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		if (port.fieldName == "canBePurchased")
		{
			foreach (bool hasBeenPurchased in GetInputValues("canBePurchased", canBePurchased))
			{
				if (purchaseAllRootsFirst && !hasBeenPurchased) return false;
				else if (!purchaseAllRootsFirst && hasBeenPurchased) return true;
			}
			if (purchaseAllRootsFirst) return true;
			return false;
		}
		else if (port.fieldName == "purchaseAllRootsFirst") return purchaseAllRootsFirst;
		else if (port.fieldName == "skills") return skills;
		else if (port.fieldName == "skillLevel") return skillLevel;
		else if (port.fieldName == "skillFlag") return skillFlag;
		else if (port.fieldName == "hasBeenPurchased") return skillLevel > 0;
		else return null; // Crisis mode, everything is broken, this should never happen
	}

	public bool IsConnectedTo(NodePort port)
	{
		return IsConnectedTo(port);
	}

	public List<NodePort> GetAllRoots()
    {
		return GetInputPort("canBePurchased").GetConnections();
	}

	public List<NodePort> GetAllChildren()
    {
		return GetOutputPort("hasBeenPurchased").GetConnections();
    }
	
	public bool IsRoot()
    {
		return GetInputPort("canBePurchased").ConnectionCount == 0;
    }

	public Skill GetCurrentSkill()
	{
		if (skillLevel < 0 || skillLevel >= skills.Length) return null;
		else return skills[skillLevel];
	}

	public void Purchase()
    {
		Skill skill = GetCurrentSkill();
		if (skill != null)
		{
			foreach (StatusEffect effectToApply in skill.effectsToApply) Player.player.AddEffect(effectToApply);
			foreach (Ability ability in skill.abilitiesToApply) Player.player.abilities.AddAbility(ability);

			skillLevel++;

			foreach (NodePort port in GetAllChildren())
            {
				if ((bool)((SkillNode) port.node).GetValue(port)) ((SkillNode) port.node).skillLevel++;

			}
		}
	}

	public void ResetSkillLevel()
    {
		skillLevel = IsRoot() ? 0 : -1;
    }

	[System.Flags]
	public enum SkillFlag
    {
		Tutorial	= (1 << 0),
		Warrior		= (1 << 1),
		Mage		= (1 << 2)
    }
}