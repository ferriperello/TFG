using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManaging : MonoBehaviour {
    public GameObject[] cubes;
    public float[] volumes;
    public int maxarray;

    private bool creating;
    private Vector3 minx;
    private Vector3 maxx;
    private Vector3 height;

    public GameObject cube;
    public Camera camera;

    Ray ray;
    RaycastHit hit;

    // Use this for initialization
    void Start () {
        creating = false;
        minx = new Vector3(0, 0, 0);
        maxx = new Vector3(0, 0, 0);
        cubes = new GameObject[10];
        maxarray = 0;

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.RightShift))
        {
            creating = false;
        }

        if (Input.GetMouseButtonDown(0) & !creating)
        {
            //Vector3 spawnPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log(spawnPosition.ToString());
            //GameObject newCube = Instantiate(cube, spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
            Debug.Log("NOT");
            this.transform.position = camera.ScreenToWorldPoint(Input.mousePosition);
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out hit))
            {
                minx = hit.point;
                //Debug.Log(minx.ToString());
                creating = true;
                //GameObject newCube = Instantiate(cube, new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z), Quaternion.identity);
            }
            creating = true;
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
            ray = camera.ScreenPointToRay(Input.mousePosition);
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
            GameObject newCube = Instantiate(cube, new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z), Quaternion.identity);
            newCube.transform.position = new Vector3(minx.x+Mathf.Abs(minx.x-maxx.x), minx.y + Mathf.Abs(minx.y - maxx.y), minx.z + Mathf.Abs(minx.z - maxx.z));
            newCube.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            cubes[maxarray] = newCube;
            maxarray += 1;
        }
        maxx = new Vector3(0, 0, 0);
    }
}
