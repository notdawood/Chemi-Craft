using TMPro;
using UnityEngine;

public class UIElement : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;

    public Element Element {
        get => _element;
        set {
            if (value == null) {
                _element = null;
                _text.text = string.Empty;
                return;
            }

            if (!GameManager.Instance.Equation.IsValid(value)) return;
            _element = value;
            _text.text = _element.name;
        }
    }

    private Element _element;

}
