using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeToAnswer = 30f;
    [SerializeField] private float timeToReview = 10f;

    public bool isAnswering = true;
    public float fillFraction;
    public bool loadNextQuestion = true;

    private float _timerValue;

    private void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer()
    {
        _timerValue = 0;
    }

    private void UpdateTimer()
    {
        _timerValue -= Time.deltaTime;

        if (isAnswering)
        {
            if (_timerValue > 0)
            {
                fillFraction = _timerValue / timeToAnswer;
            }
            else
            {
                isAnswering = false;
                _timerValue = timeToReview;
                fillFraction = 1;
            }
        }
        else
        {
            if (_timerValue > 0)
            {
                fillFraction = _timerValue / timeToReview;
            }
            else
            {
                isAnswering = true;
                _timerValue = timeToAnswer;
                fillFraction = 1f;
                loadNextQuestion = true;
            }
        }

        Debug.Log(isAnswering + ": " + _timerValue + " = " + fillFraction);
    }
}