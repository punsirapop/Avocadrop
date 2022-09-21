using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEditor;

public class Screenshot : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Image Display;

    Texture2D shot;

    public static Screenshot Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        cam = GetComponent<Camera>();
        shot = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.ARGB32, false);
    }

    public static void Capture()
    {
        Instance.StartCoroutine(Instance.TakeShot());
    }

    private IEnumerator TakeShot()
    {
        yield return new WaitForEndOfFrame();

        Rect rect = new Rect(0, 0, cam.pixelWidth, cam.pixelHeight);
        shot.ReadPixels(rect, 0, 0, false);
        shot.Apply();

        ShowPhoto();
    }

    private void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(shot, new Rect(0f, 0f, cam.pixelWidth, cam.pixelHeight),
            new Vector2(.5f, .5f), 100f);
        Display.sprite = photoSprite;
    }

    /*
    public static Screenshot Instance;

    [SerializeField] Camera myCamera;

    bool takeShot = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        myCamera = gameObject.GetComponent<Camera>();
        RenderPipelineManager.endFrameRendering += PleaseCaptureScreenshot;
    }

    private void OnDisable()
    {
        RenderPipelineManager.endFrameRendering -= PleaseCaptureScreenshot;
    }

    private void PleaseCaptureScreenshot(ScriptableRenderContext src, Camera[] cam)
    {
        if (takeShot)
        {
            takeShot = false;

            RenderTexture rt = new RenderTexture(Screen.height, Screen.height, 16);
            myCamera.targetTexture = rt;
            RenderTexture.active = rt;
            myCamera.Render();

            Texture2D screenshotTexture = new Texture2D(rt.height, rt.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, rt.height, rt.height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            RenderTexture.active = null;
            screenshotTexture.Apply();

            byte[] byteArray = screenshotTexture.EncodeToPNG();
            string path = Application.dataPath + Path.AltDirectorySeparatorChar
                + "Saves" + Path.AltDirectorySeparatorChar + "pic.png";
            File.WriteAllBytes(path, byteArray);
        }
    }

    private void PleaseCapture()
    {
        takeShot = true;
    }

    public static void Capture()
    {
        Instance.PleaseCapture();
    }
    */

    /*
    public static Screenshot Instance;
    Camera myCamera;

    bool isTakingShot = false;
    string path;
    private void Awake()
    {
        if(Instance == null) Instance = this;
        myCamera = gameObject.GetComponent<Camera>();
        RenderPipelineManager.endCameraRendering += PleaseTakeScreenshot;
    }

    private void PleaseTakeScreenshot(ScriptableRenderContext context, Camera camera)
    {
        if (isTakingShot)
        {
            isTakingShot = false;
            RenderTexture rt = myCamera.targetTexture;

            Texture2D renderResult = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, rt.width, rt.height);
            renderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = renderResult.EncodeToPNG();
            path = Application.dataPath + Path.AltDirectorySeparatorChar
                + "Saves" + Path.AltDirectorySeparatorChar + "pic.png";
            File.WriteAllBytes(path, byteArray);
            Debug.Log("Took screenshot at " + path);

            RenderTexture.ReleaseTemporary(rt);
            myCamera.targetTexture = null;
        }
    }

    private void TakeScreenshot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        isTakingShot = true;
    }

    public void TakeShot(int width, int height)
    {
        TakeScreenshot(width, height);
    }
    */
}
