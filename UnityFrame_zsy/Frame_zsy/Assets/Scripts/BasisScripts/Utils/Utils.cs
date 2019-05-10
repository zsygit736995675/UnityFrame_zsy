using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Utils : MonoBehaviour
{
    private static Utils ins;
    private static readonly System.Random mRandom = new System.Random();

    private const int SECONDS_OF_DAY = 24 * 60 * 60;
    private const int SECONDS_OF_HOUR = 60 * 60;
    private const int SECONDS_OF_MINUTE = 60;

    public static int ScreenWidth = 1136;
    public static int ScreenHeight = 640;
    public static int HalfScreenWidth = ScreenWidth / 2;
    public static int HalfScreenHeight = ScreenHeight / 2;

    public delegate void Event<T>(T arg);

    public delegate void VoidDelegate();
    public delegate void TypeDelegate(Type type);
    public delegate void BoolDelegate(bool arg);
    public delegate bool ReturnBoolDelegate();
    public delegate void IntDelegate(int arg);
    public delegate void Int2Delegate(int arg1, int arg2);
    public delegate void FloatDelegate(float arg);
    public delegate void LongDelegate(long arg);
    public delegate void StringDelegate(string arg);
    public delegate void ObjectDelegate(System.Object arg);
    public delegate void ObjectArrayDelegate(params System.Object[] args);
    public delegate void UnityObjectDelegate(UnityEngine.Object arg);
    public delegate void GameObjectDelegate(UnityEngine.GameObject arg);
    public delegate void UnityObjectArrayDelegate(UnityEngine.Object[] arg);
    public delegate void DataDelegate<in T>(T arg);



    //public delegate void StartLoad(string path, cfg.CfgManager.OnLoaded onLoaded);

    private static string mPersistentDataPath = "";

    private static readonly DateTime date_1970 = new DateTime(1970, 1, 1);

    public static void Init()
    {
        mPersistentDataPath = Application.persistentDataPath;
        GameObject go = new GameObject("utils");
        go.hideFlags = HideFlags.HideAndDontSave;
        go.AddComponent<Utils>();
    }

    public static bool TriggerEvent(VoidDelegate e)
    {
        if (e == null)
            return false;
        e();
        return true;
    }

    public static bool TriggerEvent(IntDelegate e, int arg)
    {
        if (e == null)
            return false;
        e(arg);
        return true;
    }

    public static Coroutine WaitTrue(ReturnBoolDelegate func)
    {
        return StartConroutine(DoWaitTrue(func));
    }

    private static IEnumerator DoWaitTrue(ReturnBoolDelegate func)
    {
        while (!func())
        {
            yield return null;
        }
    }

    public static bool IsVisible(Bounds bounds, Camera camera)
    {
        Vector3 vExtents = bounds.extents;
        Vector3 vCenter = bounds.center;

        if (IsVisible(bounds.min, camera))
            return true;

        if (IsVisible(bounds.max, camera))
            return true;

        Vector3 vLeftTopFront = vCenter + new Vector3(-vExtents.x, vExtents.y, -vExtents.z);
        Vector3 vLeftTopBack = vCenter + new Vector3(-vExtents.x, vExtents.y, vExtents.z);
        Vector3 vLeftbottomBack = vCenter + new Vector3(-vExtents.x, -vExtents.y, vExtents.z);
        Vector3 vRightTopFront = vCenter + new Vector3(vExtents.x, vExtents.y, -vExtents.z);
        Vector3 vRightbottomFront = vCenter + new Vector3(vExtents.x, -vExtents.y, -vExtents.z);
        Vector3 vRightbottomBack = vCenter + new Vector3(vExtents.x, -vExtents.y, vExtents.z);

        if (IsVisible(vLeftTopFront, camera))
            return true;

        if (IsVisible(vLeftTopBack, camera))
            return true;

        if (IsVisible(vLeftbottomBack, camera))
            return true;

        if (IsVisible(vRightTopFront, camera))
            return true;

        if (IsVisible(vRightbottomFront, camera))
            return true;

        if (IsVisible(vRightbottomBack, camera))
            return true;

        return false;
    }

    public static bool IsVisible(Vector3 vPoint, Camera camera)
    {
        Vector3 vScreenPoint = camera.WorldToScreenPoint(vPoint);
        float fWidth = Screen.width;
        float fHeight = Screen.height;
        if (vScreenPoint.x < 0 || vScreenPoint.x > fWidth)
            return false;
        if (vScreenPoint.y < 0 || vScreenPoint.y > fHeight)
            return false;
        if (vScreenPoint.z < camera.nearClipPlane || vScreenPoint.z > camera.farClipPlane)
            return false;
        return true;
    }

    public static void Clamp<T>(ref T val, T min, T max) where T : System.IComparable
    {
        if (val.CompareTo(min) < 0)
            val = min;
        if (val.CompareTo(max) > 0)
            val = max;
    }

    public class CustomTransform
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;
    }

    public static T Min<T>(T a, T b) where T : System.IComparable
    {
        return a.CompareTo(b) < 0 ? a : b;
    }

    public static T Max<T>(T a, T b) where T : System.IComparable
    {
        return a.CompareTo(b) < 0 ? b : a;
    }

    public static void Swap<T>(ref T a, ref T b)
    {
        T t = a;
        a = b;
        b = t;
    }

    public static float DistanceFromPointToLine(Vector3 vPoint, Vector3 vLinePoint, Vector3 vLineDir, out Vector3 vVertical)
    {
        vLineDir.Normalize();
        Vector3 vPointToLine = vPoint - vLinePoint;
        float fDot = Vector3.Dot(vPointToLine, vLineDir);
        vVertical = vLinePoint + vLineDir * fDot;
        return (vPoint - vVertical).magnitude;
    }

    public static float DistanceFromPointToLine(Vector2 vPoint, Vector2 vLinePoint, Vector2 vLineDir, out Vector2 vVertical)
    {
        vLineDir.Normalize();
        Vector2 vPointToLine = vPoint - vLinePoint;
        float fDot = Vector2.Dot(vPointToLine, vLineDir);
        vVertical = vLinePoint + vLineDir * fDot;
        return (vPoint - vVertical).magnitude;
    }

    public static bool LineIntersect(Vector2 vPoint1, Vector2 vDir1, Vector2 vPoint2, Vector2 vDir2, out Vector2 vIntersect)
    {
        vIntersect = Vector2.zero;
        vDir1.Normalize();
        vDir2.Normalize();
        if (vDir1 == vDir2)
            return false;
        Vector2 vVertical;
        float fDistance = DistanceFromPointToLine(vPoint2, vPoint1, vDir1, out vVertical);
        Vector2 vVerticalDir = vVertical - vPoint2;
        float fVerticalLen = vVerticalDir.magnitude;
        vVerticalDir.Normalize();
        float fAngle = Quaternion.Angle(Quaternion.LookRotation(vDir2), Quaternion.LookRotation(vVerticalDir));
        if (fAngle > 90)
            vDir2 = -vDir2;
        float fLen = fVerticalLen / Mathf.Cos(fAngle);
        vIntersect = vPoint2 + vDir2 * fLen;
        return true;
    }


    public static string GetString(string text, params System.Object[] args)
    {
        return string.Format(text, args);
    }

    public static void DestroyChildren(GameObject go)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (UnityEngine.Transform t in go.transform)
        {
            t.parent = null;
            children.Add(t.gameObject);
        }

        foreach (GameObject child in children)
        {
            UnityEngine.Object.Destroy(child);
        }
    }

    public static T AddComponentIfNotExist<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }

        return t;
    }

    public static void AddModalCollider(GameObject go)
    {
        BoxCollider bc = go.GetComponent<BoxCollider>();
        if (bc == null)
        {
            bc = go.AddComponent<BoxCollider>();
            bc.size = new Vector3(10000, 10000, 0);
            bc.center = new Vector3(0, 0, 5);
        }
    }

    public static string GetRelativePath(GameObject parent, GameObject child)
    {
        string path = child.name;
        UnityEngine.Transform trans = child.transform.parent;
        while (trans != null && trans != parent.transform)
        {
            path = trans.name + "/" + path;
            trans = trans.parent;
        }

        return path;
    }

    public static Coroutine StartConroutine(IEnumerator routine)
    {
        return ins.StartCoroutine(routine);
    }

    public static void DontDestoryGameObject(GameObject go)
    {
        DontDestroyOnLoad(go);
    }

    public static void StopConroutine(Coroutine coroutine)
    {
        ins.StopCoroutine(coroutine);
    }

    private void Awake()
    {
        ins = this;
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        HalfScreenWidth = ScreenWidth / 2;
        HalfScreenHeight = ScreenHeight / 2;
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void Update()
    {
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        HalfScreenWidth = ScreenWidth / 2;
        HalfScreenHeight = ScreenHeight / 2;



    }
#endif

    public static Quaternion LookRotation(Vector3 forward, Quaternion defaultRotation)
    {
        if (forward.sqrMagnitude > 0.001f)
            return Quaternion.LookRotation(forward);
        return defaultRotation;
    }

    public static Vector3 GetScreenPosition(Vector3 vWorldPos, Camera worldCamera)
    {
        Vector3 vPos = worldCamera.WorldToScreenPoint(vWorldPos);
        vPos = Camera.main.ScreenToWorldPoint(vPos);
        return vPos;
    }

    public static float GetEffectDuration(GameObject effect, bool ignoreloop = true)
    {
        float duration = 0;
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
        if (particleSystem)
        {
            if (particleSystem.loop && !ignoreloop)
                return Mathf.Infinity;
            float time = particleSystem.startLifetime + particleSystem.startDelay;
            if (time > duration)
                duration = time;
            if (duration < particleSystem.duration)
                duration = particleSystem.duration;
        }
        Component[] particleSystems = effect.GetComponentsInChildren(typeof(ParticleSystem), true);
        foreach (Component c in particleSystems)
        {
            ParticleSystem ps = c as ParticleSystem;
            if (!ps)
                continue;
            if (ps.loop && !ignoreloop)
                return Mathf.Infinity;
            float time = ps.startLifetime + ps.startDelay;
            if (time > duration)
                duration = time;
            if (duration < ps.duration)
                duration = ps.duration;
        }
        return duration;
    }

    public static string GetTimeString(int iTimeInSecond)
    {
        if (iTimeInSecond <= 0)
        {
            return "00:00:00";
        }

        int iHour = iTimeInSecond / 3600;
        int iMinute = (iTimeInSecond - iHour * 3600) / 60;
        int iSecond = iTimeInSecond % 60;
        return (iHour < 10 ? "0" : "") + iHour + ":" + (iMinute < 10 ? "0" : "") + iMinute + ":" +
               (iSecond < 10 ? "0" : "") + iSecond;
    }

    public static IEnumerator CastGameObject(GameObject go, float initScale, float initAlpha, float duration)
    {
        Vector3 oriScale = go.transform.localScale;
        float curScale = initScale;
        float scaleSpeed = (initScale - 1) / duration;
        float lastTime = Time.time;
        while (duration > 0)
        {
            float curTime = Time.time;
            float deltaTime = curTime - lastTime;
            lastTime = curTime;
            duration -= deltaTime;
            curScale -= deltaTime * scaleSpeed;
            if (scaleSpeed > 0 && curScale < 1)
                curScale = 1;
            if (scaleSpeed < 0 && curScale > 1)
                curScale = 1;

            go.transform.localScale = oriScale * curScale;
            yield return true;
        }
        go.transform.localScale = oriScale;
        yield break;
    }

    public static IEnumerator Vibration(GameObject go, float duration, float radiusX, float radiusY, float radiusZ)
    {
        Vector3 initPos = go.transform.localPosition;
        float time = Time.realtimeSinceStartup;
        while (duration > 0)
        {
            float deltaTime = Time.realtimeSinceStartup - time;
            time = Time.realtimeSinceStartup;
            duration -= deltaTime;
            go.transform.localPosition = initPos + new Vector3(UnityEngine.Random.Range(-radiusX, radiusX), UnityEngine.Random.Range(-radiusY, radiusY), UnityEngine.Random.Range(-radiusZ, radiusZ));
            yield return new WaitForSeconds(0.01f);
        }
        go.transform.localPosition = initPos;
        yield break;
    }

    public static IEnumerator ScalePingpong(GameObject go, int times, float once_duration, float minScale, float maxScale, float attenuation)
    {
        Vector3 initScale = go.transform.localScale;
        if (minScale == maxScale)
            yield break;
        if (maxScale < minScale)
            Swap(ref minScale, ref maxScale);
        if (minScale < 0)
            minScale = 0;
        if (maxScale < 1)
            maxScale = 1;
        if (minScale > 1)
            minScale = 1;
        float speed = (maxScale - minScale) * 2 / once_duration;
        float curScale = 1;
        while (times > 0)
        {
            while (curScale > minScale)
            {
                curScale -= Time.deltaTime * speed;
                Utils.Clamp(ref curScale, minScale, maxScale);
                go.transform.localScale = initScale * curScale;
                yield return true;
            }
            while (curScale < maxScale)
            {
                curScale += Time.deltaTime * speed;
                Utils.Clamp(ref curScale, minScale, maxScale);
                go.transform.localScale = initScale * curScale;
                yield return true;
            }
            if (minScale < 1)
                minScale = 1 - (1 - minScale) * attenuation;
            if (maxScale > 1)
                maxScale = maxScale * attenuation;
            speed = (maxScale - minScale) * 2 / once_duration;
            --times;
        }
        go.transform.localScale = initScale;
        yield break;
    }

    public static IEnumerator RotatePingpong(GameObject go, int times, float once_duration, float angle, float attenuation, Vector3 forward)
    {
        if (angle == 0)
            yield break;
        Quaternion initRotation = go.transform.rotation;
        if (Math.Abs(angle) > 360)
            angle -= ((int)(angle / 360)) * 360;
        float minAngle = -angle;
        float maxAngle = angle;
        if (minAngle > maxAngle)
        {
            Swap(ref minAngle, ref maxAngle);
        }
        float speed = angle * 2 * 2 / once_duration;
        float curAngle = 0;
        while (times > 0)
        {
            while (curAngle > minAngle)
            {
                curAngle -= Time.deltaTime * speed;
                if (curAngle < minAngle)
                    curAngle = minAngle;
                go.transform.rotation = initRotation * (Quaternion.Euler(0, 0, curAngle) * Quaternion.LookRotation(forward));
                yield return true;
            }
            while (curAngle < maxAngle)
            {
                curAngle += Time.deltaTime * speed;
                if (curAngle > maxAngle)
                    curAngle = maxAngle;
                go.transform.rotation = initRotation * (Quaternion.Euler(0, 0, curAngle) * Quaternion.LookRotation(forward));
                yield return true;
            }
            if (minAngle < 0)
                minAngle = minAngle * attenuation;
            if (maxAngle > 0)
                maxAngle = maxAngle * attenuation;
            speed = (maxAngle - minAngle) * 2 / once_duration;
            --times;
        }
        go.transform.rotation = initRotation;
        yield break;
    }

    public static int Random(int min, int max)
    {
        return mRandom.Next(min, max);
    }

    public static float Random(float min, float max)
    {
        return (float)(mRandom.NextDouble() * (max - min)) + min;
    }

    public static IEnumerator Flicker(GameObject[] gameObjects, float duration)
    {
        foreach (GameObject o in gameObjects)
        {
            o.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        foreach (GameObject o in gameObjects)
        {
            o.SetActive(false);
        }
        yield break;
    }

    public static string ConvertJavaTime(int javaTime)
    {
        long timeTick = ((long)javaTime) * 1000;
        long ticks_1970 = date_1970.Ticks;
        long time_ticks = ticks_1970 + timeTick * 10000;
        DateTime dt = new DateTime(time_ticks);
        dt = dt.ToLocalTime();
        return string.Format("{0:yyyy/MM/dd HH:mm}", dt);
    }

    public static long GetTicksFromJavaTime(long javaTimeInMilliseconds)
    {
        long timeTick = javaTimeInMilliseconds;
        long ticks_1970 = date_1970.Ticks;
        long time_ticks = ticks_1970 + timeTick * 10000;
        return time_ticks;
    }

    public static void ResetEffect(GameObject go)
    {
        ParticleSystem particleSystem = go.GetComponent<ParticleSystem>();
        if (particleSystem)
        {
            particleSystem.time = 0;
            particleSystem.Clear(true);
            particleSystem.Stop(true);
        }
        Component[] components = go.GetComponentsInChildren(typeof(ParticleSystem), true);
        foreach (Component c in components)
        {
            ParticleSystem ps = c as ParticleSystem;
            ps.Clear(true);
            ps.time = 0;
            ps.Stop(true);
        }

        go.SetActive(false);
    }

    public static IEnumerator FlickEffect(GameObject[] gameObjects, float duration)
    {
        foreach (GameObject o in gameObjects)
        {
            o.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        foreach (GameObject o in gameObjects)
        {
            ResetEffect(o);
            o.SetActive(false);
        }
        yield break;
    }

    public static IEnumerator FlickGameObject(GameObject[] gameObjects, float duration)
    {
        foreach (GameObject o in gameObjects)
        {
            o.SetActive(true);
        }
        yield return new WaitForSeconds(duration);
        foreach (GameObject o in gameObjects)
        {
            o.SetActive(false);
        }
        yield break;
    }

    public static void Flip(GameObject go, bool keepXPositive, bool keepYPositive, bool keepZPositive)
    {
        Vector3 scale = go.transform.localScale;
        if (keepXPositive == (scale.x < 0))
            scale.x = -scale.x;
        if (keepYPositive == (scale.y < 0))
            scale.y = -scale.y;
        if (keepZPositive == (scale.z < 0))
            scale.z = -scale.z;
        go.transform.localScale = scale;
    }

    public static void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public static void EnableComponent<T>(GameObject go, bool enabled) where T : Behaviour
    {
        if (!go)
            return;
        T t = go.GetComponent<T>();
        if (t)
            t.enabled = enabled;
    }


    public static IEnumerator HidePopupWindow(GameObject go)
    {
        Vector3 oriScale = go.transform.localScale;
        //        TweenScale.Begin(go, 0.13f, oriScale * 1.17f);
        yield return new WaitForSeconds(0.13f);
        //        TweenScale.Begin(go, 0.13f, oriScale * 0.5f);
        yield return new WaitForSeconds(0.13f);
        go.transform.localScale = oriScale;
        go.SetActive(false);
    }

    public static IEnumerator ShowPopupWindow(GameObject go)
    {
        Vector3 oriScale = go.transform.localScale;
        go.transform.localScale = oriScale * 0.5f;
        go.SetActive(true);
        yield return new WaitForEndOfFrame();
        //        TweenScale.Begin(go, 0.182f, oriScale * 1.17f);
        yield return new WaitForSeconds(0.182f);
        //        TweenScale.Begin(go, 0.13f, oriScale);
        yield return new WaitForSeconds(0.13f);
        go.transform.localScale = oriScale;
    }

    public static IEnumerator NumberChange(int current, int target, int times, GameObject label, float duration)
    {
        int step = (target - current) / times;
        for (int i = 0; i < times + 1; i++)
        {
            current += step;
            if (step < 0 && current < target)
                current = target;
            if (step > 0 && current > target)
                current = target;
            //            View.SetLabelText(label, current.ToString());
            float dura = duration - Time.deltaTime;
            if (dura < 0)
                dura = 0;
            yield return new WaitForSeconds(dura);
        }

        if (current != target)
        {
            //            View.SetLabelText(label, target.ToString());
            yield break;
        }
    }

    public static void NormalizeInWorld(GameObject go)
    {
        go.transform.position = Vector3.zero;
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
    }

    public static void NormalizeInLocal(GameObject go)
    {
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
        go.transform.localScale = Vector3.one;
    }

    public static void PauseEffect(GameObject effect, bool pause)
    {
        ParticleSystem[] pss = effect.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem particleSystem in pss)
        {
            if (pause)
                particleSystem.Pause(true);
            else
                particleSystem.Play(true);
        }
    }

    public static string GetIconImagePath(string name)
    {
        return "res/platform/icon/" + name + ".ab";
    }

    /// <summary>
    /// 根据平台获取路径
    /// </summary>
    /// <param name="relPath"></param>
    /// <returns></returns>
    public static string GetStreamingAssetPathForWWW(string relPath)
    {
        string path = "";
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:

            case RuntimePlatform.WindowsPlayer:
                path = "file:///" + Application.dataPath + "/StreamingAssets/" + relPath;
                break;
            case RuntimePlatform.Android:
                path = Application.streamingAssetsPath + "/" + relPath;
                break;
            case RuntimePlatform.OSXEditor:
                path = "file://" + Application.dataPath + "/StreamingAssets/" + relPath;
                break;
            case RuntimePlatform.IPhonePlayer:
                path = "file://" + Application.streamingAssetsPath + "/" + relPath;
                break;
        }

        return path;
    }

    public static string GetPersistentDataPathForWWW(string relPath)
    {
        string path = GetPersistentDataPath(relPath);
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                path = "file:///" + path;
                break;

            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                path = "file://" + path;
                break;
        }

        return path;
    }

    public static string GetPersistentDataPath(string relPath)
    {
        return mPersistentDataPath + "/" + relPath;
    }

    public static string GetFilePathForWWW(string relativePath)
    {
        string path = GetPersistentDataPath(relativePath);
        if (File.Exists(path))
        {
            return path;
        }
        else
        {
            return GetStreamingAssetPathForWWW(relativePath);
        }
    }

    public static string GetClockTime(int milliseconds)
    {
        int seconds = milliseconds / 1000;
        int hour = seconds / 3600;
        int minute = (seconds - hour * 3600) / 60;
        int second = seconds % 60;
        return (hour < 10 ? "0" : "") + hour + ":" + (minute < 10 ? "0" : "") + minute + ":" +
               (second < 10 ? "0" : "") + second;
    }

    public static void ToggleOn(GameObject[] goes, int index)
    {
        for (var i = 0; i < goes.Length; i++)
            goes[i].SetActive(i == index);
    }

    public static void ToggleOff(GameObject[] goes, int index)
    {
        for (var i = 0; i < goes.Length; i++)
            goes[i].SetActive(i != index);
    }

    public static void ActiveN(GameObject[] goes, int n)
    {
        for (var i = 0; i < goes.Length; i++)
            goes[i].SetActive(i < n);
    }
    public static void JumpAim(Transform obj, Vector3 endValue, float jumpPower, int numJumps, float duration, bool snapping = false)
    {
        obj.DOLocalJump(endValue, jumpPower, numJumps, duration, snapping);
    }

    public static void PrinteSystemInfo()
    {
        Debug.LogError("OperatingSystem:" + SystemInfo.operatingSystem);
        Debug.LogError("SystemMemorySize:" + SystemInfo.systemMemorySize);
        Debug.LogError("ProcessorCount:" + SystemInfo.processorCount);
        Debug.LogError("ProcessorType:" + SystemInfo.processorType);
#if UNITY_IPHONE
        Debug.LogError("IPhone Generation:" + SystemInfo.supportsShadows);
#endif
    }

    public static string GetAndroidId()
    {

#if UNITY_ANDROID 
        try
        {
            AndroidJavaClass clsUnity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject objActivity = clsUnity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject objResolver = objActivity.Call<AndroidJavaObject>("getContentResolver");
            AndroidJavaClass clsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
            return clsSecure.CallStatic<string>("getString", objResolver, "android_id");
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }
#endif
        return "";
    }

    public static T[] RandomSort<T>(T[] array, int bout = 2)
    {
        for (int i = 0; i < bout; i++)
        {
            for (int j = 0; j < array.Length; j++)
            {
                T temp = array[i];
                int ran = Random(0, array.Length);
                array[i] = array[ran];
                array[ran] = temp;
            }
        }
        return array;
    }


    public static bool IsHoverUI()
    {
#if IPHONE || ANDROID
			 if (Input.touchCount > 0)
            {
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //该方法可以判断触摸点是否在UI上
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                  return true;
                else
                  return false;
            }
        }
