using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;
    public int Score;
    private Board board;
    public Image scoreBar;
    private GameData gameData;
    // Start is called before the first frame update
    void Start()
    {
        gameData = FindObjectOfType<GameData>();
        board = FindObjectOfType<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + Score;
    }

    public void IncreaseScore(int amtToIncrease) {
        Score += amtToIncrease;
        if(gameData!= null)
        {
            int highScore = gameData.saveData.highScores[board.currentLevel];
            if (Score > highScore) {
                gameData.saveData.highScores[board.currentLevel] = Score;
            }
        }
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (board != null && scoreBar != null)
        {

            int length = board.scoreGoals.Length;

            scoreBar.fillAmount = (float)Score / (float)board.scoreGoals[length - 1];


        }
    }
}
