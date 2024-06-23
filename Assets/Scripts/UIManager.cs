using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _startTextContainer;
    [SerializeField] private GameObject _timerTextContainer;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private UIElement _elementPf;
    [SerializeField] private GameObject _plusPf;
    [SerializeField] private GameObject _container;
    [SerializeField] private RectTransform _inputsContainer;
    [SerializeField] private RectTransform _outputsContainer;
    [SerializeField] private AudioClip _winSfx;
    [SerializeField] private AudioClip _correctSfx;
    [SerializeField] private AudioClip _wrongSfx;
    [SerializeField] private AudioClip _deselectSfx;
    [SerializeField] private GameObject _videoContainer;
    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private HeartSystem _heartSystem; // Reference to the HeartSystem script

    private List<UIElement> _inputs = new List<UIElement>();
    private List<UIElement> _outputs = new List<UIElement>();
    private AudioSource _source;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (!TryGetComponent(out _source)) _source = gameObject.AddComponent<AudioSource>();
        if (_startTextContainer) _startTextContainer.SetActive(true);
        if (_timerTextContainer) _timerTextContainer.SetActive(false);
        if (_videoPlayer) _videoPlayer.loopPointReached += _ => OnVideoComplete();
        if (_heartSystem) _heartSystem.gameObject.SetActive(false);
    }

    private void Start()
    {
        GameManager.Instance.OnGameStarted += OnGameStarted;
        GameManager.Instance.OnTimerUpdate += OnTimerUpdate;
        OnTimerUpdate();
        UpdateUI();
    }

    private void Update() {
        if (Application.isEditor && _videoPlayer.isPlaying && Input.GetKeyDown(KeyCode.Space)) {
            _videoPlayer.Stop();
            OnVideoComplete();
        }
    }

    private void OnGameStarted()
    {
        if (_container) _container.SetActive(true);
        if (_startTextContainer) _startTextContainer.SetActive(false);
        if (_timerTextContainer) _timerTextContainer.SetActive(true);
        if (_heartSystem) _heartSystem.gameObject.SetActive(true);
    }

    private void OnTimerUpdate() {
        int minutes = GameManager.GameSeconds / 60;
        int seconds = GameManager.GameSeconds - minutes * 60;

        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (_timerText) _timerText.text = formattedTime;
    }

    private void UpdateUI()
    {
        Equation equation = GameManager.Instance.Equation;

        foreach (Transform child in _inputsContainer) DestroyImmediate(child.gameObject);
        foreach (Transform child in _outputsContainer) DestroyImmediate(child.gameObject);

        int i = 0;
        foreach (Element input in equation.inputs)
        {
            UIElement e = Instantiate(_elementPf, _inputsContainer);
            _inputs.Add(e);

            if (i < equation.inputs.Length - 1)
            {
                Instantiate(_plusPf, _inputsContainer);
            }

            i++;
        }

        foreach (Element output in equation.outputs)
        {
            UIElement e = Instantiate(_elementPf, _outputsContainer);
            e.Element = output;
            _outputs.Add(e);
        }
    }

    public void ToggleElement(Element element)
    {
        foreach (UIElement uiElement in _inputs)
        {
            Element e = uiElement.Element;
            if (e != null && e.name == element.name)
            {
                uiElement.Element = null;
                Play(_deselectSfx);
                return;
            }
        }

        Equation equation = GameManager.Instance.Equation;
        int i = 0;
        foreach (UIElement uiElement in _inputs)
        {
            Element e = uiElement.Element;
            if (e == null)
            {
                if (i == equation.IndexOfInputs(element)) {
                    uiElement.Element = element;
                    Play(_correctSfx);
                    CheckWin();
                    return;
                } else {
                    Play(_wrongSfx);
                    _heartSystem.WrongChoice(); // Deduct a heart for wrong choice
                    return;
                }
            }
            i++;
        }

        Play(_wrongSfx);
        _heartSystem.WrongChoice(); // Deduct a heart for wrong choice
    }

    private void CheckWin()
    {
        Equation equation = GameManager.Instance.Equation;

        for (int i = 0; i < equation.inputs.Length; i++)
        {
            if (_inputs[i].Element && _inputs[i].Element.name == equation.inputs[i].name) continue;
            return;
        }

        for (int i = 0; i < equation.outputs.Length; i++)
        {
            if (_outputs[i].Element && _outputs[i].Element.name == equation.outputs[i].name) continue;
            return;
        }

        PauseMenu.GameIsPaused = true;
        PauseMenu.CanPause = false;
        WorldElement.Interactable = false;
        print("You Won!");
        Play(_winSfx);
        if (_heartSystem) _heartSystem.gameObject.SetActive(false);
        PlayVideo();
        GameManager.Instance.LockCamera();

        if (_container) _container.SetActive(false);
    }

    private void PlayVideo()
    {
        if (_videoPlayer != null)
        {
            _videoContainer.SetActive(true);
            _videoPlayer.clip = GameManager.Instance.Equation.result;
            _videoPlayer.Play();
        }
    }

    private void OnVideoComplete() {
        print("OnVideoComplete");
        _videoContainer.SetActive(false);
        _videoPlayer.gameObject.SetActive(false);
        PauseMenu.GameIsPaused = false;
        PauseMenu.CanPause = true;
        QuestionManager.Instance.Open();
    }

    private void Play(AudioClip clip, float volume = 1)
    {
        if (_source == null || clip == null) return;

        _source.clip = clip;
        _source.volume = volume;
        _source.Play();
    }
}
