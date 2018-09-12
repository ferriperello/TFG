using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    float mainSpeed = 30.0f; //regular speed
    float shiftAdd = 50.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 100.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(1))
        {//Rotation (mouse Left Click)
            float h = 2.0f * Input.GetAxis("Mouse X");
            float v = 2.0f * Input.GetAxis("Mouse Y");
            transform.Rotate(-v, -h, 0);
        }

        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            p = p * totalRun * shiftAdd;
            p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
            p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
            p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;
        Vector3 newPos = transform.position;
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(p);
            newPos.x = transform.position.x;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
        else
        {
            transform.Translate(p);
        }
    }

    private Vector3 GetBaseInput()
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        //Q --> angle cap a esq.

        //E --> angle cap a dre.

        //Z --> Y down
        if (Input.GetKey(KeyCode.Z)){
            p_Velocity += new Vector3(0, -1, 0);
        }
        //X --> Y up
        if (Input.GetKey(KeyCode.X))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        return p_Velocity;
    }
}
