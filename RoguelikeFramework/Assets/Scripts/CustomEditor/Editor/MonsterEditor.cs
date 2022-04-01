using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;

[CustomEditor(typeof(Monster), true)]
[CanEditMultipleObjects]
public class MonsterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Monster item = (Monster)target;
        string msg = (item.uniqueID != null && item.uniqueID.Length > 0) ? "Re-upload monster (overwrites existing data in sheet)" : "Upload monster to narrative sheet";

        if (GUILayout.Button(msg))
        {
            UploadItemToSheets(item, serializedObject);
        }

        serializedObject.ApplyModifiedProperties();
    }

    public async void UploadItemToSheets(Monster monster, SerializedObject SO)
    {
        if (monster.uniqueID.Length == 0)
        {
            System.Random rand = new System.Random();
            //Stolen from stack overflow!
            monster.uniqueID = string.Join("", Enumerable.Range(0, 8).Select(n => (char)rand.Next(97, 122)));
            SerializedProperty idSave = SO.FindProperty("uniqueID");
            idSave.stringValue = monster.uniqueID;
        }

        //Begin Upload Process
        WWWForm form = new WWWForm();
        form.AddField("entry.1322731119", monster.name);
        form.AddField("entry.887831505", monster.description);
        form.AddField("entry.558851800", monster.XPFromKill);
        form.AddField("entry.217620525", monster.baseStats.resources.health);
        form.AddField("entry.945554085", monster.baseStats.resources.mana);
        form.AddField("entry.219542321", monster.baseStats.resources.stamina);
        form.AddField("entry.517323373", monster.baseStats.ac);
        form.AddField("entry.539671992", monster.baseStats.ev);
        form.AddField("entry.898218575", monster.minDepth);
        form.AddField("entry.82284167", monster.maxDepth);
        form.AddField("entry.906635114", monster.visionRadius);
        form.AddField("entry.1707958163", monster.energyPerStep);
        form.AddField("entry.268203357", monster.uniqueID);

        UnityWebRequest request = UnityWebRequest.Post("https://docs.google.com/forms/d/e/1FAIpQLScCAgl3CqqXnFUps1g7xlr5QOgW4-XwbDI1T5JZyudrxBY0UA/formResponse", form);
        //request.SetRequestHeader()

        request.SendWebRequest();

        while (!request.isDone)
        {
            await Task.Delay(100);
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("There was an error processing your request! Please troubleshoot yoru connection and try again.");
        }
        else
        {
            Debug.Log("Request finished successfully - Item was uploaded!");
        }
    }
}
