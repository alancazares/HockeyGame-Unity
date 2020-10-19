using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_HockeyRB : MonoBehaviour
{
    //Movement
    public float moveSpeed = 2;
    private string stickInputX = "Horizontal";
    private string stickInputY = "Vertical";

    private Rigidbody rb;

    //Rotation
    public float rotationRate = 360;
    public float rotationSpeed = 5;
    public Quaternion targetRotation;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //public float rotationRate = 360;
    // Update is called once per frame
    void LateUpdate()
    {
        float moveAxisX = Input.GetAxis(stickInputX);
        float moveAxisY = Input.GetAxis(stickInputY);
        Vector3 controllerInput = new Vector3(moveAxisX, 0, moveAxisY);

        ApplyInput(controllerInput);

    }

    private void ApplyInput(Vector3 stickMovement)
    {
        Move(stickMovement);
        Turn(stickMovement);
    }

    private void Move(Vector3 direction)
    {
        
        //transform.Translate(direction * moveSpeed * Time.deltaTime);
        rb.AddForce(direction * moveSpeed, ForceMode.Force);

    }


    private void Turn(Vector3 direction)
    {
        //don't look at the thing unless you are actually moving the joystick
        if (direction.sqrMagnitude > 0.1f)
        {
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
            

    }
}
