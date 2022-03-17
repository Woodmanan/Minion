using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class FeedbackSubmission : MonoBehaviour
{
    [SerializeField] TMP_InputField Q1;
    [SerializeField] TMP_InputField Q2;
    [SerializeField] TMP_InputField Q3;
    [SerializeField] TMP_InputField Q4;
    [SerializeField] TMP_InputField Q5;
    [SerializeField] TMP_InputField Q6;

    [SerializeField] GameObject sendingPanel;
    [SerializeField] Button sendButton;
    [SerializeField] Button exitButton;
    [SerializeField] TextMeshProUGUI sendingText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSend()
    {
        StartCoroutine(SendReport());
    }

    IEnumerator SendReport()
    {   
        sendButton.interactable = false;
        exitButton.interactable = false;
        sendingPanel.SetActive(true);
        sendingText.text = "Sending Report...";

        //Begin Upload Process
        WWWForm form = new WWWForm();
        form.AddField("entry.1747765906", Q1.text);
        form.AddField("entry.798160135", Q2.text);
        form.AddField("entry.1911834209", Q3.text);
        form.AddField("entry.683732448", Q4.text);
        form.AddField("entry.361536822", Q5.text);
        form.AddField("entry.1619532917", Q6.text);


        byte[] rawData = form.data;

        UnityWebRequest request = UnityWebRequest.Post("https://docs.google.com/forms/u/0/d/e/1FAIpQLScdWdsRlulib1VXs0Q_k5iOhoiUVwyeo8jkb0zMeZUGCSgLOA/formResponse", form);
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
        exitButton.interactable = true;
        ReturnToTitle();
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
