using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public Text questionText;
    public Text scoreText;
    public BasicObjectPool answerButtonPool;
    public Transform AnswerButtonParent;
    public int playerScore;
    public GameObject questionDisplay;
    public GameObject endGameDisplay;
    public Text timeText;

    private DataController dataController;
    private RoundData roundData;
    private QuestionData[] questionPool;
    private bool isRoundActive;
    private int questionIndex;
    private float timeRemaining;

    private List<GameObject> answerButtonObjects = new List<GameObject>();
    
	// Use this for initialization
	void Start () {
        dataController = FindObjectOfType<DataController>();
        roundData = dataController.getCurrentRoundData();
        questionPool = roundData.questions;
        timeRemaining = roundData.timeLimitInSeconds;
        ShowQuestions();
        playerScore = 0;
        UpdateTime();
        isRoundActive = true;
	}

    private void ShowQuestions()
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionText.text = questionData.questionText;

        for(int i = 0; i<questionData.answers.Length; i++)
        {
            GameObject answerButtonObject = answerButtonPool.GetObj();
            answerButtonObject.transform.SetParent(AnswerButtonParent);
            answerButtonObjects.Add(answerButtonObject);

            AnswerButton answerButton = answerButtonObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]);
        }
    }

    private void RemoveAnswerButtons()
    {
        while (answerButtonObjects.Count > 0)
        {
            answerButtonPool.ReturnObj(answerButtonObjects[0]);
            answerButtonObjects.RemoveAt(0);
        }
    }

    public void AnswerClicked(bool isCorrect)
    {
        if (isCorrect)
        {
            //Add player score
            playerScore += roundData.addedPointsForCorrectAnswer;
            scoreText.text = "Score: " + playerScore.ToString();
        }

        if (questionPool.Length > questionIndex + 1)
        {
            questionIndex++;
            ShowQuestions();
        }
        else
        {
            //End the game.
            EndRound();
        }
           
    }

    public void EndRound()
    {
        isRoundActive = false;
        questionDisplay.SetActive(false);
        endGameDisplay.SetActive(true);
    }

    public void Reset()
    {
        SceneManager.LoadScene("MenuScreen");
    }

    private void UpdateTime()
    {
        timeText.text = "Time: " + Mathf.Round(timeRemaining);
    }
	
	// Update is called once per frame
	void Update () {
        if (isRoundActive)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTime();
            if (timeRemaining <= 0)
                EndRound();
        }
           
	}
}
