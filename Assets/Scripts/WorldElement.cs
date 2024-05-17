using TMPro;
using UnityEngine;

public class WorldElement : MonoBehaviour
{
    [SerializeField] private Element _element;
    [SerializeField] private GameObject _display;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private AudioClip _sfx;

    private void Awake()
    {
        if (_text) _text.text = _element.name;
        if (_display) _display.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (!PauseMenu.GameIsPaused)
        {
            print($"OnMouseEnter {name}");
            if (_display) _display.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (!PauseMenu.GameIsPaused)
        {
            print($"OnMouseExit {name}");
            if (_display) _display.SetActive(false);
        }
    }

    private void OnMouseUp()
    {
        if (!PauseMenu.GameIsPaused)
        {
            print($"OnMouseUp {name}");
            UIManager.Instance.ToggleElement(_element);
        }
    }
}
