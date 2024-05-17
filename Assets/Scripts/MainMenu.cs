using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static Equation ActiveEquation;

    [SerializeField] private TextMeshProUGUI _welcome;

    private void Awake()
    {
        if (_welcome) _welcome.text = null;
        Prefs.OnDisplayNameChanged += OnDisplayNameChanged;
    }

    private void OnDisplayNameChanged()
    {
        if (_welcome) _welcome.text = $"Welcome {Prefs.DisplayName}";
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