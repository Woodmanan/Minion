using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using System.Reflection;
using UnityEditor;

public class InputRemapController : MonoBehaviour
{
    [SerializeField] GameObject contentFrame;
    [SerializeField] InputSetting defaultInputSetting;
    [SerializeField] InputSetting inputSetting;
    [SerializeField] GameObject keySettingPrefab;

    private readonly string[] keyNames = new string[]
    {
        "left", "right", "up", "down",
        "up_left", "up_right", "down_left", "down_right",
        "drop", "pick_up", "open_inventory", "equip", "unequip",
        "escape", "accept", "apply", "cast_spell", "fire",
        "wait", "go_up", "go_down", "auto_attack", "auto_explore"
    };

    private Dictionary<string, InputRemap> keyMapping;

    private string FormatName(string keyName)
    {
        string s = char.ToUpper(keyName[0]) + keyName.Substring(1);
        s.Replace('_', ' ');
        return s;
    }

    public void RemapControl(string buttonName, KeyCode[] keys)
    {
        InputRemap currentRemap = keyMapping[buttonName];
        currentRemap.keys = keys;
        inputSetting.GetType().GetField(buttonName).SetValue(inputSetting, keys);
        List<InputRemap> duplicates = new List<InputRemap>();
        
        foreach(KeyValuePair<string, InputRemap> kv in keyMapping)
        {
            if (!buttonName.Equals(kv.Key))
            {
                if (KeyCollision(keys, kv.Value.keys))
                {
                    duplicates.Add(kv.Value);
                }

                if (!hasDuplicate(kv.Key))
                {
                    kv.Value.RemoveWarning();
                }
            }
        }
        
        if (duplicates.Count > 0)
        {
            currentRemap.RaiseWarning();
            foreach (InputRemap ir in duplicates)
            {
                ir.RaiseWarning();
            }
        }
        else
        {
            currentRemap.RemoveWarning();
        }
    }

    public bool hasDuplicate(string targetButton)
    {
        KeyCode[] keys = keyMapping[targetButton].keys;
        foreach(KeyValuePair<string, InputRemap> kv in keyMapping)
        {
            if (!targetButton.Equals(kv.Key))
            {
                if (KeyCollision(keys, kv.Value.keys))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool KeyCollision(KeyCode[] keys1, KeyCode[] keys2)
    {
        if (keys1[keys1.Length - 1] == keys2[keys2.Length - 1])
        {
            // this is a different case
            return true;
        }
        
        if (keys1.Length != keys2.Length)
        {
            return false;
        }

        foreach (KeyCode k in keys1)
        {
            if (!keys2.Contains(k))
            {
                return false;
            }
        }

        return true;
    }
    
    void Start()
    {
        if (File.Exists(Constants.InputPath()))
        {
            string serializedInput = File.ReadAllText(Constants.InputPath());
            JsonUtility.FromJsonOverwrite(serializedInput, inputSetting);
        }

        keyMapping = new Dictionary<string, InputRemap>();
        
        for (int i = 0; i < keyNames.Length; i++)
        {
            string buttonName = keyNames[i];
            KeyCode[] keys = (KeyCode[]) inputSetting.GetType().GetField(buttonName).GetValue(inputSetting);
            GameObject keySetting = Instantiate(keySettingPrefab, contentFrame.transform);
            InputRemap irComponent = keySetting.GetComponent<InputRemap>();
            keyMapping.Add(buttonName, irComponent);
            
            irComponent.buttonName = buttonName;
            irComponent.keys = keys;
            irComponent.displayText = FormatName(buttonName);
            irComponent.irController = this;
        }
    }

    public void SaveInput()
    {
        string serializedInput = JsonUtility.ToJson(inputSetting);
        File.WriteAllText(Constants.InputPath(), serializedInput);
        //SaveScriptableObjectInEditor();
    }

    private void SaveScriptableObjectInEditor()
    {
        string serializedInput = JsonUtility.ToJson(inputSetting);
        JsonUtility.FromJsonOverwrite(serializedInput, inputSetting);
        EditorUtility.SetDirty(inputSetting);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void ResetToDefault()
    {
        inputSetting = defaultInputSetting;
        for (int i = 0; i < keyNames.Length; i++)
        {
            string buttonName = keyNames[i];
            KeyCode[] keys = (KeyCode[]) inputSetting.GetType().GetField(buttonName).GetValue(inputSetting);
            InputRemap irComponent = keyMapping[buttonName];
            
            irComponent.keys = keys;
            
            irComponent.Refresh();
        }
    }
}
