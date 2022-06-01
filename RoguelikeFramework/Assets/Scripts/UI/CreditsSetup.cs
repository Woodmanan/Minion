using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreditsSetup : MonoBehaviour
{
    public List<string> names;
    public GameObject namePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(CreditsSetup), true)]
[CanEditMultipleObjects]
public class Creditor : Editor //I'm so sorry
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        CreditsSetup setup = target as CreditsSetup;
        //string msg = (item.uniqueID != null && item.uniqueID.Length > 0) ? "Re-upload monster (overwrites existing data in sheet)" : "Upload monster to narrative sheet";

        if (GUILayout.Button("Rebuild the credits"))
        {
            for (int i = setup.transform.childCount; i > 0; --i)
            {
                DestroyImmediate(setup.transform.GetChild(0).gameObject);
            }
            foreach (string s in setup.names)
            {
                GameObject newName = Instantiate(setup.namePrefab, setup.transform);
                newName.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = s;
            }
        }

        
    }
}

#endif