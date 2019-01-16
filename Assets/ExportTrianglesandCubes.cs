using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class ExportTrianglesandCubes : MonoBehaviour {

    public GameObject GO;
    public Text progressText;
    public GameObject saveButton;
    private static string path;
    public bool[] findedTriangles;
    public static ArrayList cubes;
    private int triangleSize;

    private static bool pathSet = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (pathSet)
        {
            pathSet = false;
            StartSave();
        }
        else
        {
            //Debug.Log("False pathset");
            //saveButton.SetActive(true);
        }
    }

    public static void SetExportPath(string p)
    {
        path = p;
        pathSet = true;
    }

    public void StartSave()
    {
        findedTriangles = RayCasting_Expansion.GetFindedTriangles();
        triangleSize = RayCasting_Expansion.GetTriangleSize()/3;
        cubes = CubeManaging.GetCubesArray();
        Save();
    }

    private void Save()
    {
        string fileContent = GetFileContent();
        string actualDate = System.DateTime.Now.ToString("s");
        actualDate = actualDate.Replace(':', '_');
        //Escriure-ho tot al fitxer
        try
        {
            string dataFilePath = Path.Combine(path, actualDate+".txt");
            File.WriteAllText(dataFilePath, fileContent);
        }
        catch (System.IO.IOException e)
        {
            Debug.Log(e.ToString());
        }
    }

    private string GetFileContent()
    {
        //Llista de totes les posicion que han sigut trobades
        string trianglesString = CreateStringBoolean();

        //A cada linea, id cube, transform, scale
        string cubesString = CreateStringCubes();

        string s = new StringBuilder()
            .Append(trianglesString)
            .Append(cubesString)
            .ToString();

        return s;
    }

    private string CreateStringBoolean()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0} Triangles of a total of {1} ", triangleSize,findedTriangles.Length).Append("\n");
        for (int i = 0; i < findedTriangles.Length; i++)
        {
            if (findedTriangles[i])
            {
                sb.AppendFormat("{0} , ", i);
            }
        }
        sb.Append("\n");

        return sb.ToString();
    }

    private string CreateStringCubes()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("{0} Cubes", cubes.Count).Append("\n");
        for (int i = 0; i < cubes.Count; i++)
        {
            GameObject aCube = CubeManaging.GetCubeinArray(i);
            Vector3 position = aCube.transform.position;
            Vector3 scale = aCube.transform.lossyScale;

            sb.AppendFormat("{0} , {1} , {2} , {3} , {4} , {5} , {6}", i, position.x, position.y, position.z, scale.x, scale.y, scale.z).Append("\n");
        }
        sb.Append("\n");
        return sb.ToString();
    }
}
