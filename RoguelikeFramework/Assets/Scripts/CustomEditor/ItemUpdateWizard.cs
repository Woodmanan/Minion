using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;
using System.Net;
using Unity.Collections;

public struct ItemData
{
    public string name;
    public string pluralName;
    public string description;
    public int minDepth;
    public int maxDepth;
    public ItemRarity rarity;
    
    public override string ToString() => $"ItemData with the following information \n" +
                                         $"name: {name}\n" +
                                         $"plural name: {pluralName})" +
                                         $"description: {description}";
}

public class ItemUpdateWizard
{
    [MenuItem("Tools/Dangerous/Update Item Stats")]
    static void UpdateItems()
    {
        string itemFolderPath = GetPathToFolder("Items");
        if (itemFolderPath == "No File Found!")
        {
            Debug.LogError("No Items folder found");
            return;
        }
        Debug.Log($"Path to folder is {itemFolderPath}");

        List<Item> items = FetchItems(itemFolderPath);
        Debug.Log("Folder items successfully fetched");
        
        string fileName = itemFolderPath + "/info.tsv";
        using (var client = new WebClient())
        {
            UnityEngine.Debug.Log("Downloading information from Google Sheets...");
            client.DownloadFile(
                "https://docs.google.com/spreadsheets/d/1W7yDuY5QEWDmoR9y_gxF0cCUm3T7f_8rGK97Yj4e7E0/export?format=tsv&gid=456697107",
                fileName);
            
            UnityEngine.Debug.Log("Download complete! Updating...");
        }

        Dictionary<string, ItemData> newInfo = ProcessFile(fileName);

        foreach (var item in items)
        {
            string id = item.uniqueID;
            if (newInfo.ContainsKey(id))
            {
                ItemData data = newInfo[id];
                item.name = data.name;
                item.plural = data.pluralName;
                item.description = data.description;
                item.minDepth = data.minDepth;
                item.maxDepth = data.maxDepth;
                item.rarity = data.rarity;
                newInfo.Remove(id);
            }
            else
            {
                Debug.LogError(string.Format("Google sheets does not have the item {0} " +
                                             "with id {1}", item.name, id));
            }
        }

        if (newInfo.Count == 0)
        {
            Debug.Log("Update successfulu!");
        }
        else
        {
            Debug.LogError("There are still " + newInfo.Count + " items that are not used: \n" + newInfo.ToString());
        }
        
        Debug.Log("Cleaning up unused files...");

        //Clean old file
        File.Delete(fileName);
        UnityEngine.Debug.Log("Cleanup Successful!");
        AssetDatabase.Refresh();

        }

    static List<Item> FetchItems(string path)
    {
        List<Item> items = new List<Item>();

        var info = new DirectoryInfo(path);

        foreach (FileInfo f in info.GetFiles("*.prefab"))
        {
            string filePath = f.FullName;
            int length = filePath.Length - info.FullName.Length + path.Length;
            filePath = filePath.Substring(f.FullName.Length - length, length);
            items.Add(AssetDatabase.LoadAssetAtPath<Item>(filePath));
        }

        return items;
    }

    // This is not a generic process file. It processes the specific tsv file downloaded from google sheets
    static Dictionary<string, ItemData> ProcessFile(string path)
    {
        List<string> lines = File.ReadLines(path).ToList();
        
        List<string> headers = lines.First().Split('\t').ToList();
        int nameIndex = headers.IndexOf("Item Name");
        int pluralNameIndex = headers.IndexOf("Plural Name");
        int descriptionIndex = headers.IndexOf("Description");
        int minDepthIndex = headers.IndexOf("Min Depth");
        int maxDepthIndex = headers.IndexOf("Max Depth");
        int rarityIndex = headers.IndexOf("Rarity");
        int keyIndex = headers.IndexOf("Unique Key");
        if (nameIndex == -1 || pluralNameIndex == -1 || descriptionIndex == -1 || minDepthIndex == -1 ||
            maxDepthIndex == -1 || rarityIndex == -1 || keyIndex == -1)
        {
            Debug.LogError("The headers in the data does not match. Please contact David or Woody on discord.");
        }
        
        Dictionary<string, ItemRarity> stringToRarity = new Dictionary<string, ItemRarity>()
        {
            {"COMMON", ItemRarity.COMMON},
            {"UNCOMMON", ItemRarity.UNCOMMON},
            {"RARE", ItemRarity.RARE},
            {"EPIC", ItemRarity.EPIC},
            {"LEGENDARY", ItemRarity.LEGENDARY},
            {"UNIQUE", ItemRarity.UNIQUE}
        };
        
        Dictionary<string, ItemData> newInfo = new Dictionary<string, ItemData>();
        foreach (string line in lines.Skip(1))
        {
            string[] attributes = line.Split('\t');
            ItemData item = new ItemData();
            item.name = attributes[nameIndex];
            item.pluralName = attributes[pluralNameIndex];
            item.description = attributes[descriptionIndex];
            item.minDepth = int.Parse(attributes[minDepthIndex]);
            item.maxDepth = int.Parse(attributes[maxDepthIndex]);
            item.rarity = stringToRarity[attributes[rarityIndex].ToUpper()];
            string key = attributes[keyIndex];
            newInfo.Add(key, item);
        }

        return newInfo;
    }
    
    static string GetPathToFolder(string folder)
    {
        string path = "Assets";
        var info = new DirectoryInfo(path);

        DirectoryInfo[] directories = info.GetDirectories("*", SearchOption.AllDirectories);

        foreach (DirectoryInfo d in directories)
        {
            if (d.Name.Equals(folder))
            {
                string filePath = d.FullName;
                int length = filePath.Length - info.FullName.Length + path.Length;
                filePath = filePath.Substring(d.FullName.Length - length, length);
                return filePath;
            }
        }
        return "No File Found!";
    }
}
