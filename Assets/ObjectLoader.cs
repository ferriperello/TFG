using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsImpL;

public class ObjectLoader : MonoBehaviour {

    public GameObject loadedObject;
    public ImportOptions importOptions = new ImportOptions();
    private ObjectImporter objImporter;

    // Use this for initialization
    void Start () {
        string filePath = Menu_Manager.GetFilePath();
        Debug.Log("LOADER");
        Debug.Log(filePath);
        objImporter = loadedObject.AddComponent<ObjectImporter>();
        importOptions.buildColliders = true;

        objImporter.ImportModelAsync("test", filePath, null, importOptions);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
