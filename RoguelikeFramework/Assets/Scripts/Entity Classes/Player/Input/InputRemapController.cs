using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class InputRemapController : MonoBehaviour
{
    public GameObject contentFrame;
    public InputSetting inputSetting;
    public GameObject keySettingPrefab;
    
    private readonly string[] keyNames = new string[]
        {"left", "right", "up", "down", 
        "upLeft", "upRight", "downLeft", "downRight",
        "drop", "pickUp", "openInventory", "equip", "unEquip",
        "escaping", "accept", "apply", "castSpell", "fire",
        "wait", "goUp", "goDown", "autoAttack", "autoExplore"};
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < keyNames.Length; i++)
        {
            GameObject keySetting = Instantiate(keySettingPrefab, contentFrame.transform);
            keySetting.GetComponent<InputRemap>().buttonName = keyNames[i];
            keySetting.GetComponent<InputRemap>().index = i;
        }
    }
}
