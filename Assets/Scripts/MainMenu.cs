using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static Equation ActiveEquation;

    [SerializeField] private TextMeshProUGUI _welcome;
    [SerializeField] private GameObject[] _loggedInControls;

    private void Awake()
    {
        UpdateLoggedInControls();
        OnDisplayNameChanged();
        Prefs.OnDisplayNameChanged += OnDisplayNameChanged;
    }

    private void OnDisplayNameChanged()
    {
        if (_welcome) _welcome.text = string.IsNullOrEmpty(Prefs.DisplayName) ? null :  $"Welcome {Prefs.DisplayName}";
    }

    public void UpdateLoggedInControls()
    {
        foreach (GameObject obj in _loggedInControls)
        {
            if (!obj) continue;
            obj.SetActive(!string.IsNullOrEmpty(Prefs.DisplayName));
        }
    }

    public void StartMenu(Equation equation)
    {
        ActiveEquation = equation;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    

    public void Quit()
    {
        Application.Quit();
    }
}