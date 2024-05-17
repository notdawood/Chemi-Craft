using System;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public static GameManager Instance;
    public static int MaxTime { get => Instance._maxTime; }
    public static int GameSeconds { get; private set; }
    public static bool GameOver;

    public Equation Equation;
    
    public event Action OnGameStarted;
    public event Action OnTimerUpdate;

    [SerializeField, Tooltip("Max time to solve the equation (including questions) in seconds")]
    private int _maxTime = 300; // 5min * 60sec = 300sec

    [SerializeField] private CinemachineVirtualCameraBase _mainCamera;
    [SerializeField] private CinemachineVirtualCameraBase _lapCamera;
    [SerializeField] private CinemachineVirtualCameraBase _lockCamera;

    private bool _gameStated;
    private float _timer;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (MainMenu.ActiveEquation != null) Equation = MainMenu.ActiveEquation;

        GameSeconds = 0;
        GameOver = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        CameraSwitcher.Register(_mainCamera);
        CameraSwitcher.Register(_lapCamera);
        CameraSwitcher.SwitchCamera(_lapCamera);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(_mainCamera);
        CameraSwitcher.Unregister(_lapCamera);
    }

    private void OnDestroy() => GameSeconds = 0;

    private void Update()
    {
        if (_gameStated) {
            if (!GameOver && !PauseMenu.GameIsPaused) {
                _timer += Time.deltaTime;

                if (((int)_timer) > GameSeconds) {
                    GameSeconds = (int)_timer;
                    OnTimerUpdate?.Invoke();
                }
            }

            return;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gameStated = true;
            
            _timer = 0;
            GameSeconds = 0;
            OnGameStarted?.Invoke();

            print("Camera switch requested");
            if(CameraSwitcher.IsActiveCamera(_mainCamera))
            {
                print("Switching to LapCamera");
                CameraSwitcher.SwitchCamera(_lapCamera);
            }
            else if(CameraSwitcher.IsActiveCamera(_lapCamera))
            {
                print("Switching MainCamera");
                CameraSwitcher.SwitchCamera(_mainCamera);
            }
        }
    }

    public void LockCamera() {
        print("Switching to LockCamera");
        CameraSwitcher.SwitchCamera(_lockCamera);
    }

}
