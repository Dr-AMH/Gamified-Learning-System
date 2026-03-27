using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public Question[] questions;

    public TextMeshProUGUI questionText;
    public Text[] answerTexts;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI feedbackText;
    public Slider progressBar;

    private int currentQuestion = 0;
    private int score = 0;

    public DataLogger logger;
    //public int selectedIndex;

    void Start()
    {
        LoadQuestion();
    }

    void LoadQuestion()
    {
        Question q = questions[currentQuestion];

        questionText.text = q.questionText;

        for (int i = 0; i < answerTexts.Length; i++)
        {
            answerTexts[i].text = q.answers[i];
        }

        feedbackText.text = "";
        progressBar.value = currentQuestion;
    }

    public void Answer(int index)
    {
        // ✅ STOP if quiz already finished
        if (currentQuestion >= questions.Length)
            return;

        // ✅ Safety check
        if (logger == null)
        {
            Debug.LogError("Logger NOT assigned!");
            return;
        }

        Question q = questions[currentQuestion];

        // ✅ LOG USER ACTION
        logger.LogEvent("AnswerSelected", index.ToString());

        // ✅ CHECK ANSWER
        if (index == q.correctAnswerIndex)
        {
            score++;
            feedbackText.text = "Correct!";
            feedbackText.color = Color.green;

            logger.LogEvent("Result", "Correct");
        }
        else
        {
            feedbackText.text = "Wrong!";
            feedbackText.color = Color.red;

            logger.LogEvent("Result", "Wrong");
        }

        scoreText.text = "Score: " + score;

        // ✅ VISUAL HIGHLIGHT
        for (int i = 0; i < answerTexts.Length; i++)
        {
            if (i == q.correctAnswerIndex)
                answerTexts[i].color = Color.green;
            else
                answerTexts[i].color = Color.red;
        }

        // ✅ MOVE TO NEXT QUESTION
        currentQuestion++;

        // ✅ CHECK END
        if (currentQuestion < questions.Length)
        {
            Invoke("LoadQuestion", 1.0f);
        }
        else
        {
            questionText.text = "Level Complete!";
            feedbackText.text = "Final Score: " + score;

            logger.SaveToFile();
        }
    }

    public void Restart()
    {
        currentQuestion = 0;
        score = 0;
        scoreText.text = "Score: 0";
        LoadQuestion();
    }
}