#else 
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            // Debug.Log("ui");
            return true;
        }

        else
        {
            //  Debug.Log("scene");
            return false;
        }

#endif

    }



    public static Quaternion[] GetSectorPos(float radius, float angleDegree, int segments)
    {
        if (segments == 0)
        {
            segments = 1;
#if UNITY_EDITOR
            Debug.Log("segments must be larger than zero.");
#endif
        }
        float startAngleDegree = (180 - angleDegree) / 2;
        segments -= 1;
        Quaternion[] qus = new Quaternion[segments + 2];

        float angle = Mathf.Deg2Rad * angleDegree;
        float startAngle = Mathf.Deg2Rad * startAngleDegree;

        float currAngle = angle + startAngle; //第一个三角形的起始角度
        float deltaAngle = angle / segments; //根据分段数算出每个三角形在圆心的角的角度

        for (int i = 1; i < qus.Length; i++)
        {
            //圆上一点的公式: x = r*cos(angle), y = r*sin(angle)
            //根据半径和角度算出弧度上的点的位置

            float x = Mathf.Cos(currAngle);
            float y = Mathf.Sin(currAngle);

            //这里为了我的需求改到了把点算到了(x,y,0), 如果需要其他平面, 可以改成(x,0,y)或者(0,x,y)
            currAngle -= deltaAngle;
            qus[i] = new Quaternion(x * radius, y * radius, 0, (angle_360(new Vector3(x, y, 0), Vector3.zero) - 90));
        }

        return qus;
    }


    private static float angle_360(Vector3 from_, Vector3 to_)
    {
        //两点的x、y值
        float x = from_.x - to_.x;
        float y = from_.y - to_.y;

        //斜边长度
        float hypotenuse = Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));

        //求出弧度
        float cos = x / hypotenuse;
        float radian = Mathf.Acos(cos);

        //用弧度算出角度    
        float angle = 180 / (Mathf.PI / radian);

        if (y < 0)
        {
            angle = -angle;
        }
        else if ((y == 0) && (x < 0))
        {
            angle = 180;
        }
        return angle;
    }

    public static void StartCountDown(float time,VoidDelegate callback=null)
    {
        StartConroutine(CountDown(time,callback));
    }

    static   IEnumerator CountDown(float timer,VoidDelegate callback=null)
    {
            yield return new WaitForSeconds(timer);

            if (callback != null)
                callback();
    }

}
