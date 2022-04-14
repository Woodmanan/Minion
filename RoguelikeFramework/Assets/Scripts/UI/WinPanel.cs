using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(GameController.singleton != null)
            Destroy(GameController.singleton.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToMainMenu()
    {
        AudioManager.i.Level = 0;
        SceneManager.LoadScene(0);
    }

    public void GoToFeedback()
    {
        SceneManager.LoadScene("FeedbackScene", LoadSceneMode.Single);
    }
}

