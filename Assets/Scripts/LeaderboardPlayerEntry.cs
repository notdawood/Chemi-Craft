using TMPro;
using UnityEngine;

public class LeaderboardPlayerEntry : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _rank;
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _highScore;
    [SerializeField] private TextMeshProUGUI _score;

    public void Setup(int rank, string name, int highScore, int score)
    {
        if (_rank) _rank.text = (rank+1).ToString();
        if (_name) _name.text = name;
        if (_highScore) _highScore.text = highScore.ToString();
        if (_score) _score.text = score.ToString();
    }

}
