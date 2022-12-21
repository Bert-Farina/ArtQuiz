using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class Quiz : MonoBehaviour
{
    [Header("Questions")] [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private List<QuestionSO> questions = new();

    [Header("Answers")] [SerializeField] private GameObject[] answerButtons;

    [Header("Button Colors")] [SerializeField]
    private Sprite defaultAnswerSprite;

    [SerializeField] private Sprite correctAnswerSprite;

    [Header("Timer")] [SerializeField] private Image timerImage;

    [Header("Scoring")] [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Progress Bar")] [SerializeField]
    private Slider progressBar;

    public bool isComplete = false;

    private QuestionSO _currentQuestion;
    private Timer _timer;
    private int _correctAnswerIndex;
    private bool _hasAnsweredEarly;
    private ScoreKeeper _scoreKeeper;

    private void Awake()
    {
        _timer = FindObjectOfType<Timer>();
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    private void LateUpdate()
    {
        timerImage.fillAmount = _timer.fillFraction;
        if (_timer.loadNextQuestion)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator Slider is in Whole Numbers mode, dont apply
            if (progressBar.value == progressBar.maxValue)
            {
                isComplete = true;
                return;
            }

            _hasAnsweredEarly = false;
            GetNextQuestion();
            _timer.loadNextQuestion = false;
        }
        else if (!_hasAnsweredEarly && !_timer.isAnswering)
        {
            DisplayAnswer(-1);
            SwitchButtonState(false);
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = _currentQuestion.GetQuestion();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = _currentQuestion.GetAnswer(i);
        }

        _correctAnswerIndex = _currentQuestion.GetCorrectAnswerIndex();
    }

    private void GetNextQuestion()
    {
        if (questions.Count <= 0) return;
        SwitchButtonState(true);
        SetDefaultButtonSprites();
        GetRandomQuestion();
        DisplayQuestion();
        progressBar.value++;
        _scoreKeeper.IncrementQuestionsSeen();
    }

    private void GetRandomQuestion()
    {
        int index = Random.Range(0, questions.Count);
        _currentQuestion = questions[index];

        if (questions.Contains(_currentQuestion)) questions.Remove(_currentQuestion);
    }

    private void SetDefaultButtonSprites()
    {
        foreach (GameObject t in answerButtons) t.GetComponent<Image>().sprite = defaultAnswerSprite;
    }

    public void OnAnswerSelected(int index)
    {
        _hasAnsweredEarly = true;
        DisplayAnswer(index);
        SwitchButtonState(false);
        _timer.CancelTimer();
        scoreText.text = "Score: " + _scoreKeeper.CalculateScore() + "%";
    }

    private void DisplayAnswer(int index)
    {
        Image buttonImage;

        if (index == _correctAnswerIndex)
        {
            questionText.text = "¡Correcto!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            _scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            questionText.text = "Lo siento, la respuesta correcta era:\n" +
                                _currentQuestion.GetAnswer(_correctAnswerIndex);
            buttonImage = answerButtons[_correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    private void SwitchButtonState(bool state)
    {
        foreach (GameObject t in answerButtons) t.GetComponent<Button>().interactable = state;
    }
}