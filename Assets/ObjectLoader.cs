using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsImpL;

public class ObjectLoader : MonoBehaviour {

    public GameObject loadedObject;
    public ImportOptions importOptions = new ImportOptions();
    private ObjectImporter objImporter;
    private bool firstLoad = false;

    // Use this for initialization
    void Start () {
        string filePath = Menu_Manager.GetFilePath();
        Debug.Log("LOADER");
        Debug.Log(filePath);
        objImporter = loadedObject.AddComponent<ObjectImporter>();
        importOptions.buildColliders = true;

        objImporter.ImportModelAsync("loadedObject", filePath, loadedObject.transform, importOptions);
        firstLoad = true;
        //objImporter.ImportFile(filePath, loadedObject.transform, importOptions);  
    }
	
	// Update is called once per frame
	void Update () {
        if (firstLoad)
        {
            if (objImporter.AllImported)
            {
                Camera.main.GetComponent<CameraController>().WhenLoaded();
                firstLoad = false;
            }
        }
    }

    public class CustomObjImporter : ObjectImporter
    {
        protected override void OnImportingComplete()
        {
            print("ON IMPORT COMPLETE");
            base.OnImportingComplete();
        }
    }
}


