using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using SimpleFileBrowser;

public class Menu_Manager : MonoBehaviour {

    private static string filePath;
    
    public GameObject CubePanel;
    public GameObject RTPanel;
    public GameObject EPPanel;
    private bool CubePanelOn = false;
    private bool RTPanelOn = false;
    private bool EPPanelOn = false;


    // Use this for initialization
    void Start () {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Objects", ".obj"));
        FileBrowser.SetDefaultFilter(".obj");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

    }
	
	// Update is called once per frame
	void Update () {
	}

    public void OpenBrowser()
    {
        ToggleOffAll();
        StartCoroutine(ShowLoadDialogCoroutine());
        
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Objects", ".obj"));
        FileBrowser.SetDefaultFilter(".obj");

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
            ObjectLoader.SetFilepath(filePath);
        }
        
    }

    public void OpenSaver()
    {
        ToggleOffAll();
        StartCoroutine(ShowSaveDialogCoroutine());
    }

    IEnumerator ShowSaveDialogCoroutine()
    {
        // Show a save file dialog and wait for a response from user
        // save file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForSaveDialog(true, null, "Save TXT", "Save");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);
        if (FileBrowser.Success)
        {
            filePath = FileBrowser.Result;
            ExportTrianglesandCubes.SetExportPath(filePath+"\\");

        }
    }

    public void ToggleCubePanel()
    {
        if (CubePanelOn)
        {
            CubePanelOn = false;
            CubePanel.SetActive(false);
        }
        else
        {
            CubePanelOn = true;
            RTPanelOn = false;
            EPPanelOn = false;
            RTPanel.SetActive(false);
            EPPanel.SetActive(false);
            CubePanel.SetActive(true);
        }
    }

    public void ToggleRTPanel()
    {
        if (RTPanelOn)
        {
            RTPanelOn = false;
            RTPanel.SetActive(false);
        }
        else
        {
            RTPanelOn = true;
            CubePanelOn = false;
            EPPanelOn = false;
            EPPanel.SetActive(false);
            CubePanel.SetActive(false);
            RTPanel.SetActive(true);
        }
    }

    public void ToggleEPanel()
    {
        if (EPPanelOn)
        {
            EPPanelOn = false;
            EPPanel.SetActive(false);
        }
        else
        {
            EPPanelOn = true;
            CubePanelOn = false;
            RTPanelOn = false;
            RTPanel.SetActive(false);
            CubePanel.SetActive(false);
            EPPanel.SetActive(true);
        }
    }

    private void ToggleOffAll()
    {
        CubePanel.SetActive(false);
        RTPanel.SetActive(false);
        EPPanel.SetActive(false);
        CubePanelOn = false;
        RTPanelOn = false;
        EPPanelOn = false;
    }
}

