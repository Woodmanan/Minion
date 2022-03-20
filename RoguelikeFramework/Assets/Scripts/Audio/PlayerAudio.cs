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
        player.connections.OnHealing.AddListener(1, OnHealing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnHealing(ref int healAmount)
    {
        AudioManager.i.HealthUp(player.baseStats.resources.health);
    }
}
