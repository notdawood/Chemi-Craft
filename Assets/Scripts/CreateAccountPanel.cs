using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountPanel : MonoBehaviour
{
    
    [SerializeField] private TMP_InputField _name;
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TextMeshProUGUI _message;
    [SerializeField] private TextMeshProUGUI _error;
    [SerializeField] private Button _button;

    private void Awake()
    {
        if (_message) _message.text = null;
        if (_error) _error.text = null;
        if (_button) _button.onClick.AddListener(CreateAccount);
    }
    
    public void CreateAccount()
    {
        string name = _name.text.Trim();
        string email = _email.text.Trim();
        string password = _password.text.Trim();

        if (string.IsNullOrEmpty(name)) {
            _name.text = "Name cannot be empty!";
            return;
        } else if (string.IsNullOrEmpty(email)) {
            _email.text = "Email cannot be empty!";
            return;
        } else if (string.IsNullOrEmpty(password)) {
            _password.text = "Password cannot be empty!";
            return;
        }

        if (_message) _message.text = null;
        if (_error) _error.text = null;

        var request = new RegisterPlayFabUserRequest {
            DisplayName = name,
            Email = email,
            Password = password,
            RequireBothUsernameAndEmail = false,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, response => {
            if (_message) _message.text = "Register successfull";
            Debug.Log($"Register successfull: {response.PlayFabId}", this);
        }, OnError);
    }

    private void OnError(PlayFabError error)
    {
        if (_error) _error.text = error.ErrorMessage;
        Debug.LogError(error, this);
    }
}
