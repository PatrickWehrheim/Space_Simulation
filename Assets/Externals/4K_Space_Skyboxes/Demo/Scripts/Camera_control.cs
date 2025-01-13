using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera_control : MonoBehaviour {

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private bool _isRightMousePressed = false;

    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (_isRightMousePressed)
        {

            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        }
    }

    public void OnRightButtonPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _isRightMousePressed = true;
        }
        else if (context.canceled)
        {
            _isRightMousePressed = false;
        }
    }
}
