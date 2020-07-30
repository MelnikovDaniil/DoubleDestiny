using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject _target;
    public Animator GamePanelAnimator;
    public Camera camera;
    public float secondsToGoBack;
    private float secondsForMoving;
    private float toCameraSize;
    private float currentTime;
    private const float StandartCameraSize = 5;
    private const float XCameraKoof = 8.7f;
    private const float YCameraKoof = 5f;
    private Vector3 currentPosition;
    private float currentCameraSize;
    private Vector3 cameraShift;
    

    // Update is called once per frame
    void Update()
    {
        if(_target != null)
        {
            
            if (secondsForMoving>currentTime)
            {
                currentTime += Time.deltaTime;
                var k = currentTime / secondsForMoving;
                var cameraSize = Mathf.Lerp(currentCameraSize, toCameraSize, k);
                var cameraPosition = Vector3.Lerp(currentPosition, _target.transform.position + cameraShift, k);
                var cameraCoof = (cameraSize - StandartCameraSize) / -StandartCameraSize;//Mathf.Lerp(StandartCameraSize, toCameraSize, 1 - k) - toCameraSize;
                transform.position = new Vector3(
                    Mathf.Clamp(cameraPosition.x, cameraCoof * -XCameraKoof, cameraCoof * XCameraKoof),
                    Mathf.Clamp(cameraPosition.y, cameraCoof * -YCameraKoof, cameraCoof * YCameraKoof),
                    -10);
                camera.orthographicSize = cameraSize;
            }
            else
            {
                if (_target.name == "basePosition")
                {
                    _target = null;
                    GamePanelAnimator.Play("GamePanelReturn", 0);
                }
                else
                {
                    var cameraCoof = (toCameraSize - StandartCameraSize) / -StandartCameraSize;
                    transform.position = new Vector3(
                        Mathf.Clamp(_target.transform.position.x + cameraShift.x, cameraCoof * -XCameraKoof, cameraCoof * XCameraKoof),
                        Mathf.Clamp(_target.transform.position.y + cameraShift.y, cameraCoof * -YCameraKoof, cameraCoof * YCameraKoof),
                        -10);
                }
            }
        }

    }

    public void SetTarget(GameObject target, float size, float time, Vector3 cameraShift)
    {
        if (_target == null)
        {
            GamePanelAnimator.Play("GamePanel", 0);
        }
        currentPosition = transform.position;
        currentCameraSize = camera.orthographicSize;
        secondsForMoving = time;
        _target = target;
        this.cameraShift = cameraShift;
        toCameraSize = size;
        currentTime = 0;
    }

    public void SetCameraBasePosition(float time = 1)
    {
        currentPosition = transform.position;
        _target = new GameObject("basePosition");
        _target.transform.position = Vector3.zero;
        currentCameraSize = camera.orthographicSize;
        secondsForMoving = time;
        this.cameraShift = Vector3.zero;
        toCameraSize = StandartCameraSize;
        currentTime = 0;
    }
}
