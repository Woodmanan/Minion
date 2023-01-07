using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEditor;
using System.Net;

public struct MonsterData
{
    public string name;
    public string description;
    public int XPOnKill;
    public int health;
    public int mana;
    public int stamina;
    public int AC;
    public int EV;
    public int minDepth;
    public int maxDepth;
    public int visionRadius;
    public int energyPerStep;
}
public class MonsterUpdateWizard : MonoBehaviour
{
    [MenuItem("Tools/Dangerous/Update Monster Stats")]
    public static void UpdateItems()
    {
        string monsterFolderPath = GetPathToFolder("Monsters");
        if (monsterFolderPath == "No File Found!")
        {
            Debug.LogError("No Monsters folder found");
            return;
        }
        Debug.Log($"Path to folder is {monsterFolderPath}");

        List<Monster> monsters = new List<Monster>();
        monsters = FetchMonsters(monsterFolderPath, monsters);
        
        Debug.Log("Folder items successfully fetched");
        
        string fileName = monsterFolderPath + "/info.tsv";
        using (var client = new WebClient())
        {
            UnityEngine.Debug.Log("Downloading information from Google Sheets...");
            client.DownloadFile(
                "https://docs.google.com/spreadsheets/d/17yVSIvMdFFzqOIB63bBmcU8Ou_7y5Oi5HTdgrS63wMU/export?format=tsv&gid=917248642",
                fileName);
            
            UnityEngine.Debug.Log("Download complete! Updating...");
        }

        Dictionary<string, MonsterData> newInfo = ProcessFile(fileName);

        foreach (var monster in monsters)
        {
            string id = monster.uniqueID;
            if (newInfo.ContainsKey(id))
            {
                MonsterData data = newInfo[id];
                monster.displayName = data.name;
                monster.description = data.description;
                monster.XPFromKill = data.XPOnKill;
                monster.baseStats.resources.health = data.health;
                monster.baseStats.resources.mana = data.mana;
                monster.baseStats.resources.stamina = data.stamina;
                monster.baseStats.ac = data.AC;
                monster.baseStats.ev = data.EV;
                monster.minDepth = data.minDepth;
                monster.maxDepth = data.maxDepth;
                monster.visionRadius = data.visionRadius;
                monster.energyPerStep = data.energyPerStep;
                newInfo.Remove(id);
                EditorUtility.SetDirty(monster);
            }
            else
            {
                Debug.LogWarning(string.Format("Google sheets does not have the monster {0} " +
                                             "with id {1}", monster.name, id));
            }
        }

        if (newInfo.Count == 0)
        {
            Debug.Log("Update successfulu!");
        }
        else
        {
            Debug.LogError("There are still " + newInfo.Count + " monsters that are not used: \n" + newInfo.ToString());
        }
        
        Debug.Log("Cleaning up unused files...");

        //Clean old file
        File.Delete(fileName);
        UnityEngine.Debug.Log("Cleanup Successful!");
        AssetDatabase.Refresh();

        }

    static List<Monster> FetchMonsters(string path, List<Monster> monsters)
    {
        var info = new DirectoryInfo(path);
        foreach (FileInfo f in info.GetFiles("*.prefab"))
        {
            string filePath = f.FullName;
            int length = filePath.Length - info.FullName.Length + path.Length;
            filePath = filePath.Substring(f.FullName.Length - length, length);
            monsters.Add(AssetDatabase.LoadAssetAtPath<Monster>(filePath));
        }
        
        foreach (string subPath in Directory.GetDirectories(path))
        {
            FetchMonsters(subPath, monsters);
        }

        return monsters;
    }

    // This is not a generic process file. It processes the specific tsv file downloaded from google sheets
    static Dictionary<string, MonsterData> ProcessFile(string path)
    {
        List<string> lines = File.ReadLines(path).ToList();
        
        List<string> headers = lines.First().Split('\t').ToList();
        int nameIndex = headers.IndexOf("Name");
        int descriptionIndex = headers.IndexOf("Description");
        int xpIndex = headers.IndexOf("XP On Kill");
        int healthIndex = headers.IndexOf("Health");
        int manaIndex = headers.IndexOf("Mana");
        int staminaIndex = headers.IndexOf("Stamina");
        int ACIndex = headers.IndexOf("AC");
        int EVIndex = headers.IndexOf("EV");
        int minDepthIndex = headers.IndexOf("Min Depth");
        int maxDepthIndex = headers.IndexOf("Max Depth");
        int visionRadiusIndex = headers.IndexOf("Vision Radius");
        int energyPerStepIndex = headers.IndexOf("Energy Per Step");
        int keyIndex = headers.IndexOf("Unique ID");
        if (nameIndex == -1 || descriptionIndex == -1 || xpIndex == -1 || healthIndex == -1 || manaIndex == -1 ||
            staminaIndex == -1 ||  ACIndex == -1 || EVIndex == -1 || minDepthIndex == -1 || maxDepthIndex == -1 ||
            visionRadiusIndex == -1 || energyPerStepIndex == -1 || keyIndex == -1)
        {
            Debug.LogError("The headers in the data does not match. Please contact David or Woody on discord.");
        }
        
        Dictionary<string, MonsterData> newInfo = new Dictionary<string, MonsterData>();
        foreach (string line in lines.Skip(1))
        {
            string[] attributes = line.Split('\t');
            MonsterData monster = new MonsterData();
            string key = attributes[keyIndex];
            if (key.Length == 0)
            {
                Debug.LogWarning($"Monster {attributes[nameIndex]} is in the drive, but not in the project! Please create it, and assign it a key in the drive.");
                continue;
            }

            monster.name = attributes[nameIndex];
            monster.description = attributes[descriptionIndex];
            Debug.Log($"XP header is {xpIndex}, with value of '{attributes[xpIndex]}'");
            monster.XPOnKill = int.Parse(attributes[xpIndex]);
            monster.health = int.Parse(attributes[healthIndex]);
            monster.mana = int.Parse(attributes[manaIndex]);
            monster.stamina = int.Parse(attributes[staminaIndex]);
            monster.AC = int.Parse(attributes[ACIndex]);
            monster.EV = int.Parse(attributes[EVIndex]);
            monster.minDepth = int.Parse(attributes[minDepthIndex]);
            monster.maxDepth = int.Parse(attributes[maxDepthIndex]);
            monster.visionRadius = int.Parse(attributes[visionRadiusIndex]);
            monster.energyPerStep = int.Parse(attributes[energyPerStepIndex]);
            newInfo.Add(key, monster);

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
