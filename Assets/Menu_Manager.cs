using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEditor;

public class Menu_Manager : MonoBehaviour {

    public InputField inputField;
    private static string fileRoute;
    private static string filePath;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        fileRoute = inputField.text;
	}

    public void ChangeLevel() {
        Debug.Log(fileRoute);
        Debug.Log(filePath);
        Debug.Log("CHANGEEE");
        /*
         Si el inputfield esta buid, llavors carregar de la opcio que toqui, la 1a per defecte en blanc i
         i la resta amb les subcarpetas que es posaran en 
         * */

        SceneManager.LoadScene("EditorScene", LoadSceneMode.Single);
    }

    public static string GetFilename() {
        return fileRoute;
    }
    public static string GetFilepath()
    {
        return filePath;
    }

    public void Filepath()
    {
        filePath = EditorUtility.OpenFilePanel("select OBJ", Application.persistentDataPath, "obj");
        if (filePath.Length != 0)
        {
            Debug.Log(filePath);
        }
    }
}

