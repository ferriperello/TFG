﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {

    float mainSpeed = 30.0f; //regular speed
    float shiftAdd = 50.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 100.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    private float up_orientation = 0.0f;
    private Bounds boundingBox;
    public GameObject loadedObj;
    private Vector3 originalPos;
    private Vector3 originalCenter;

    public bool[] findedTriangles;
    public int[] nts;
    public int[] ots;
    public int nTriangles;

    public GameObject prefab;

    // Use this for initialization
    void Start()
    {
        boundingBox = loadedObj.GetComponentInChildren<Renderer>().bounds;

        originalPos = boundingBox.max;
        originalCenter = boundingBox.center;
        transform.position = originalPos;
        transform.LookAt(originalCenter);

        //amb el objecte ja carregat
        ots = loadedObj.GetComponentInChildren<MeshFilter>().mesh.triangles;
        nTriangles = ots.Length/3;
        findedTriangles = new bool[nTriangles];
        nts = new int[nTriangles * 3];
        Debug.Log(findedTriangles.Length);
        Debug.Log(findedTriangles[0]);
        //loadedObj.GetComponentInChildren<MeshFilter>().mesh.triangles =  nts;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            int loop = 10000000;
            Debug.Log("TESTING WITH " + loop + " rays");
            var chrono = System.Diagnostics.Stopwatch.StartNew();
            double total = 0;

            Debug.Log(CubeManaging.GetTotalVolume());
            //fer el loop
            for (int i = 0; i < loop; i++)
            {
                if (i % 10000 == 0) Debug.Log("Iteració : "+i);
                //Vector3 randomXY = Random.rotation.eulerAngles;
                Vector3 randomXY = Random.insideUnitSphere;
                //Debug.Log(randomXY);
                //Debug.Log(sphere.transform.position);
                //Ray ray = new Ray(sphere.transform.position, new Vector3(randomXY.x, -1, randomXY.z));
               /* float randvolume = Random.Range(0.0f, CubeManaging.GetTotalVolume());
                ArrayList volumes = CubeManaging.GetVolumesArray();
                Debug.Log("randV"+randvolume);
                int k = 0;
                float volsdescarted = 0;
                Debug.Log("vol de " + k + ":" + volumes[k]);
                while ((volsdescarted +(float)volumes[k]) < randvolume)
                {
                    Debug.Log("entro");
                    volsdescarted += (float)volumes[k];
                    k++;
                    Debug.Log("vol de " + k + ":" + volumes[k]);
                    Debug.Log("totalVuntilnow" + volsdescarted);

                }
                Debug.Log(volumes);
                Debug.Log(k);
                GameObject cube = CubeManaging.GetCubeinArray(k);
                Mesh someMesh = cube.GetComponent<MeshFilter>().mesh;
                Bounds b = someMesh.bounds;
                Vector3 max = b.max;
                Vector3 min = b.max;
                Debug.Log("MAX"+max);
                Debug.Log("MIN"+min);
                Vector3 point = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
                while (!someMesh.bounds.Contains(point))
                {
                    point = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
                }
                //Instantiate(prefab, point, Quaternion.identity);*/
                Ray ray = new Ray(transform.position, randomXY);
                //Debug.DrawRay(new Vector3(0,0,0),randomXY*100,Color.green,500);
                RaycastHit hitInfo;
                //Physics.Raycast(ray, out hitInfo);
                /*if (Physics.Raycast(ray, out hitInfo))
                {
                 total = total + 1;
                }*/
                //Part del codi per a retornar el triangle amb el que ha colisionat
                if (Physics.Raycast(ray, out hitInfo))
                {
                    total = total + 1;
                    //Physics.Raycast(ray, out hitInfo);
                    //Debug.Log(hitInfo.triangleIndex);
                    //MeshCollider meshCollider = hitInfo.collider as MeshCollider;
                    /* (meshCollider == null || meshCollider.sharedMesh == null)
                        break;*/

                    /* Mesh mesh = meshCollider.sharedMesh;
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
                     Debug.DrawLine(p2, p0, Color.blue, 500);*/
                    findedTriangles[hitInfo.triangleIndex] = true;
                }
            }
            chrono.Stop();

            Debug.Log("Total Hits " + total);
            Debug.Log("Total Time " + chrono.ElapsedMilliseconds);

            //set new triangles
            int j = 0;
            for (int i = 0; i < nTriangles; i++) {
                if (findedTriangles[i])
                {
                    nts[j] = ots[i * 3];
                    nts[j+1] = ots[(i * 3)+1];
                    nts[j+2] = ots[(i * 3)+2];
                    j+= 3;
                }
            }
            loadedObj.GetComponentInChildren<MeshFilter>().mesh.triangles = nts;

        }

        if (Input.GetKey(KeyCode.R))
        {
            transform.position = originalPos;
            transform.LookAt(originalCenter);
            loadedObj.GetComponentInChildren<MeshFilter>().mesh.triangles = ots;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, 0, 0.5f);
            up_orientation += 0.5f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 0, -0.5f);
            up_orientation -= 0.5f;
        }

        if (Input.GetMouseButton(1))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * 2.5f, -Input.GetAxis("Mouse X") * 2.5f, 0));
            float X = transform.rotation.eulerAngles.x;
            float Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, up_orientation);
        }

        if (Input.GetKey(KeyCode.Space)) {
            transform.Rotate(0, 0, -(up_orientation));
            up_orientation = 0.0f;
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
