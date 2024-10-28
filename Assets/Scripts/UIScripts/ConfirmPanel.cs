using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour
{

    public string levelToLoad;
    public Image[] Stars;
    public int level;
    public  Text highScoreText;

    private int highScore;
    private int starsActive;
    private GameData gameData;
    // Start is called before the first frame update
    void OnEnable()
    {
        gameData = FindObjectOfType<GameData>();
        LoadData();
        ActivateStars();
        setText();
    }

    void LoadData()
    {
        if(gameData != null)
        {
            starsActive = gameData.saveData.stars[level - 1];
            highScore = gameData.saveData.highScores[level - 1];
        }
    }

    void setText()
    {
        highScoreText.text = "" + highScore; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateStars()
    {
        for (int i = 0; i < starsActive; i++)
        {
            Stars[i].enabled = true;
        }
    }

    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }

    public void Play()
    {
        PlayerPrefs.SetInt("Current Level", level);
        SceneManager.LoadScene(levelToLoad);
    }
}
