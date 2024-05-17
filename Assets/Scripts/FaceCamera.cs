using UnityEngine;

public class FaceCamera : MonoBehaviour
{

    private Transform _cam;

    private void Awake() {
        _cam = Camera.main.transform;
    }
    
    private void LateUpdate()
    {
        transform.LookAt(_cam);
    }

}
