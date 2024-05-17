using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIAnswer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _letterText;
    [SerializeField] private TextMeshProUGUI _answerText;
    
    private string[] _letters = {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J"
    };

    private Button _btn;
    private void Awake() {
        if (!TryGetComponent(out _btn)) _btn = gameObject.AddComponent<Button>();
    }

    public void Setup(string text, int index, UnityAction onClick)
    {
        _letterText.text = _letters[index];
        _answerText.text = text;
        _btn.onClick.AddListener(onClick);
    }
}
