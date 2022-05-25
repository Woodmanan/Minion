using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Player : Monster
{
    [HideInInspector] public Item NewItemInSight = null;

    private static Monster _player;
    public static Monster player
    {
        get
        {
            if (_player == null)
            {
                try
                {
                    _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
                }
                catch
                {
                    Debug.LogWarning("Effect chunk called on player before they could be found.");
                }
            }
            return _player;
        }
        set
        {
            _player = value;
        }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        Setup();
        player = this;
        Player.player.connections.OnTurnStartLocal.AddListener(1000, OnTurnStart);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    //Special case, because it affects the world around it through the player's view.
    public override void UpdateLOS()
    {
        view = LOS.GeneratePlayerLOS(Map.current, location, visionRadius);
        NewItemInSight = null;
        foreach (Item i in view.visibleItems)
        {
            if (i.seen == false)
            {
                i.seen = true;
                LogManager.S.Log("You see a " + i.GetName() + ".");
                NewItemInSight = i;
            }
        }

    }
 
    public override int XPTillNextLevel()
    {
        baseStats.resources.xp = level;
        return level;
    }

    public override void OnLevelUp()
    {
        UIController.singleton.OpenSkillsPanel();
    }

    public override void Die()
    {
        Remove();
        if (resources.health <= 0)
        {
            Debug.Log("Game over!");
            AudioManager.i.GameOver();
            SceneManager.LoadScene("LoseScene", LoadSceneMode.Single);
        }
    }

    public void OnTurnStart()
    {

    }
}
