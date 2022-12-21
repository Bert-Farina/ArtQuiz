using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    private ScoreKeeper _scoreKeeper;

    // Start is called before the first frame update
    private void Awake()
    {
        _scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void ShowFinalScore()
    {
        finalScoreText.text = "¡Felicidades!\nHas acertado un " + _scoreKeeper.CalculateScore() + "% de las preguntas";
    }
}