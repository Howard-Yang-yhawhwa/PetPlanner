using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHelper : MonoBehaviour
{
    List<GameObject> screenshotObjects = new List<GameObject>();
    GameObject displayObject;

    private void Awake()
    {
        foreach(Transform child in transform) {
            screenshotObjects.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        displayObject = screenshotObjects[0];
        displayObject.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12)) {
            TakeScreenshot();
        }

        // Switch between objects to display using left and right arrow keys
        if(Input.GetKeyDown(KeyCode.LeftArrow)) {
            displayObject.SetActive(false);
            int currentIndex = screenshotObjects.IndexOf(displayObject);
            int newIndex = currentIndex - 1;
            if(newIndex < 0) {
                newIndex = screenshotObjects.Count - 1;
            }
            displayObject = screenshotObjects[newIndex];
            displayObject.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.RightArrow)) {
            displayObject.SetActive(false);
            int currentIndex = screenshotObjects.IndexOf(displayObject);
            int newIndex = currentIndex + 1;
            if(newIndex >= screenshotObjects.Count) {
                newIndex = 0;
            }
            displayObject = screenshotObjects[newIndex];
            displayObject.SetActive(true);
        }
    }

    private void TakeScreenshot()
    {
        // Take screenshot of the screen and save it to the path with the object's name as the filename
        string folderPath = Application.persistentDataPath + "/Screenshots";
        string pathname = folderPath + "/" + displayObject.name + ".png";

        // Create the folder if it doesn't exist
        if(!System.IO.Directory.Exists(folderPath)) {
            System.IO.Directory.CreateDirectory(folderPath);
        }
        
        ScreenCapture.CaptureScreenshot(pathname);

    }
}
