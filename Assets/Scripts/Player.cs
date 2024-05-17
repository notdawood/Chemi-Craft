using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
  [SerializeField] CinemachineVirtualCameraBase MainCamera;
  [SerializeField] CinemachineVirtualCameraBase LapCamera;

    private bool _gameStated;

    private void OnEnable()
    {
        CameraSwitcher.Register(MainCamera);
        CameraSwitcher.Register(LapCamera);
        CameraSwitcher.SwitchCamera(LapCamera);
    }

    private void OnDisable()
    {
        CameraSwitcher.Unregister(MainCamera);
        CameraSwitcher.Unregister(LapCamera);
    }

    private void Update()
    {
        if (_gameStated) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gameStated = true;
            print("Camera switch requested");
            if(CameraSwitcher.IsActiveCamera(MainCamera))
            {
                print("Switching to LapCamera");
                CameraSwitcher.SwitchCamera(LapCamera);
            }
            else if(CameraSwitcher.IsActiveCamera(LapCamera))
            {
                print("Switching MainCamera");
                CameraSwitcher.SwitchCamera(MainCamera);
            }
        }
        
    }

}
