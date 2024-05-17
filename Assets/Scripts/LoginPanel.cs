using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.Events;

public class LoginPanel : MonoBehaviour
{

    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private TextMeshProUGUI _error;
    [SerializeField] private Button _button;

    public UnityEvent<LoginResult> OnLogin;
    private void Awake()
    {
        if (_message) _message.text = null;
        if (_error) _error.text = null;
        if (_button) _button.onClick.AddListener(Login);
    }
    
    public void Login()
    {
        string email = _email.text.Trim();
        string password = _password.text.Trim();

        if (string.IsNullOrEmpty(email)) {
            _email.text = "Email cannot be empty!";
            return;
        } else if (string.IsNullOrEmpty(password)) {
            _password.text = "Password cannot be empty!";
            return;
        }

        if (_message) _message.text = null;
        if (_error) _error.text = null;

        var request = new LoginWithEmailAddressRequest {
            Email = email,
            Password = password,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(request, response => {
            string displayName = response.InfoResultPayload.PlayerProfile.DisplayName;
            if (_message) _message.text = $"Welcome {response.InfoResultPayload.PlayerProfile.DisplayName}";
            Debug.Log($"Login successfull: {response.PlayFabId}", this);
            Prefs.PlayFabId = response.PlayFabId;
            Prefs.DisplayName = displayName;
            OnLogin?.Invoke(response);
        }, OnError);
    }

    private void OnError(PlayFabError error)
    {
        if (_error) _error.text = error.ErrorMessage;
        Debug.LogError(error, this);
    }

}
