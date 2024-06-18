
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class LeaderboardPanel : MonoBehaviour
{
    
    private const string HighScoreStatisticName = "HighScore";
    private const string ScoreStatisticName = "Score";

    [SerializeField] private RectTransform _content;
    [SerializeField] private LeaderboardPlayerEntry _playerEntry;
    [SerializeField] private GameObject _loading;
    [SerializeField] private Button _refresh;

    private List<PlayerLeaderboardEntry> _scoreLeaderboard;
    private List<PlayerLeaderboardEntry> _highScoreLeaderboard;

    private void Awake()
    {
        if (_refresh) _refresh.onClick.AddListener(OnRefresh);
        if (_loading) _loading.SetActive(false);
        ClearLeaderboard();
    }

    private void OnEnable()
    {
        OnRefresh();
    }

    private void OnRefresh()
    {
        if (_loading) _loading.SetActive(true);
        ClearLeaderboard();
        GetHighScoreLeaderboard();
        GetTotalScoreLeaderboard();
    }

    private void ClearLeaderboard()
    {
        _scoreLeaderboard = null;
        _highScoreLeaderboard = null;

        // Clear previous leaderboard entries
        foreach (Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void GetHighScoreLeaderboard() => GetScoreLeaderboard(HighScoreStatisticName);
    private void GetTotalScoreLeaderboard() => GetScoreLeaderboard(ScoreStatisticName);
    private void GetScoreLeaderboard(string name)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = name,
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, result => {
            if (ScoreStatisticName == name) _scoreLeaderboard = result.Leaderboard;
            else _highScoreLeaderboard = result.Leaderboard;

            Debug.Log($"GetLeaderboard({name}): {result.ToJson()}");
            DisplayLeaderboard();
        }, OnGetLeaderboardFailure);
    }

    private void DisplayLeaderboard()
    {
        if (_scoreLeaderboard == null || _highScoreLeaderboard == null) return;
        if (_loading) _loading.SetActive(false);

        foreach (var item in _scoreLeaderboard)
        {
            LeaderboardPlayerEntry entry = Instantiate(_playerEntry, _content);
            string playerName = item.DisplayName ?? item.PlayFabId;
            
            int highScore = 0;
            foreach (var highScoreItem in _highScoreLeaderboard) {
                if (highScoreItem.PlayFabId != item.PlayFabId) continue;
                highScore = highScoreItem.StatValue;
                break;
            }

            entry.Setup(item.Position, playerName, item.StatValue, highScore);
        }
    }

    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError("Error retrieving leaderboard: " + error.GenerateErrorReport());
    }
}
