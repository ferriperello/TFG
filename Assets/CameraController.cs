using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsImpL;
using UnityEngine.UI;
using System;

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
    public Vector3[] ovs;
    public int nVertex;
    public int nTriangles;

    private int loopNumber = 1;

    public Dictionary<int, List<int>> neighboursinfo = new Dictionary<int, List<int>>();
    public Dictionary<Vector3, List<int>> vertexinfo = new Dictionary<Vector3, List<int>>();

    public GameObject prefab;
    public Text progressText;

    public GameObject ExpansionCanvas;
    public Text newnumloops;
    private bool toggleLoopcanvas = false;

    public Text errorText;

    // Use this for initialization
    void Start()
    {
        newnumloops.text = loopNumber.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = originalPos;
            transform.LookAt(originalCenter);
            loadedObj.GetComponentInChildren<MeshFilter>().mesh.triangles = ots;
        }
        if (Input.GetKey(KeyCode.Y))
        {
            //Start();
            ExpandNTimes();
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
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, 0, 0.5f);
            up_orientation += 0.5f;
        }
        //E --> angle cap a dre.
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 0, -0.5f);
            up_orientation -= 0.5f;
        }
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

    public void WhenLoaded() {
        //boundingBox = loadedObj.GetComponentInChildren<Renderer>().GetComponentInChildren<Renderer>().bounds;
        boundingBox = loadedObj.GetComponentInChildren<Renderer>().bounds;

        originalPos = boundingBox.max;
        originalCenter = boundingBox.center;
        transform.position = originalPos;
        transform.LookAt(originalCenter);

        //amb el objecte ja carregat
        //aixo haurien de ser copies??
        ots = loadedObj.GetComponentInChildren<MeshFilter>().mesh.triangles;
        ovs = loadedObj.GetComponentInChildren<MeshFilter>().mesh.vertices;
        nVertex = ots.Length;
        nTriangles = nVertex / 3;
        findedTriangles = new bool[nTriangles];
        nts = new int[nTriangles * 3];
        Debug.Log(ots[0]);
        Debug.Log(findedTriangles.Length);
        Debug.Log(findedTriangles[0]);

        FindNeightbours();
    }

    private void FindNeightbours() {
        var chrono = System.Diagnostics.Stopwatch.StartNew();
        Debug.Log("entro al Diccionari!");
        Debug.Log("ots length" + ots.Length);
        Debug.Log("ovs length" + ovs.Length);
        for (int i = 0; i < nVertex; i++)
        {
            if (!neighboursinfo.ContainsKey(ots[i]))
            {
                neighboursinfo.Add(ots[i], new List<int> { i / 3 });
            }
            else
            {
                List<int> aux = neighboursinfo[ots[i]];
                //aux[aux.Length + 1] = i / 3;
                aux.Add(i / 3);
                neighboursinfo[ots[i]] = aux;
            }
        }

        for (int p = 0; p < ovs.Length; p++)
        {
            if (!vertexinfo.ContainsKey(ovs[p]))
            {
                vertexinfo.Add(ovs[p], new List<int> { p });
            }
            else
            {
                List<int> aux = vertexinfo[ovs[p]];
                aux.Add(p);
                vertexinfo[ovs[p]] = aux;
            }
        }
        /*
        foreach (KeyValuePair<int, List<int>> kvp in neighboursinfo)
        {
            Debug.Log("Key = " + kvp.Key + ", Values : ");
            foreach (int i in kvp.Value)
            {
                Debug.Log(i);
            }
        }
        foreach (KeyValuePair<Vector3, List<int>> kvp in vertexinfo)
        {
            Debug.Log("Key = " + kvp.Key + ", Values : ");
            foreach (int i in kvp.Value)
            {
                Debug.Log(i);
            }
        }*/
        chrono.Stop();
        Debug.Log("Bucle total, # de iteracions = " + nVertex);
        Debug.Log("Numero de Claus creades : " + neighboursinfo.Keys.Count);
        Debug.Log("Total Time " + chrono.ElapsedMilliseconds);
        Debug.Log("Final de creació del Map");
    }

    public void RayTracing(int loop) {

        Debug.Log("TESTING WITH " + loop + " rays");
        var chrono = System.Diagnostics.Stopwatch.StartNew();
        double total = 0;
        double lasthitround = 1;
        double minrounds = loop * 0.1;
        Debug.Log("minrounds : " + minrounds);
        findedTriangles = new bool[nTriangles];
        nts = new int[nTriangles * 3]; ;

        Debug.Log(CubeManaging.GetTotalVolume());
        //fer el loop
        for (int i = 0; i < loop; i++)
        {
            if (!(lasthitround % minrounds == 0))
            {
                if (i % 1000 == 0) Debug.Log("Iteració : " + i);
                //Vector3 randomXY = Random.rotation.eulerAngles;
                Vector3 randomXY = UnityEngine.Random.insideUnitSphere;
                //Debug.Log(randomXY);
                //Debug.Log(sphere.transform.position);
                //Ray ray = new Ray(sphere.transform.position, new Vector3(randomXY.x, -1, randomXY.z));
                float randvolume = UnityEngine.Random.Range(0.0f, CubeManaging.GetTotalVolume());
                ArrayList volumes = CubeManaging.GetVolumesArray();
                Debug.Log("randV" + randvolume);
                int k = 0;
                float volsdescarted = 0;
                Debug.Log("vol de " + k + ":" + volumes[k]);
                while ((volsdescarted + (float)volumes[k]) < randvolume)
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
                Bounds b = cube.GetComponent<Collider>().bounds;
                //Bounds b = someMesh.bounds;
                Vector3 max = b.max;
                Vector3 min = b.min;
                Debug.Log("MAX" + max);
                Debug.Log("MIN" + min);
                Vector3 point = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
                while (!b.Contains(point))
                {
                    point = new Vector3(UnityEngine.Random.Range(min.x, max.x), UnityEngine.Random.Range(min.y, max.y), UnityEngine.Random.Range(min.z, max.z));
                }
                //Instantiate(prefab, point, Quaternion.identity);*/
                Ray ray = new Ray(point, randomXY);
                //Ray ray = new Ray(transform.position, randomXY);
                //Ray ray = new Ray(transform.position, new Vector3(0, 1, 0));
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
                    total += 1;
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
                    if (findedTriangles[hitInfo.triangleIndex] == false)
                    {
                        findedTriangles[hitInfo.triangleIndex] = true;
                        lasthitround = 1;
                    }
                    else
                    {
                        lasthitround += 1;
                    }
                }
                else
                {
                    lasthitround += 1;
                }
            }
        }
        chrono.Stop();

        Debug.Log("Total Hits " + total);
        Debug.Log("Total Time " + chrono.ElapsedMilliseconds);
        Debug.Log("Rounds without hit " + lasthitround);

        //set new triangles
        int painted = 0;
        int j = 0;
        for (int i = 0; i < nTriangles; i++)
        {
            if (findedTriangles[i])
            {
                painted++;
                nts[j] = ots[i * 3];
                nts[j + 1] = ots[(i * 3) + 1];
                nts[j + 2] = ots[(i * 3) + 2];
                j += 3;
            }
        }
        loadedObj.GetComponentInChildren<MeshFilter>().mesh.triangles = nts;
        Debug.Log("Painted Triangles = " + painted);
        Debug.Log("Descarted Triangles = " + (nTriangles - painted));
        Debug.Log("Of a total of " + nTriangles + " Triangles");

    }


    public void ExpandNTimes()
    {
        try
        {
            for (int i = 0; i < loopNumber; i++)
            {
                Debug.Log("FAIG EXPANSIO");
                DoExpansion();
            }
        }
        catch (NullReferenceException e)
        {
            errorText.text = "CAN'T DO EXPANSION";
            StartCoroutine(wait());
            errorText.text = "";
        }
        
    }
    
    private void DoExpansion()
    {
        //tenint els nous triangles visibles des del punt, fem visibles tots els seus veins (comparteixen un vertex)
        int[] auxnts = (int[])nts.Clone();
        bool[] auxfinded = (bool[])findedTriangles.Clone();
        /*Debug.Log("INICIAL");
        for (int  b= 0; b < auxfinded.Length;  ++b)
        {
            Debug.Log(b+" : "+auxfinded[b]);
        }*/
        for (int i = 0; i < nTriangles; i++)
        {
            if (findedTriangles[i])
            {
                //mirar els vehins dels 3 vertex
                /*Debug.Log("Triangle : " + i);
                Debug.Log("Auxfinded : " + auxfinded[i]);
                Debug.Log("findedTriangles : " + findedTriangles[i]);
                Debug.Log("Miro els vertex : " + i * 3 + ", " + ((i * 3) + 1) + "," + ((i * 3) + 2));*/
                for (int y = 0; y < 3; ++y)
                {
                    
                    int v = ots[(i * 3) + y];
                    //Debug.Log("For de veï numero : " + y + ", que es el triangle:" + ovs[v]);
                    List<int> idemVertex = vertexinfo[ovs[v]];

                    foreach(int x in idemVertex)
                    {
                        //Debug.Log("Miro primer el vertex : " + x);
                        List<int> triangleNeighb = neighboursinfo[x];
                        
                        foreach(int k in triangleNeighb)
                        {
                            //Debug.Log("i veig els veï : " + k);
                            if (!findedTriangles[k])
                            {

                                auxfinded[k] = true;
                            }
                        }
                    }
                }
                /*Debug.Log("Depres d'iterar");
                for (int b = 0; b < auxfinded.Length; ++b)
                {
                    Debug.Log(b + " : " + auxfinded[b]);
                }*/
            }
        }

        int j = 0;
        for (int i = 0; i < nTriangles; i++)
        {
            if (auxfinded[i])
            {
                auxnts[j] = ots[i * 3];
                auxnts[j + 1] = ots[(i * 3) + 1];
                auxnts[j + 2] = ots[(i * 3) + 2];
                j += 3;
            }
        }
        findedTriangles = (bool[])auxfinded.Clone();
        nts = (int[])auxnts.Clone();
        loadedObj.GetComponentInChildren<MeshFilter>().mesh.triangles = (int[])nts.Clone();
    }

    public void ToggleLoopCanvas()
    {
        if (toggleLoopcanvas)
        {
            ExpansionCanvas.SetActive(false);
            toggleLoopcanvas = false;
            newnumloops.text = loopNumber.ToString();
        }
        else
        {
            ExpansionCanvas.SetActive(true);
            toggleLoopcanvas = true;
            newnumloops.text = loopNumber.ToString();
        }

    }

    public void changeLoopNumber()
    {
        loopNumber = int.Parse(newnumloops.text);
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
    }
}