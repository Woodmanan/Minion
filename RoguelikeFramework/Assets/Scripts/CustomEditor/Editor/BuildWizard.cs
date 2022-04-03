using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Diagnostics;
using System.Linq;
using System.IO.Compression;
using System.IO;

public class BuildWizard
{
    /* 
     * If set to granular, release candidates will pop out with MM-dd-HH-mm time, rather than MM-dd
     * 
     * Set this to true if you make lots and lots of last minute changes, and think you'll
     * probably fuck up a build right before release - you'll have more LKG's to go back to!
     */
    private const bool useGranularTime = false;

    [MenuItem("Build/Standard/Launch Windows Build")]
    public static void MakeWindows()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Builds Folder", "", "");
        string folderName = $"{PlayerSettings.productName} - Windows - Testing";
        string exeName = $"{PlayerSettings.productName}.exe";

        if (path.Length == 0)
        {
            UnityEngine.Debug.Log("Aborting!");
            return;
        }

        string targetPath = $"{path}/{folderName}/{exeName}";

        MakeBuild(targetPath, BuildTarget.StandaloneWindows);
    }

    [MenuItem("Build/Release/Launch Windows Build - Release Candidate")]
    public static void MakeWindowsZipped()
    {
        string date = System.DateTime.Now.ToString(useGranularTime ? "MM-dd-HH-mm" : "MM-dd");

        string path = EditorUtility.SaveFolderPanel("Choose Builds Folder", "", "");
        string folderName = $"{PlayerSettings.productName} - Windows - {date}";
        string exeName = $"{PlayerSettings.productName}.exe";

        if (path.Length == 0)
        {
            UnityEngine.Debug.Log("Aborting!");
            return;
        }

        string targetPath = $"{path}/{folderName}/{exeName}";

        MakeBuild(targetPath, BuildTarget.StandaloneWindows);

        { //Zip up the resulting build
            string zipSource = $"{path}/{folderName}";
            string zipTarget = $"{path}/Zipped Releases/{PlayerSettings.productName}-Windows-{date}.zip";

            if (!System.IO.Directory.Exists($"{path}/Zipped Releases"))
            {
                System.IO.Directory.CreateDirectory($"{path}/Zipped Releases");
            }

            if (File.Exists(zipTarget))
            {
                UnityEngine.Debug.Log("Clearing old build target!");
                File.Delete(zipTarget);
            }

            ZipFile.CreateFromDirectory(zipSource, zipTarget);
        }
        
    }

    [MenuItem("Build/Standard/Launch Mac Build")]
    public static void MakeMac()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Builds Folder", "", "");
        string folderName = $"{PlayerSettings.productName} - Mac";
        string exeName = $"{PlayerSettings.productName}.app";

        if (path.Length == 0)
        {
            UnityEngine.Debug.Log("Aborting!");
            return;
        }

        string targetPath = $"{path}/{folderName}/{exeName}";

        MakeBuild(path, BuildTarget.StandaloneOSX);
    }

    public static void MakeBuild(string path, BuildTarget target)
    {
        Stopwatch watch = new Stopwatch();
        watch.Start();

        { //Pull fresh item data and sort
            ItemUpdateWizard.UpdateItems();
            MonsterUpdateWizard.UpdateItems();

            ItemSortWizard.SortItems();
            MonsterSortWizard.SortItems();
        }
        

        { //Launch the actual build
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray();
            buildPlayerOptions.locationPathName = path;
            buildPlayerOptions.target = target;
            buildPlayerOptions.options = BuildOptions.None;

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            watch.Stop();
            BuildSummary summary = report.summary;
            UnityEngine.Debug.Log($"{target} build finished with result of {summary.result}: " + summary.totalSize / 1000000 + " mb built in " + watch.ElapsedMilliseconds / 1000 + " seconds");
        }
    }
}
