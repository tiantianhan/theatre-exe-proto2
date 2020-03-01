using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

//Ref: https://www.youtube.com/watch?v=iM0ghkvsRos&list=PLuy9D2-sFABrVNVxpB_kPl6tHaC-QBmAP&index=5
public class InvisibleObjectManager : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    
    [SerializeField]
    private Text trackedImageText;

    [SerializeField]
    private string[] refImageNames;

    [SerializeField]
    private GameObject[] prefabs;

    private Dictionary<string, GameObject> objectForRefs;

    private void Awake() {

        //Set up object dictionary
        Debug.Assert(refImageNames.Length == prefabs.Length, "Image and object arrays have unequal sizes.");    

        objectForRefs = new Dictionary<string, GameObject>();

        for(int i = 0; i < refImageNames.Length; i++){
            //Instantiate and hide
            GameObject instance = Instantiate(prefabs[i], new Vector3(0, 0, 0), Quaternion.identity, this.transform);
            // instance.SetActive(false);
            
            //Put in dictionary
            objectForRefs[refImageNames[i]] = instance;
            trackedImageText.text = "" + objectForRefs.Count;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs){
        foreach (ARTrackedImage trackedImage in eventArgs.added){
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated){
            UpdateARImage(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed){
            objectForRefs[trackedImage.referenceImage.name].SetActive(false);
        }
    }
    
    void UpdateARImage(ARTrackedImage trackedImage){
        string imageName = trackedImage.referenceImage.name;
        
        
        objectForRefs[imageName].SetActive(true);
        objectForRefs[imageName].transform.position = trackedImage.transform.position;

        trackedImageText.text = imageName + objectForRefs.Count;
    }
}
