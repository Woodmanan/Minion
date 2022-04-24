using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;
using System.IO;


public class InputTracking : MonoBehaviour
{
    [SerializeField] InputSetting inputSetting;
    //Public functions for accessing all of this
    public static Queue<PlayerAction> actions = new Queue<PlayerAction>();
    public static Queue<string> inputs = new Queue<string>();

    public static bool HasNextAction()
    {
        return actions.Count > 0;
    }

    public static PlayerAction PopNextAction()
    {
        if (HasNextAction())
        {
            inputs.Dequeue();
            return actions.Dequeue();
        }
        else
        {
            return PlayerAction.NONE;
        }
    }

    public static Tuple<PlayerAction, string> PopNextPair()
    {
        if (HasNextAction())
        {
            PlayerAction act = actions.Dequeue();
            string inp = inputs.Dequeue();
            return new Tuple<PlayerAction, string>(act, inp);
        }
        else
        {
            return new Tuple<PlayerAction, string>(PlayerAction.NONE, "");
        }
    }

    public static Tuple<PlayerAction, string> PeekNextPair()
    {
        if (HasNextAction())
        {
            PlayerAction act = actions.Peek();
            string inp = inputs.Peek();
            return new Tuple<PlayerAction, string>(act, inp);
        }
        else
        {
            return new Tuple<PlayerAction, string>(PlayerAction.NONE, "");
        }
    }

    public static PlayerAction PeekNextAction()
    {
        if (HasNextAction())
        {
            return actions.Peek();
        }
        else
        {
            return PlayerAction.NONE;
        }
    }

    public static void PushAction(PlayerAction act)
    {
        actions.Enqueue(act);
        inputs.Enqueue(Input.inputString);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Constants.InputPath()))
        {
            string serializedInput = File.ReadAllText(Constants.InputPath());
            JsonUtility.FromJsonOverwrite(serializedInput, inputSetting);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Extensive check that we probably don't want in the built game
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (actions.Count != inputs.Count)
        {
            Debug.LogError("Actions and input queues got misaligned.");
        }
        #endif

        AddMovement();
    }

    public void AddMovement()
    {
        if (UIController.WindowsOpen)
        {
            PushAction(PlayerAction.NONE);
            return;
        }
        
        //Add movements
        if (Left())
        {
            PushAction(PlayerAction.MOVE_LEFT);
        }
        else if (Right())
        {
            PushAction(PlayerAction.MOVE_RIGHT);
        }
        else if (Down())
        {
            PushAction(PlayerAction.MOVE_DOWN);
        }
        else if (Up())
        {
            PushAction(PlayerAction.MOVE_UP);
        }
        else if (UpLeft())
        {
            PushAction(PlayerAction.MOVE_UP_LEFT);
        }
        else if (UpRight())
        {
            PushAction(PlayerAction.MOVE_UP_RIGHT);
        }
        else if (DownLeft())
        {
            PushAction(PlayerAction.MOVE_DOWN_LEFT);
        }
        else if (DownRight())
        {
            PushAction(PlayerAction.MOVE_DOWN_RIGHT);
        }
        else if (Drop())
        {
            PushAction(PlayerAction.DROP_ITEMS);
        }
        else if (PickUp())
        {
            PushAction(PlayerAction.PICK_UP_ITEMS);
        }
        else if (OpenInventory())
        {
            PushAction(PlayerAction.OPEN_INVENTORY);
        }
        else if (Equip())
        {
            PushAction(PlayerAction.EQUIP);
        }
        else if (Unequip())
        {
            PushAction(PlayerAction.UNEQUIP);
        }
        else if (Apply())
        {
            PushAction(PlayerAction.APPLY);
        }
        else if (CastSpell())
        {
            PushAction(PlayerAction.CAST_SPELL);
        }
        else if (Fire())
        {
            PushAction(PlayerAction.FIRE);
        }
        else if (GoUp())
        {
            PushAction(PlayerAction.ASCEND);
        }
        else if (GoDown())
        {
            PushAction(PlayerAction.DESCEND);
        }
        else if (AutoAttack())
        {
            PushAction(PlayerAction.AUTO_ATTACK);
        }
        else if (AutoExplore())
        {
            PushAction(PlayerAction.AUTO_EXPLORE);
        }
        else if (Escaping())
        {
            PushAction(PlayerAction.ESCAPE_SCREEN);
        }
        else if (Accept())
        {
            PushAction(PlayerAction.ACCEPT);
        }
        else if (Wait())
        {
            PushAction(PlayerAction.WAIT);
        }
        else if
            (Input.inputString !=
             "") //FINAL CHECK! Use this to add empty input to the buffer for character checks. (MUST BE LAST CHECK)
        {
            PushAction(PlayerAction.NONE);
        }
    }

    private bool Left()
    {
        KeyCode[] keys = inputSetting.left;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Right()
    {
        KeyCode[] keys = inputSetting.right;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Up()
    {
        KeyCode[] keys = inputSetting.up;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Down()
    {
        KeyCode[] keys = inputSetting.down;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool UpLeft()
    {
        KeyCode[] keys = inputSetting.up_left;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool UpRight()
    {
        KeyCode[] keys = inputSetting.up_right;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool DownLeft()
    {
        KeyCode[] keys = inputSetting.down_left;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool DownRight()
    {
        KeyCode[] keys = inputSetting.down_right;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Drop()
    {
        KeyCode[] keys = inputSetting.drop;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool PickUp()
    {
        KeyCode[] keys = inputSetting.pick_up;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool OpenInventory()
    {
        KeyCode[] keys = inputSetting.open_inventory;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Equip()
    {
        KeyCode[] keys = inputSetting.equip;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Unequip()
    {
        KeyCode[] keys = inputSetting.unequip;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Escaping()
    {
        KeyCode[] keys = inputSetting.escape;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Accept()
    {
        KeyCode[] keys = inputSetting.accept;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    //Currently Q for convention; Apply is the backend stuff, these will probably just be quaffables
    //It's Q, for 'Qiswhatyouhittoapply'
    private bool Apply()
    {
        KeyCode[] keys = inputSetting.apply;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool CastSpell()
    {
        KeyCode[] keys = inputSetting.cast_spell;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Fire()
    {
        KeyCode[] keys = inputSetting.fire;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool Wait()
    {
        KeyCode[] keys = inputSetting.wait;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool GoUp()
    {
        KeyCode[] keys = inputSetting.go_up;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool GoDown()
    {
        KeyCode[] keys = inputSetting.go_down;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool AutoAttack()
    {
        KeyCode[] keys = inputSetting.auto_attack;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

    private bool AutoExplore()
    {
        KeyCode[] keys = inputSetting.auto_explore;
        int n = keys.Length;
        
        Debug.Assert(n != 0);
        
        if (n == 1)
        {
            return Input.GetKeyDown(keys[0]);
        }
        else
        {
            bool pressed = true;
            int i = 0;
            
            while (i < n - 1)
            {
                pressed &= Input.GetKey((keys[i]));
                i++;
            }
            pressed &= Input.GetKeyDown(keys[n - 1]);
            
            return pressed;
        }
    }

}
