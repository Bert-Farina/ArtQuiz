using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Quiz _quiz;
    private EndScreen _endScreen;

    private void Awake()
    {
        _quiz = FindObjectOfType<Quiz>();
        _endScreen = FindObjectOfType<EndScreen>();
    }

    private void Start()
    {
        _quiz.gameObject.SetActive(true);
        _endScreen.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_quiz.isComplete) return;
        _quiz.gameObject.SetActive(false);
        _endScreen.gameObject.SetActive(true);
        _endScreen.ShowFinalScore();
    }

    public void onReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}