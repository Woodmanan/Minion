using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSetting : ScriptableObject
{
    public KeyCode[] left;
    public KeyCode[] right;
    public KeyCode[] up;
    public KeyCode[] down;
    public KeyCode[] up_left;
    public KeyCode[] up_right;
    public KeyCode[] down_left;
    public KeyCode[] down_right;
    public KeyCode[] drop;
    public KeyCode[] pick_up;
    public KeyCode[] open_inventory;
    public KeyCode[] equip;
    public KeyCode[] unequip;
    public KeyCode[] escape;
    public KeyCode[] accept;
    public KeyCode[] apply;
    public KeyCode[] cast_spell;
    public KeyCode[] fire;
    public KeyCode[] wait;
    public KeyCode[] go_up;
    public KeyCode[] go_down;
    public KeyCode[] auto_attack;
    public KeyCode[] auto_explore;
}
