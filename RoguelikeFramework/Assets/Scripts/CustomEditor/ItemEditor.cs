using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;

[CustomEditor(typeof(Item))]
[CanEditMultipleObjects]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Item item = (Item) target;
        string msg = (item.uniqueID != null && item.uniqueID.Length > 0) ? "Re-upload item (overwrites existing data in sheet)" : "Upload item to narrative sheet";

        if (GUILayout.Button(msg))
        {
            UploadItemToSheets(item);
        }
    }

    public async void UploadItemToSheets(Item item)
    {
        if (item.uniqueID.Length == 0)
        {
            System.Random rand = new System.Random();
            //Stolen from stack overflow!
            item.uniqueID = string.Join("", Enumerable.Range(0, 8).Select(n => (char)rand.Next(97, 122)));
        }

        //Begin Upload Process
        WWWForm form = new WWWForm();
        form.AddField("entry.460501323", item.name);
        form.AddField("entry.1467945079", item.plural);
        form.AddField("entry.1860561094", item.description);
        form.AddField("entry.275157245", item.minDepth);
        form.AddField("entry.1930844012", item.maxDepth);
        form.AddField("entry.617925616", item.uniqueID);
        form.AddField("entry.788732120", item.rarity.ToString());

        byte[] rawData = form.data;

        UnityWebRequest request = UnityWebRequest.Post("https://docs.google.com/forms/d/e/1FAIpQLSe5iQhY9NVMT_E1jhQxJw80QGnm4cNB4WubzFYseukgMc-bug/formResponse", form);
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
