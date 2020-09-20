using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontCamController : MonoBehaviour
{
    public float spdH;
    public float spdV;

    private float yaw;
    private float pitch;

    // Start is called before the first frame update
    private float targetZoom;
    private float smoothValue;
    private float zoomSpd;

    void Start()
    {
        targetZoom = GetComponent<Camera>().fieldOfView;
        smoothValue = 5.0f;
        zoomSpd = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        Rotation();
    }

    private void Rotation()
    {
        yaw += spdH * Input.GetAxis("Mouse X");
        pitch -= spdV * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(pitch, yaw + 180f, 0f);
    }

    private void Zoom()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * smoothValue;
        targetZoom = Mathf.Clamp(targetZoom, 10f, 90f);
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, targetZoom, Time.deltaTime * zoomSpd);
    }
}
