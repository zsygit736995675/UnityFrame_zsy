using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine.UI;
using System.Linq;

public class takePhoto : MonoBehaviour
{
    public string deviceName;
    //接收返回的图片数据
    WebCamTexture tex;
    public Texture2D _tex;



    public RawImage image;
    public RectTransform imageParent;
    public AspectRatioFitter imageFitter;

    // Device cameras
    WebCamDevice frontCameraDevice;
    WebCamDevice backCameraDevice;
    WebCamDevice activeCameraDevice;

    WebCamTexture frontCameraTexture;
    WebCamTexture backCameraTexture;
    WebCamTexture activeCameraTexture;

    // Image rotation
    Vector3 rotationVector = new Vector3(0f, 0f, 0f);

    // Image uvRect
    Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
    Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

    // Image Parent's scale
    Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    Vector3 fixedScale = new Vector3(-1f, 1f, 1f);



    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 20, 200, 80), "开启摄像头"))
        {
            // 调用摄像头
            StartCoroutine(start());
        }

        if (GUI.Button(new Rect(10, 120, 200, 80), "捕获照片"))
        {
            //捕获照片
            tex.Pause();
            StartCoroutine(getTexture());
        }

        if (GUI.Button(new Rect(10, 220, 200, 80), "再次捕获"))
        {
            //重新开始
            tex.Play();
        }

        if (GUI.Button(new Rect(220, 20, 200,80 ), "录像"))
        {
            //录像
            StartCoroutine(SeriousPhotoes());
        }

        if (GUI.Button(new Rect(10, 320, 200, 80), "停止"))
        {
            //停止捕获镜头
            tex.Stop();
            StopAllCoroutines();
        }

        if (tex != null)
        {

            // 捕获截图大小               —距X左屏距离   |   距Y上屏距离  
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), tex);
        }


        if (GUI.Button(new Rect(10, 420, 200, 80), "打开相机"))
        {
            StartCoroutine(StartRo());
        }

        if (GUI.Button(new Rect(10, 520, 200, 80), "切换摄像头"))
        {
            SwitchCamera();
        }


    }

    public IEnumerator StartRo()
    {

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        // Check for device cameras
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No devices cameras found");
        }

        // Get the device's cameras and create WebCamTextures with them
        frontCameraDevice = WebCamTexture.devices.Last();
        backCameraDevice = WebCamTexture.devices.First();

        frontCameraTexture = new WebCamTexture(frontCameraDevice.name);
        backCameraTexture = new WebCamTexture(backCameraDevice.name);

        // Set camera filter modes for a smoother looking image
        frontCameraTexture.filterMode = FilterMode.Trilinear;
        backCameraTexture.filterMode = FilterMode.Trilinear;

        // Set the camera to use by default
        SetActiveCamera(frontCameraTexture);

    }

    // Set the device camera to use and start it
    public void SetActiveCamera(WebCamTexture cameraToUse)
    {
        if (activeCameraTexture != null)
        {
            activeCameraTexture.Stop();
        }

        activeCameraTexture = cameraToUse;
        activeCameraDevice = WebCamTexture.devices.FirstOrDefault(device => device.name == cameraToUse.deviceName);

        image.texture = activeCameraTexture;
        image.material.mainTexture = activeCameraTexture;

        activeCameraTexture.Play();
    }


    // Switch between the device's front and back camera
    public void SwitchCamera()
    {
        SetActiveCamera(activeCameraTexture.Equals(frontCameraTexture) ?backCameraTexture : frontCameraTexture);
    }

    void Update()
    {
        // Skip making adjustment for incorrect camera data
        if (activeCameraTexture==null||activeCameraTexture.width < 100)
        {
            Debug.Log("Still waiting another frame for correct info...");
            return;
        }

        //// Rotate image to show correct orientation 
        //rotationVector.z = -activeCameraTexture.videoRotationAngle;
        //image.rectTransform.localEulerAngles = rotationVector;

        // Set AspectRatioFitter's ratio
      //  float videoRatio = (float)activeCameraTexture.width / (float)activeCameraTexture.height;
        //imageFitter.aspectRatio = videoRatio;

        // Unflip if vertically flipped
      //  image.uvRect =activeCameraTexture.videoVerticallyMirrored ? fixedRect : defaultRect;

        // Mirror front-facing camera's image horizontally to look more natural
       // imageParent.localScale =activeCameraDevice.isFrontFacing ? fixedScale : defaultScale;



        int ccwNeeded = -activeCameraTexture.videoRotationAngle;
        image.rectTransform.localEulerAngles = new Vector3(0f, 0f, ccwNeeded);

        float videoRatio = (float)activeCameraTexture.width / (float)activeCameraTexture.height;

        imageFitter.aspectRatio = videoRatio;

        if (activeCameraTexture.videoVerticallyMirrored)
            image.uvRect = new Rect(0, 1, 1, -1);  // flip on HORIZONTAL axis
        else
            image.uvRect = new Rect(0, 0, 1, 1); // no flip
    }



/// <summary>
/// 捕获窗口位置
/// </summary>
public IEnumerator start()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            deviceName = devices[0].name;
            tex = new WebCamTexture(deviceName, 750 , 1334, 12);
           
            tex.Play();
        }
    }

    /// <summary>
    /// 获取截图
    /// </summary>
    /// <returns>The texture.</returns>
    public IEnumerator getTexture()
    {
        yield return new WaitForEndOfFrame();
        Texture2D t = new Texture2D(Screen.width, Screen.width);
        t.ReadPixels(new Rect(0, 0 , Screen.width, Screen.width), 0, 0, true);
        //距X左的距离        距Y屏上的距离
        // t.ReadPixels(new Rect(220, 180, 200, 180), 0, 0, false);
        t.Apply();
        byte[] byt = t.EncodeToPNG();
        //		File.WriteAllBytes(Application.dataPath+"/Photoes/"+Time.time+".jpg",byt);
        tex.Play();
    }

    /// <summary>
    /// 连续捕获照片
    /// </summary>
    /// <returns>The photoes.</returns>
    public IEnumerator SeriousPhotoes()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            Texture2D t = new Texture2D(400, 300, TextureFormat.RGB24, true);
            t.ReadPixels(new Rect(Screen.width / 2 - 180, Screen.height / 2 - 50, 360, 300), 0, 0, false);
            t.Apply();
            print(t);
            byte[] byt = t.EncodeToPNG();
            //			File.WriteAllBytes(Application.dataPath + "/MulPhotoes/" + Time.time.ToString().Split('.')[0] + "_" + Time.time.ToString().Split('.')[1] + ".png", byt);
            Thread.Sleep(300);
        }
    }
}