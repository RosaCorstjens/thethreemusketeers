using UnityEngine;
using System.Collections;

public class CameraManager 
{
    Camera mainCamera;
    CameraScript camScript;
    public CameraScript CameraScript { get { return camScript; } set { camScript = value; } }

    public void Initialize()
    {
        mainCamera = Camera.main;

        camScript = mainCamera.GetComponent<CameraScript>();
    }

    public void SetTarget(Transform target)
    {
        camScript.target = target;
    }

    public void FocusBack(bool back)
    {
        camScript.FocusBack = back;
    }
}
