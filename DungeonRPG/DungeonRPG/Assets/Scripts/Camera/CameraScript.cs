using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public Transform target;

    public float walkDistance, runDistance;
    public float height;
    public float xSpeed, ySpeed;
    public float heightDamping = 2f;
    public float rotationDamping = 3f;

    private float rotationAngle = 0;
    public bool FocusBack { get { return rotationAngle == 0 ? true : false; } set { rotationAngle = value == true ? 0 : 180; } }

    public bool CanReceiveInput { get; set; }

    private Transform myTransform;
    private float x, y;
    private Vector2 currentMousePos, lastMousePos, deltaMousePos;
    private bool camButtonDown = false;

    void Start()
    {
        myTransform = transform;
        if (target == null) CameraSetUp();
    }

    void Update()
    {
        if (!CanReceiveInput)
        {
            camButtonDown = false;
            return;
        }

        if (Input.GetMouseButtonDown(1)) camButtonDown = true;
        if (Input.GetMouseButtonUp(1)) camButtonDown = false;
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (camButtonDown)
        {
            y += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            x += Input.GetAxis("Mouse Y") * ySpeed * 0.002f;

            x = ClampAngle(x, 25, 90);

            Quaternion rotation = Quaternion.Euler(x, y, 0);
            Vector3 position = rotation * new Vector3(0f, 0f, -walkDistance) + target.position;

            myTransform.rotation = rotation;
            myTransform.position = position;
        }
        else
        {
            // Calculate the current rotation angles
            float wantedRotationAngle = target.eulerAngles.y - rotationAngle;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = myTransform.eulerAngles.y;
            float currentHeight = myTransform.position.y;

            // Damp the rotation around the y-axis
            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

            // Damp the height
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            // Convert the angle into a rotation
            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            // Set the position of the camera on the x-z plane to:
            // distance meters behind the target
            myTransform.position = target.position;
            myTransform.position -= currentRotation * Vector3.forward * walkDistance;

            // Set the height of the camera
            myTransform.position = new Vector3(myTransform.position.x, currentHeight, myTransform.position.z);

            // Always look at the target
            myTransform.LookAt(target);

            y = myTransform.rotation.eulerAngles.y;
        }
    }

    public void CameraSetUp()
    {
        myTransform.position = new Vector3(target.position.x, target.position.y + height, target.position.z - walkDistance);

        myTransform.LookAt(target);

        x = myTransform.position.x;
        y = myTransform.position.y;
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;

        return Mathf.Clamp(angle, min, max);
    }

    private void GetDeltaMousePosition()
    {
        // Store the old value. 
        lastMousePos = currentMousePos;

        // Get the new value.
        currentMousePos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Subtract. 
        deltaMousePos = currentMousePos - lastMousePos;
    }
}
