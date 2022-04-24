using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System;


public class InputTracking : MonoBehaviour
{
    public List<InputSetting> allInputSettings;
    private InputSetting inputSetting;
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
        inputSetting = allInputSettings[0];
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

    //WASD has been removed here
    private bool Left()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.left)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool Right()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.right)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool Up()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.up)
        {
            pressed = pressed & Input.GetKeyDown(key);
        }

        return pressed;
    }

    private bool Down()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.down)
        {
            pressed = pressed & Input.GetKeyDown(key);
        }

        return pressed;
    }

    private bool UpLeft()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.up_left)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool UpRight()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.up_right)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool DownLeft()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.down_left)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool DownRight()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.down_right)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool Drop()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.drop)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool PickUp()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.pick_up)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool OpenInventory()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.open_inventory)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool Equip()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.equip)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    //TODO: Revisit this sequence of inputs. As of right now PlayerAction.MOVE_UP_RIGHT is the one keyed to this.
    private bool Unequip()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.unequip)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool Escaping()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.escape)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool Accept()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.accept)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    //Currently Q for convention; Apply is the backend stuff, these will probably just be quaffables
    //It's Q, for 'Qiswhatyouhittoapply'
    private bool Apply()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.apply)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool CastSpell()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.cast_spell)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool Fire()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.fire)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool Wait()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.wait)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool GoUp()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.go_up)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool GoDown()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.go_down)
        {
            pressed = pressed & Input.GetKeyDown((key));
        }

        return pressed;
    }

    private bool AutoAttack()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.auto_attack)
        {
            pressed = pressed & Input.GetKeyDown(key);
        }

        return pressed;
    }

    private bool AutoExplore()
    {
        bool pressed = true;
        foreach (KeyCode key in inputSetting.auto_explore)
        {
            pressed = pressed & Input.GetKeyDown(key);
        }

        return pressed;
    }

}
