using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSplash : MonoBehaviour
{
    public string sceneToLoad;
    private GameData gameData;
    public void WinOK()
    {

        SceneManager.LoadScene("sceneToLoad");
    }

    public void LoseOK()
    {
        SceneManager.LoadScene("sceneToLoad");
    }

    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
