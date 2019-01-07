using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AsImpL;
using UnityEngine.UI;

public class ObjectLoader : MonoBehaviour {

    public GameObject loadedObject;
    public ImportOptions importOptions = new ImportOptions();
    private ObjectImporter objImporter;
    private bool firstLoad = false;
    private static string filePath;
    private static bool fpdone = false;
    public Text progressText;
    public GameObject loadButton;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (firstLoad)
        {
            if (objImporter.AllImported)
            {
                Camera.main.GetComponent<CameraController>().WhenLoaded();
                firstLoad = false;
                StartCoroutine(wait());
                progressText.enabled = false;
                loadButton.SetActive(false);
            }
        }

        if (fpdone)
        {
            loadObject();
            fpdone = false;
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

    public void loadObject() {
        firstLoad = true;
        objImporter = loadedObject.AddComponent<ObjectImporter>();
        importOptions.buildColliders = true;
        objImporter.ImportModelAsync("loadedObject", filePath, loadedObject.transform, importOptions);
        
    }

    public static void setFilepath(string fp)
    {
        filePath = fp;
        fpdone = true;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(5);
    }
}


