using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private TextMeshProUGUI _highScore;

    private void OnEnable()
    {
        if (_name) _name.text = Prefs.DisplayName;
        if (_score) _score.text = "Loading...";
        if (_highScore) _highScore.text = "Loading...";

        var request = new GetPlayerStatisticsRequest {
            StatisticNames = new List<string>() {
                Prefs.StatScore,
                Prefs.StatHighScore
            }
        };

        Debug.Log($"GetPlayerStatistics {request.ToJson()}", this);

        PlayFabClientAPI.GetPlayerStatistics(request, response => {
            Debug.Log($"GetPlayerStatistics {response.Statistics}", this);
            
            foreach (StatisticValue stat in response.Statistics)
            {
                Debug.Log($"GetPlayerStatistics {stat}", this);

                if (_score && Prefs.StatScore == stat.StatisticName) {
                    _score.text = stat.Value.ToString();
                }

                if (_highScore && Prefs.StatHighScore == stat.StatisticName) {
                    _highScore.text = stat.Value.ToString();
                }
            }
        }, error => Debug.LogError(error, this));
    }
}
