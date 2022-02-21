﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SkillNode : Node
{

	[Input] public bool canBePurchased;
	[Input] public Skill[] skills;
	[Input] public int skillLevel;
	[Input] public SkillFlag skillFlag;
	[Output] public bool hasBeenPurchased;

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
				if (hasBeenPurchased) return true;
            }
			return false;
		}
		else if (port.fieldName == "skills") return skills;
		else if (port.fieldName == "skillLevel") return skillLevel;
		else if (port.fieldName == "hasBeenPurchased") return skillLevel > 0;
		else return null; // Crisis mode, everything is broken, this should never happen
	}

	public bool IsConnectedTo(NodePort port)
	{
		return IsConnectedTo(port);
	}

	public Skill GetCurrentSkill()
	{
		if (skillLevel > skills.Length) return null;
		else return skills[skillLevel];
	}

	public void Purchase()
    {
		Skill skill = GetCurrentSkill();
		if (skill != null)
		{
			// apply skill here
			skillLevel++;
		}
	}

	[System.Flags]
	public enum SkillFlag
    {
		Tutorial	= (1 << 0),
		Warrior		= (1 << 1),
		Mage		= (1 << 2)
    }
}