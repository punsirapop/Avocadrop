using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System;
using UnityEngine.Rendering;
using Unity.VisualScripting;
using System.Text.RegularExpressions;
using UnityEditor;


public class Screenshot : MonoBehaviour
{
    /*
    [SerializeField] Image imgArea;

    Texture2D img;

    private void Awake()
    {
        img = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("TAKING A SHOT");
            StartCoroutine(Capture());
        }
    }

    private IEnumerator Capture()
    {
        yield return new WaitForEndOfFrame();

        Rect region = new Rect(0, 0, Screen.width, Screen.height);

        img.ReadPixels(region, 0, 0, false);
        img.Apply();
    }

    private void ShowImg()
    {
        Sprite imgSprite = Sprite.Create(img, new Rect(0f, 0f, img.width, img.height), new Vector2(.5f, .5f), 100f);
        imgArea.sprite = imgSprite;
    }
    */

    /*
    [SerializeField] Camera cam;
    [SerializeField] Image Display;

    Texture2D shot;

    private void Awake()
    {
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
        cam = GetComponent<Camera>();
        shot = new Texture2D(cam.pixelWidth, cam.pixelHeight, TextureFormat.ARGB32, false);
    }

    private void OnDisable()
    {
        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void HandlePhaseChanged(Phase phase)
    {
        if (phase == Phase.GameEnd)
        {
            StartCoroutine(TakeShot());
        }
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
    */

    [SerializeField] Transform env;
    [SerializeField] Camera myCamera;
    [SerializeField] Image toShow;

    bool takeShot = false;
    string path, persistentPath;
    Texture2D screenshotTexture;

    private void Awake()
    {
        // myCamera = gameObject.GetComponent<Camera>();
        // RenderPipelineManager.endFrameRendering += PleaseCaptureScreenshot;
        PhaseManager.OnPhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        // RenderPipelineManager.endFrameRendering -= PleaseCaptureScreenshot;
        PhaseManager.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void HandlePhaseChanged(Phase phase)
    {
        if (phase == Phase.GameEnd)
        {
            // takeShot = true;
            StartCoroutine(PleaseCaptureScreenshot());
        }
    }

    private IEnumerator PleaseCaptureScreenshot()//ScriptableRenderContext src, Camera[] cam
    {
        yield return new WaitForEndOfFrame();
        {
            // takeShot = false;
            int h = Screen.height, w = Screen.height;
            Debug.Log(env.rotation.eulerAngles.z);
            switch (env.rotation.eulerAngles.z % 180)
            {
                case 0:
                    w = Screen.height - 25;
                    h = Screen.height;
                    break;
                case 90:
                    w = Screen.height + 500;
                    h = Screen.height;
                    break;
                default:
                    break;
            }

            RenderTexture rt = new RenderTexture(w, h, 16);
            myCamera.targetTexture = rt;
            RenderTexture.active = rt;
            myCamera.Render();

            screenshotTexture = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, rt.width, rt.height);
            screenshotTexture.ReadPixels(rect, 0, 0);
            RenderTexture.active = null;
            screenshotTexture.Apply();

            Sprite photoSprite = Sprite.Create(screenshotTexture,
                new Rect(0f, 0f, screenshotTexture.width, screenshotTexture.height),
                new Vector2(.5f, .5f), 100f);
            toShow.sprite = photoSprite;
        }
    }

    public void SaveImage()
    {
        byte[] byteArray = screenshotTexture.EncodeToPNG();
        SetPath();
        File.WriteAllBytes(path, byteArray);
    }

    private void SetPath()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar
            + "Saves" + Path.AltDirectorySeparatorChar + DateTime.Now.ToString("dd-M-yyyy_HH-mm-ss") + ".png";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar
            + "Saves" + Path.AltDirectorySeparatorChar + DateTime.Now.ToString("dd-M-yyyy_HH-mm-ss") + ".png";
        Debug.Log("Save path: " + path);
    }

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
