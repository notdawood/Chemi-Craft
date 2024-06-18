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
        
        PlayFabClientAPI.GetPlayerStatistics(request, response => {
            if (_score) _score.text = "None !";
            if (_highScore) _highScore.text = "None !";
            
            foreach (StatisticValue stat in response.Statistics)
            {
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
