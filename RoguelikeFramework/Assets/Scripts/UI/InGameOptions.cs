using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameOptions : MonoBehaviour
{
    public void Ability ()
    {
        UIController.singleton.OpenAbilities();
    }

    public void Inventory ()
    {
        UIController.singleton.OpenInventoryInspect();
    }

    public void Equipment ()
    {
        UIController.singleton.OpenEquipmentInspect();
    }
}
