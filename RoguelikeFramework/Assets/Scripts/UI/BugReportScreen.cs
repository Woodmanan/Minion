using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class BugReportScreen : RogueUIPanel
{
    //Don't uncomment these! These are already declared in the base class,
    //and are listed here so you know they exist.

    //bool inFocus; - Tells you if this is the window that is currently focused. Not too much otherwise.

    [SerializeField] TMP_InputField description;
    [SerializeField] TMP_InputField contact;

    [SerializeField] GameObject sendingPanel;
    [SerializeField] Button sendButton;
    [SerializeField] TextMeshProUGUI sendingText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * One of the more important functions here. When in focus, this will be called
     * every frame with the stored input from InputTracking
     */
    public override void HandleInput(PlayerAction action, string inputString)
    {
       
    }

    /* Called every time this panel is activated by the controller */
    public override void OnActivation()
    {
        sendingPanel.SetActive(false);
    }
    
    /* Called every time this panel is deactived by the controller */
    public override void OnDeactivation()
    {

    }

    /* Called every time this panel is focused on. Use this to refresh values that might have changed */
    public override void OnFocus()
    {

    }

    /*
     * Called when this panel is no longer focused on (added something to the UI stack). I don't know 
     * what on earth this would ever get used for, but I'm leaving it just in case (Nethack design!)
     */
    public override void OnDefocus()
    {
        
    }

    public void StartSend()
    {
        StartCoroutine(SendReport());
    }

    IEnumerator SendReport()
    {
        sendButton.interactable = false;
        sendingPanel.SetActive(true);
        sendingText.text = "Sending Report...";

        //Begin Upload Process
        WWWForm form = new WWWForm();
        form.AddField("entry.858372980", description.text);
        form.AddField("entry.913728396", LevelLoader.singleton.seed);
        form.AddField("entry.654686957", Map.current.index);
        form.AddField("entry.1859707881", "Log files currently inactive");
        form.AddField("entry.574029582", contact.text);



        byte[] rawData = form.data;

        UnityWebRequest request = UnityWebRequest.Post("https://docs.google.com/forms/u/0/d/e/1FAIpQLSdMSkT0f09ThBCQ7dz3UzEcoxlA7iXYWqtbOthcsQxvw3Y14Q/formResponse", form);
        request.timeout = 5;

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            sendingText.text = "Failed to send report.";
            request.Dispose();
        }
        else
        {
            sendingText.text = "Report sent! Thank you!";
        }

        yield return new WaitForSeconds(1f);
        sendButton.interactable = true;
        ExitAllWindows();
        
    }
}
