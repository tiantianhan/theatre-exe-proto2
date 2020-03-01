using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Rendering;
using System.IO;
using UnityEngine.UI;

public class ClickPhoto : MonoBehaviour
{
    [SerializeField]
    RenderTexture renderTexture;

    [SerializeField]
    ARCameraBackground m_ARCameraBackground;

    [SerializeField]
    ARCameraManager m_ARCameraManager;
    
    Texture2D m_LastCameraTexture;

    

    [SerializeField]
    RawImage flash_image;

    Color originalColor;

    float duration = 53;
    float smoothness = 0.02f;
    


    private void Start()
    {
        originalColor = flash_image.color;
    }

    //public void clickPhotoNow()
    //{

    //    GameObject.Find("Button").GetComponentInChildren<Text>().text = "CLICKED!!!!";

    //    //Graphics.Blit(null, renderTexture, m_ARCameraBackground.material);


    //    // Copy the RenderTexture from GPU to CPU
    //    //var activeRenderTexture = RenderTexture.active;
    //    //RenderTexture.active = renderTexture;

    //    m_LastCameraTexture = new Texture2D(m_ARCameraBackground.material.mainTexture.width, m_ARCameraBackground.material.mainTexture.height, TextureFormat.RGB24, true);
    //    m_LastCameraTexture.ReadPixels(new Rect(0, 0, m_ARCameraBackground.material.mainTexture.width, m_ARCameraBackground.material.mainTexture.height), 0, 0);
    //    m_LastCameraTexture.Apply();
    //   // RenderTexture.active = activeRenderTexture;

    //    // Write to file
    //    //byte[] bytes = m_LastCameraTexture.EncodeToPNG();
    //    //File.WriteAllBytes(Application.persistentDataPath + "/camera_image", bytes);
    //    NativeGallery.SaveImageToGallery(m_LastCameraTexture, "Camera", "hiiipppp");



    //}

    public void Snapshot()
    {
        StartCoroutine(CaptureScreen());
    }

    public IEnumerator CaptureScreen()
    {
        yield return null;

        //disable UI for rendering image
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        //read the screen buffer oly after camera and gui are rendered but before its displayed - therefore when I comment it out, I can see both UI and VO
        yield return new WaitForEndOfFrame();

        m_LastCameraTexture = new Texture2D(m_ARCameraBackground.material.mainTexture.width, m_ARCameraBackground.material.mainTexture.height, TextureFormat.RGB24, true);
        
        //read screen content into texture 
        m_LastCameraTexture.ReadPixels(new Rect(0, 0, m_ARCameraBackground.material.mainTexture.width, m_ARCameraBackground.material.mainTexture.height), 0, 0);
        m_LastCameraTexture.Apply();

        //save image
        NativeGallery.SaveImageToGallery(m_LastCameraTexture, "Camera", "hiiipppOOp");

        //Flash camera image and fade it over time (0 - 100) within 3 seconds 
        yield return StartCoroutine(FlashImage());

        
        //enable UI  for user again 
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        yield return new WaitForSeconds(0.3f);

        flash_image.color = originalColor;

      
    }

    IEnumerator FlashImage()
    {
        float progress = 0;
        float increment = smoothness / duration;
        

        while (progress < 1)
        {
            flash_image.color = Color.Lerp(flash_image.color, Color.red, progress);
            progress += increment;
            
        }
        yield return new WaitForSeconds(smoothness);
 
    }
}
