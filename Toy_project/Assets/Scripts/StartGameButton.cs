using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartGameButton : MonoBehaviour
{
    public Button startButton;
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(changeSceneTask);
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void changeSceneTask()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
