using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    public Transform followTarget;
    public float sensitivity;

    private float targetZoom;
    private float smoothValue;

    void Start()
    {
        smoothValue = 5.0f;
    }
   
    void Update()
    {
        CameraRotation();
        Zoom();
    }

    private void CameraRotation()
    {
        followTarget.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensitivity, Vector3.up);
        followTarget.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * sensitivity, Vector3.right);

        var angles = followTarget.localEulerAngles;
        angles.z = 0;

        var angle = followTarget.localEulerAngles.x;

        if(angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        followTarget.transform.localEulerAngles = angles;
        //transform.rotation = Quaternion.Euler(0, followTarget.rotation.eulerAngles.y, 0);
        //followTarget.localEulerAngles = new Vector3(angles.x, 0, 0);
    }


    private void Zoom()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * smoothValue;
        targetZoom = Mathf.Clamp(targetZoom, 10f, 90f);
  
    }



}
