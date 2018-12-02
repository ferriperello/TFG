using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using SimpleFileBrowser;

public class Menu_Manager : MonoBehaviour {

    public Text textField;
    private static string filePath;

	// Use this for initialization
	void Start () {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Objects", ".obj"));
        FileBrowser.SetDefaultFilter(".obj");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

    }
	
	// Update is called once per frame
	void Update () {
	}

    public void ChangeLevel()
    {
        Debug.Log(filePath);
        Debug.Log("CHANGEEE");
        /*
         Si el inputfield esta buid, llavors carregar de la opcio que toqui, la 1a per defecte en blanc i
         i la resta amb les subcarpetas que es posaran en 
         * */

        SceneManager.LoadScene("EditorScene", LoadSceneMode.Single);
    }

    public static string GetFilePath()
    {
        return filePath;
    }

    public void OpenBrowser()
    {
        StartCoroutine(ShowLoadDialogCoroutine());
        
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
        if (FileBrowser.Success)
        {
            filePath = FileBrowser.Result;
            textField.text = filePath;
        }
        
    }
}

