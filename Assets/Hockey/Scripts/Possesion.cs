using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possesion : MonoBehaviour
{
    //item is the puck
    public GameObject item;
    public GameObject tempParent;
    public Transform guide;
    

    //shooting the puck
    public Transform shootSpot;
    public float puckSpeed = 5000f;
    private bool inPosession;

    //Calculations
    private float distance;
    private float angle;

    private void Start()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Puck")
        {
            PickPuck();
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyUp("space") && inPosession)
        {
            ShootPuck();
        }
        //Xbox X button
        if (Input.GetKeyUp("joystick button 0") && inPosession)
        {
            ShootPuck();
            //CalculateDistance();
            //CalculateAngle();
        }
        DrawShootingLine();
    }


    void PickPuck()
    {
        item.GetComponent<Rigidbody>().useGravity = false;
        item.GetComponent<Rigidbody>().isKinematic = true;
        item.transform.position = guide.transform.position;
        item.transform.rotation = guide.transform.rotation;
        item.transform.parent = guide.transform.parent;
        inPosession = true;

    }

    void LetGoPuck()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.transform.parent = null;
        item.transform.position = guide.transform.position;
        
    }

    void ShootPuck()
    {
        item.GetComponent<Rigidbody>().useGravity = true;
        item.GetComponent<Rigidbody>().isKinematic = false;
        item.transform.parent = null;
        item.transform.position = guide.transform.position;

        //direction = desiredPosition - currentPosition
        Vector3 shoot = (shootSpot.position - item.transform.position).normalized;
        item.GetComponent<Rigidbody>().AddForce(shoot * puckSpeed);
        inPosession = false;

    }

    void DrawShootingLine()
    {   //line to see distance and direction
        Debug.DrawLine(item.transform.position, shootSpot.position, Color.red);
    }

    void CalculateDistance()
    {
        distance = Vector3.Distance(shootSpot.position, item.transform.position);
        Debug.Log("Distance" + distance);
    }

    void CalculateAngle()
    {
        Vector3 itemForward = item.transform.forward;
        Vector3 distance = shootSpot.transform.position - item.transform.position;

        angle = Vector3.Angle(shootSpot.position, item.transform.position) * Mathf.Rad2Deg;
        Debug.DrawRay(item.transform.position, itemForward *10, Color.blue,10);
        Debug.DrawRay(item.transform.position, distance , Color.blue, 10);
        Debug.Log("Angle" + angle);
    }
}
