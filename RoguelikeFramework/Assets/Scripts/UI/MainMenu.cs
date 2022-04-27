using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchGame()
    {
        AudioManager.i.Level = 1;
        AudioManager.i.UIStart();
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }

    public void LaunchFeedback()
    {
        SceneManager.LoadScene("FeedbackScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
