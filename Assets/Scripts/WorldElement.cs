using TMPro;
using UnityEngine;

public class WorldElement : MonoBehaviour
{

    public static bool Interactable = true;

    [SerializeField] private Element _element;
    [SerializeField] private GameObject _display;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private AudioClip _sfx;

    private void Awake()
    {
        Interactable = true;
        if (_text) _text.text = _element.name;
        if (_display) _display.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (Interactable && !PauseMenu.GameIsPaused)
        {
            print($"OnMouseEnter {name}");
            if (_display) _display.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (Interactable && !PauseMenu.GameIsPaused)
        {
            print($"OnMouseExit {name}");
            if (_display) _display.SetActive(false);
        }
    }

    private void OnMouseUp()
    {
        if (Interactable && !PauseMenu.GameIsPaused)
        {
            print($"OnMouseUp {name}");
            UIManager.Instance.ToggleElement(_element);
        }
    }
}
