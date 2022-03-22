using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Game Controller"))
        {
            Destroy(GameObject.Find("Game Controller"));
        }

        //UIController.singleton = null;
        LevelLoader.singleton = null;
        Player.player = null;

        RogueUIPanel.panels.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
