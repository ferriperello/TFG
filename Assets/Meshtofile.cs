using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using UnityEngine.UI;

public class Meshtofile : MonoBehaviour {

    private const string MTL_FILE_NAME_SUFFIX = "Material";
    private const string DEFAULT_MATERIAL_NAME = "Default";
    private const string DEFAULT_MESH_NAME = "Mesh";
    private Mesh mesh;
    private Material material;
    private static string path;
    private int triangleSize;
    public GameObject GO;
    public Text progressText;
    public GameObject saveButton;

    private static bool pathSet = false;

    public void SetMesh(Mesh m)
    {
        mesh = m;
    }

    public void SetMat(Material m)
    {
        material = m;
    }

    public static void Setpath(string p)
    {
        path = p;
        pathSet = true;
    }

    public void StartSave()
    {
        Debug.Log("ENTRO");
        triangleSize = RayCasting_Expansion.GetTriangleSize();
        mesh = GO.GetComponentInChildren<MeshFilter>().mesh;
        material = GO.GetComponentInChildren<MeshRenderer>().material;
        //path = "C:/Users/ferra/Documents/TFG/exportTry/";
        Save(mesh, material, path);
        Debug.Log("DONE");
    }

    public void Save(Mesh m, Material mat, string path)
    {
        mesh = m;
        material = mat;

        string objFileContent = GetObjFileContent();
        string mtlFileContent = GetMtlFileContent();
        try
        {
            string objFilePath = Path.Combine(path, GetObjFileName());
            File.WriteAllText(objFilePath, objFileContent);

            string mtlFilePath = Path.Combine(path, GetMtlFileName());
            File.WriteAllText(mtlFilePath, mtlFileContent);

            if (material != null && material.mainTexture != null)
            {
                string texturePath = material.mainTexture.name;
                //string texturePath = AssetDatabase.GetAssetPath(material.mainTexture);
                string newTexturePath = Path.Combine(path, GetMainTextureFileName());
                File.Copy(texturePath, newTexturePath, true);
            }
        }
        catch (System.IO.IOException e)
        {
            Debug.Log(e.ToString());
        }
    }

    public string GetObjFileContent()
    {

        //Vector3[] vertices = mesh.vertices;
        //Vector3[] vertices = CalculateVertices();
        Vector3[] vertices = new Vector3[triangleSize];
        Vector3[] normals =  new Vector3[triangleSize];
        CalculateVertices(vertices, normals);

        //Vector3[] normals = mesh.normals;
        Vector2[] uvCoords = mesh.uv;
        int[] triangles = mesh.triangles;

        string header = CreateHeader(mesh.name);
        //crear llista de vertex a partir dels triangles
        string verticesList = ListVertices(vertices);
        string textureCoordsList = ListTextureCoords(uvCoords);
        string normalsList = ListNormals(normals);
        string materialRefs = ListMaterialRefs();
        string facesList = ListFaces(triangles, vertices.Length, normals.Length, uvCoords.Length);

        string s = new StringBuilder()
            .Append(header)
            .Append(materialRefs)
            .Append(verticesList)
            .Append(textureCoordsList)
            .Append(normalsList)
            .Append(facesList)
            .ToString();
        return s;
    }

    private string CreateHeader(string meshName)
    {
        StringBuilder sb = new StringBuilder()
            .AppendFormat("o {0}", meshName).Append("\n");
        return sb.ToString();
    }

