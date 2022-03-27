using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonsterAudio
{
    private Player player;
    
    // Start is called before the first frame update
    public override void Start()
    {
        player = GetComponent<Player>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
