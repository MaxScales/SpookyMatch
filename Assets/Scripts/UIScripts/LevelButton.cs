using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public bool isActive;
    public Sprite activeSprite;
    public Sprite lockedSprite;
    private Image buttonImage;
    private Button myButton;
   

    public Image[] Stars;

    public Text levelText;
    public int level;
    public GameObject confirmPanel;
    private int startsActive;


    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        LoadData();
        ActivateStars();
        ShowLevel();
        
    }

    void LoadData()
    {
        //Is there Game Data
        if(gameData != null)
        {
           

            //determine how many starts to activate
            startsActive = gameData.saveData.stars[level - 1];
        }
    }

    

    void ActivateStars()
    {
        for(int i = 0; i < startsActive; i ++)
        {
            Stars[i].enabled = true;
        }
    }
    void ShowLevel()
    {
        levelText.text = "" + level ;
    }


   

    public void ConfirmPanel(int level)
    {
        confirmPanel.GetComponent<ConfirmPanel>().level = level;
        confirmPanel.SetActive(true);
    }
}