    private string ListVertices(Vector3[] vertices)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("# Vertices list").Append("\n");
        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 v = vertices[i];
            sb.AppendFormat("v {0} {1} {2}", -v.x, v.y, v.z).Append("\n");
        }
        return sb.ToString();
    }

    private string ListTextureCoords(Vector2[] uvCoords)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("# Texture coordinates list").Append("\n");
        for (int i = 0; i < uvCoords.Length; ++i)
        {
            Vector2 uv = uvCoords[i];
            sb.AppendFormat("vt {0} {1} 0", uv.x, uv.y).Append("\n");
        }
        return sb.ToString();
    }

    private string ListNormals(Vector3[] normals)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("# Normal vectors list").Append("\n");
        for (int i = 0; i < normals.Length; ++i)
        {
            Vector3 n = normals[i];
            sb.AppendFormat("vn {0} {1} {2}", n.x, n.y, n.z).Append("\n");
        }
        return sb.ToString();
    }

    private string ListMaterialRefs()
    {
        StringBuilder sb = new StringBuilder()
            .Append("# Material file references").Append("\n")
            .AppendFormat("mtllib {0}", GetMtlFileName()).Append("\n")
            .AppendFormat("usemtl {0}", DEFAULT_MATERIAL_NAME).Append("\n");
        return sb.ToString();
    }

    private string ListFaces(int[] triangles, int vLength, int nLength, int uvLength)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("# Faces list").Append("\n");
        for (int i = 0; i < triangleSize - 2; i += 3)
        {
            sb.AppendFormat("f {0}/{1}/{2} {3}/{4}/{5} {6}/{7}/{8}",
                (triangles[i + 2] + 1).ToString(),
                vLength == nLength ? (triangles[i + 2] + 1).ToString() : "",
                vLength == uvLength ? (triangles[i + 2] + 1).ToString() : "",
                (triangles[i + 1] + 1).ToString(),
                vLength == nLength ? (triangles[i + 1] + 1).ToString() : "",
                vLength == uvLength ? (triangles[i + 1] + 1).ToString() : "",
                (triangles[i + 0] + 1).ToString(),
                vLength == nLength ? (triangles[i + 0] + 1).ToString() : "",
                vLength == uvLength ? (triangles[i + 0] + 1).ToString() : "")
                .Append("\n");
        }
        return sb.ToString();
    }

    private string GetObjFileName()
    {
        return string.Format("{0}{1}", mesh != null ? mesh.name : DEFAULT_MESH_NAME, ".obj");
    }

    private string GetMtlFileName()
    {
        return string.Format("{0}{1}{2}", mesh != null ? mesh.name : string.Empty, MTL_FILE_NAME_SUFFIX, ".mtl");
    }

    
    private string GetMainTextureFileName()
    {
        if (material == null || material.mainTexture == null)
            return null;
        string mainTexturePath = material.mainTexture.name;
        //string mainTexturePath = GetAssetPath(material.mainTexture);
        return mainTexturePath;
    }

    private string GetMtlFileContent()
    {
        if (material == null)
            return GetDefaultMtlFileContent();

        StringBuilder sb = new StringBuilder();
        sb.Append("# Define a new material with default name").Append("\n");
        sb.AppendFormat("newmtl {0}", DEFAULT_MATERIAL_NAME).Append("\n");

        Color mainColor = material.color;
        sb.Append("# Diffuse/Tint color").Append("\n");
        sb.AppendFormat("Kd {0} {1} {2}", mainColor.r, mainColor.g, mainColor.b).Append("\n");
        sb.AppendFormat("d {0}", mainColor.a).Append("\n");
        sb.AppendFormat("Tr {0}", 1 - mainColor.a).Append("\n");

        
        if (material.mainTexture != null)
        {
            sb.Append("# Diffuse map").Append("\n");
            sb.AppendFormat("map_Kd {0}", GetMainTextureFileName()).Append("\n");
        }

        return sb.ToString();
    }

    private string GetDefaultMtlFileContent()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("# Define a new material with default name").Append("\n");
        sb.AppendFormat("newmtl {0}", DEFAULT_MATERIAL_NAME).Append("\n");

        Color mainColor = Color.white;
        sb.Append("# Diffuse/Tint color").Append("\n");
        sb.AppendFormat("Kd {0} {1} {2}", mainColor.r, mainColor.g, mainColor.b).Append("\n");
        sb.AppendFormat("d {0}", mainColor.a).Append("\n");
        sb.AppendFormat("Tr {0}", 1 - mainColor.a).Append("\n");

        return sb.ToString();
    }

    private Vector3[] CalculateVertices(Vector3[] finalvertex, Vector3[] finalnormals)
    {
        int[] triangles = mesh.triangles;
        Vector3[] vertex = mesh.vertices;
        Vector3[] normals = mesh.normals;
        for (int i=0; i < triangleSize; i++)
        {
            //Debug.Log(triangles[i]);
            finalvertex[i] = vertex[triangles[i]];
            finalnormals[i] = normals[triangles[i]];
        }
        return finalvertex;
    }

    private bool CheckSave()
    {

        try {
            mesh = GO.GetComponentInChildren<MeshFilter>().mesh;
            material = GO.GetComponentInChildren<MeshRenderer>().material;
        }
        catch (NullReferenceException e) {
            return false;
        }

        return true;
    }

    void Update()
    {
        if (CheckSave())
        {
            if (pathSet)
            {
                pathSet = false;
                StartSave();
            }
            else
            {
                //Debug.Log("False pathset");
                saveButton.SetActive(true);
            }
        }
        else {
            saveButton.SetActive(false);
        }
    }
}
