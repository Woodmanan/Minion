using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InputRemap : MonoBehaviour
{
    public Text displayTxt;

    public bool hasWarning;
    public InputRemapController irController;
    public string displayText;
    public string buttonName;
    public KeyCode[] keys;
    private HashSet<KeyCode> tempKeys;
    public bool remapping;
    private int buffer;
    
    // Start is called before the first frame update
    void Start()
    {
        remapping = false;
        tempKeys = new HashSet<KeyCode>();
        Refresh();
    }

    // Update is called once per frame
    void Update()
    {
        if (!remapping)
        {
            return;
        }

        // The player will click the button in order to start the remapping process
        // That is also a mouse_up event, that sometimes interferes with the remapping process
        // I'm giving it a 3 frame buffer so I will not have to worry about the mouse_up from button press
        if (buffer > 0)
        {
            buffer -= 1;
            return;
        }
        
        // wait for player's next input
        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                tempKeys.Add(kcode);
            }

            if (Input.GetKeyUp(kcode))
            {
                OnEndRemap();
            }
        }
    }

    public void OnStartRemap()
    {
        Debug.Log("start remapping");
        remapping = true;
        buffer = 3;
        tempKeys.Clear();
        //TODO Show some change on UI
    }

    private void OnEndRemap()
    {
        Debug.Log("end remapping" + tempKeys.ToArray());
        remapping = false;
        keys = tempKeys.ToArray();
        irController.RemapControl(buttonName, keys);
        Refresh();
    }

    private void Refresh()
    {
        displayTxt.text = displayText;
        displayTxt.text += ": ";
        foreach (KeyCode key in keys)
        {
            displayTxt.text += key + " ";
        }
    }

    public void RemoveWarning()
    {
        hasWarning = false;
        displayTxt.color = Color.black;
    }

    public void RaiseWarning()
    {
        hasWarning = true;
        displayTxt.color = Color.red;
    }
}
