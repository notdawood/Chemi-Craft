using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{

    private const int CorrectAnswerScore = 100;

    public static QuestionManager Instance;

    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private UIAnswer answerPrefab;
    [SerializeField] private GameObject container;
    [SerializeField] private GameObject _gameOverContainer;
    [SerializeField] private RectTransform answersContainer;
    [SerializeField] private bool _test;
    private int currentIndex;
    private int _score;
    
    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        container.SetActive(false);
        if (_gameOverContainer) _gameOverContainer.SetActive(false);
    }

    private void Start() {
        if (Application.isEditor && _test) {
            Open();
        }
    }

    public void Open() {
        // Show & unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        _scoreText.text = "";
        container.SetActive(true);
        currentIndex = 0;
        SetQuestion(currentIndex);
    }

    private void SetQuestion(int questionIndex) {

        Equation.Question[] questions = GameManager.Instance.Equation.questions;
        Equation.Question question = questions[questionIndex];
        
        contentText.text = question.content;

        foreach (Transform obj in answersContainer)
        {
            Destroy(obj.gameObject);
        }

        for (int i = 0; i < question.answers.Length; i++)
        {
            string item = question.answers[i];
            int _i = i;
            UIAnswer answer = Instantiate(answerPrefab, answersContainer);
            answer.Setup(item, i, () => {
                if (_i == question.correctAnswer) {
                    // correct
                    _score += CorrectAnswerScore;
                    print($"Question {_i}-'{item}' was answered correctly - current score: {_score}");
                } else {
                    // incorrect
                    print($"Question {_i}-'{item}' was answered INcorrectly - current score: {_score}");
                }

                currentIndex++;
                if (currentIndex >= questions.Length) {
                    // question complete
                    int remainingTime = GameManager.MaxTime - GameManager.GameSeconds;
                    print($"All question complete! taken Time: {remainingTime} questions score: {_score}");
                    _score += remainingTime * 10;
                    _scoreText.text = _score.ToString();
                    GameManager.GameOver = true;
                    if (container) container.SetActive(false);
                    if (_gameOverContainer) _gameOverContainer.SetActive(true);

                    var request = new UpdatePlayerStatisticsRequest {
                        Statistics = new() {
                            new() { 
                                StatisticName = Prefs.StatScore,
                                Value = _score
                            },
                            new() { 
                                StatisticName = Prefs.StatHighScore,
                                Value = _score
                            }
                        }
                    };

                    PlayFabClientAPI.UpdatePlayerStatistics(request, response => {
                        Debug.Log($"UpdatePlayerStatistics succesfull", this);
                    }, error => {
                        Debug.LogError(error, this);
                    });
                } else {
                    SetQuestion(currentIndex);
                }
            });  
        }
    }

    public void BackToMain()
    {
        SceneManager.LoadScene(0);
    }

}
