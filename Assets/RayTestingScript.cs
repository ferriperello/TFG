using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayTestingScript : MonoBehaviour {

    float mainSpeed = 30.0f; //regular speed
    float shiftAdd = 50.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 100.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    private float up_orientation = 0.0f;
    public GameObject sphere;
    public GameObject cube;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        
        if (Input.GetKey(KeyCode.T))
        {
            int loop = 100000000;
            Debug.Log("TESTING WITH " + loop + " rays");
            var chrono = System.Diagnostics.Stopwatch.StartNew();
            double total = 0;
            //fer el loop
            for (int i = 0; i < loop; i++)
            {
                //Vector3 randomXY = Random.rotation.eulerAngles;
                Vector3 randomXY = Random.insideUnitSphere;
                //Debug.Log(randomXY);
                //Debug.Log(sphere.transform.position);
                //Ray ray = new Ray(sphere.transform.position, new Vector3(randomXY.x, -1, randomXY.z));
                Ray ray = new Ray(sphere.transform.position, randomXY);
                //Debug.DrawRay(new Vector3(0,0,0),randomXY*100,Color.green,500);
                RaycastHit hitInfo;
                //Physics.Raycast(ray, out hitInfo);
                if (Physics.Raycast(ray, out hitInfo))
                {
                    total = total + 1;
                    if (total % 10000000 == 0)
                    {
                        GameObject newCube = Instantiate(cube, new Vector3(0, 0, 0), Quaternion.identity);
                        newCube.transform.localScale = new Vector3(10f, 10f, 10f);
                        newCube.transform.position = hitInfo.point;
                    }
                }
                /*//Part del codi per a retornar el triangle amb el que ha colisionat
                if (!Physics.Raycast(ray, out hitInfo))
                    return;

                MeshCollider meshCollider = hitInfo.collider as MeshCollider;
                if (meshCollider == null || meshCollider.sharedMesh == null)
                    return;

                Mesh mesh = meshCollider.sharedMesh;
                Vector3[] vertices = mesh.vertices;
                int[] triangles = mesh.triangles;
                Vector3 p0 = vertices[triangles[hitInfo.triangleIndex * 3 + 0]];
                Vector3 p1 = vertices[triangles[hitInfo.triangleIndex * 3 + 1]];
                Vector3 p2 = vertices[triangles[hitInfo.triangleIndex * 3 + 2]];
                Transform hitTransform = hitInfo.collider.transform;
                p0 = hitTransform.TransformPoint(p0);
                p1 = hitTransform.TransformPoint(p1);
                p2 = hitTransform.TransformPoint(p2);
                Debug.DrawLine(p0, p1, Color.blue, 500);
                Debug.DrawLine(p1, p2, Color.blue, 500);
                Debug.DrawLine(p2, p0, Color.blue, 500);
                Debug.Log(hitInfo.triangleIndex);
                */
            }
            chrono.Stop();
            Debug.Log("Total Hits " + total);
            Debug.Log("Total Time " + chrono.ElapsedMilliseconds);

        }

        if (Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * 2.5f, -Input.GetAxis("Mouse X") * 2.5f, 0));
            float X = transform.rotation.eulerAngles.x;
            float Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, up_orientation);
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
        transform.Translate(p);

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

        //Z --> Y down
        if (Input.GetKey(KeyCode.Z))
        {
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
