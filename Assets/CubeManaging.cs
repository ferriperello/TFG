using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CubeManaging : MonoBehaviour {
    public static ArrayList cubes;
    public static ArrayList volumes;
    public static float totalVolume;

    private bool creating;
    private Vector3 minx;
    private Vector3 maxx;
    private Vector3 height;
    private float triangleHeight;

    public GameObject cube;
    public Camera camera;

    public GameObject loadedObject;
    public int[] ots;
    public int nTriangles;

    Ray ray;
    RaycastHit hit;

    public GameObject CubeCanvas;
    public Text newheight;
    private bool togglecanvas = false;

    // Use this for initialization
    void Start () {
        creating = false;
        minx = new Vector3(0, 0, 0);
        maxx = new Vector3(0, 0, 0);
        cubes = new ArrayList();
        volumes = new ArrayList();
        triangleHeight = 10.0f;
        totalVolume = 0;
        newheight.text = triangleHeight.ToString();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKey(KeyCode.RightShift))
        {
            creating = false;
        }
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) & !creating)
            {
                //Vector3 spawnPosition = camera.ScreenToWorldPoint(Input.mousePosition);
                //Debug.Log(spawnPosition.ToString());
                //GameObject newCube = Instantiate(cube, spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
                Debug.Log("NOT");
                this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    minx = hit.point;
                    //Debug.Log(minx.ToString());
                    creating = true;
                    //GameObject newCube = Instantiate(cube, new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z), Quaternion.identity);
                }
                creating = true;
            }
        }
        if (Input.GetKey(KeyCode.LeftShift) & Input.GetMouseButtonDown(0))
        {
            Debug.Log("CREATING");
            Create();
        }
    }

    private void Create() {
        if (Input.GetMouseButtonDown(0) & creating & maxx.Equals(new Vector3(0, 0, 0)))
        {
            Debug.Log("CREAT");
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                maxx = hit.point;
                creating = false;
                //GameObject newCube = Instantiate(cube, new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z), Quaternion.identity);
            }
            Debug.Log("MINX");
            Debug.Log(minx.ToString());
            Debug.Log("MAXX");
            Debug.Log(maxx.ToString());
            GameObject newCube = Instantiate(cube, new Vector3(0,0,0), Quaternion.identity);
            //newCube.transform.position = new Vector3(minx.x+Mathf.Abs(minx.x-maxx.x), minx.y + Mathf.Abs(minx.y - maxx.y), minx.z + Mathf.Abs(minx.z - maxx.z));
            // newCube.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            
            Vector3 between = maxx - minx;
            float betweenX = maxx.x - minx.x;
            float betweenZ = maxx.z - minx.z;
            //float distance = between.magnitude;
            //newCube.transform.localScale = new Vector3(distance,distance,distance);
            //newCube.transform.localScale = new Vector3(distance, 10f, distance);
            Vector3 newpos = minx + (between / 2.0f);
            newpos.y = Mathf.Min(maxx.y,minx.y);
            Debug.Log("Maxx y = " + maxx.y);
            Debug.Log("Minx y = " + minx.y);
            Debug.Log("Min y = " + newpos.y);
            Debug.Log("THeight y / 2 = " + triangleHeight / 2);
            
           
            
            //newCube.transform.LookAt(maxx);
            newCube.transform.localScale = new Vector3(Mathf.Abs(betweenX), triangleHeight, Mathf.Abs(betweenZ));
            newpos.y += (triangleHeight/2);
            newCube.transform.position = newpos;
            Debug.Log("Min y = " + newpos.y);

            float cubevolume = newCube.transform.localScale.x * newCube.transform.localScale.y * newCube.transform.localScale.z;
            cubes.Add(newCube);
            volumes.Add(cubevolume);
            totalVolume += cubevolume;

            //Debug.Log(newCube.transform.localScale.x * newCube.transform.localScale.y * newCube.transform.localScale.z);

        }
        maxx = new Vector3(0, 0, 0);
    }

    public static float GetTotalVolume()
    {
        return totalVolume;
    }

    public static ArrayList GetVolumesArray()
    {
        return volumes;
    }

    public static ArrayList GetCubesArray()
    {
        return cubes;
    }
    public static GameObject GetCubeinArray(int i)
    {
        return (GameObject)cubes[i];
    }

    public void ToggleCanvas()
    {
        if (togglecanvas)
        {
            CubeCanvas.SetActive(false);
            togglecanvas = false;
            newheight.text = triangleHeight.ToString();
        }
        else
        {
            CubeCanvas.SetActive(true);
            togglecanvas = true;
            newheight.text = triangleHeight.ToString();
        }

    }

    public void ChangeHeigth()
    {
        triangleHeight = float.Parse(newheight.text);
    }
}
