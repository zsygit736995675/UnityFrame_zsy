using System.Collections;
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public delegate void ABCallback(AssetBundle ab);

public delegate void ObjCallback(Object go);

public class ResMgr : SingletonObject<ResMgr>
{
    private const int AssetBundleCacheSize = 50;//缓存大小

    private readonly LRU<string, ABInfo> mAssetBundles = new LRU<string, ABInfo>(AssetBundleCacheSize);//ab缓存

    private readonly HashSet<string> mLoading = new HashSet<string>();//加载中

    private AssetBundleManifest manifest;//ab清单用于获取依赖


    public void Init()
    {
        //初始化依赖文件
        StartCoroutine(GetAssetBundleManifest());
    }


    private void Awake()
    {
        //缓存溢出处理
        mAssetBundles.onRemoveEntry = OnCacheOverflow;
    }

    /// <summary>
    /// 只通过ab路径加载路径，加载结束放入缓存，callback
    /// </summary>
    public Coroutine LoadAB(string abPath, ABCallback callback = null, bool canAutoUnload = true)
    {
        return   StartCoroutine(_LoadAB(abPath, callback, canAutoUnload));
    }

    /// <summary>
    /// 加载ab，并取出资源，callback传出资源
    /// </summary>
    public Coroutine LoadAssetFromAB<T>(string abPath, string resName, GameObject go, ObjCallback callback = null)
    {
        return StartCoroutine(_LoadAssetFromAB(abPath, resName, go, typeof(T), callback));
    }

    /// <summary>
    /// 加载ab,取出资源并实例化，callback传出
    /// </summary>
    public Coroutine CreateFromAB(string abPath, string resName, ObjCallback callback = null)
    {
        return StartCoroutine(_CreateFromAB(abPath, resName, callback));
    }

    public void CleanRef(GameObject go)
    {
        ABRef abRef = go.GetComponent<ABRef>();
        if (abRef != null)
        {
            Destroy(abRef);
        }
    }

    public void CleanUp()
    {
        StopAllCoroutines();
        mLoading.Clear();
        List<ABInfo> abs = mAssetBundles.GetValues();
        foreach (ABInfo ab in abs)
        {
            if (ab.canUnload)
            {
                ab.ab.Unload(true);
                mAssetBundles.Remove(ab.path);
            }
        }
        Resources.UnloadUnusedAssets();
    }

    public string Dump()
    {
        StringBuilder sb = new StringBuilder();

        List<ABInfo> abs = mAssetBundles.GetValues();
        foreach (ABInfo info in abs)
        {
            sb.Append("{");
            sb.Append(info.path);
            sb.Append(":");
            sb.Append(info.refCount);
            sb.Append("}");
        }

        sb.Append("loading{");
        foreach (string path in mLoading)
        {
            sb.Append(path);
            sb.Append(",");
        }

        sb.Append("}");

        return sb.ToString();
    }

    private IEnumerator _LoadAB(string abPath, ABCallback callback, bool canUnload)
    {
        Logger.Log("_LoadAB:" + abPath);

        if (abPath == null)
          yield  return null;

         abPath = abPath.ToLower();

        if (manifest == null)
        {
            yield return StartCoroutine(GetAssetBundleManifest());
        }

        if (manifest != null)
        {
            string[] allDependencies = GetAssetBundleDependencies(abPath);

            for (int i = 0; i < allDependencies.Length; i++)
            {
                yield return StartCoroutine(_LoadAB(allDependencies[i], null, canUnload));
            }
        }

        ABInfo abInfo;
        if (mAssetBundles.TryGetValue(abPath, out abInfo))
        {
            abInfo.canUnload = abInfo.canUnload && canUnload;

            if (callback != null)
            {
                callback(abInfo.ab);
            }
            yield break;
        }

        if (mLoading.Contains(abPath))
        {
            while (mLoading.Contains(abPath))
            {
                yield return null;
            }

            if (mAssetBundles.TryGetValue(abPath, out abInfo))
            {
                abInfo.canUnload = abInfo.canUnload && canUnload;

                if (callback != null)
                {
                    callback(abInfo.ab);
                }
            }
        }
        else
        {
            string resPath = PathUtils.LoadABPath + abPath.ToLower();

            mLoading.Add(abPath);

            Logger.Log("_LoadAB resPath:" + resPath);
            WWW www = new WWW(resPath);
          
            yield return www;

            mLoading.Remove(abPath);
            if (www!=null&&string.IsNullOrEmpty(www.error))
            {
                www.assetBundle.LoadAllAssets();
                abInfo = new ABInfo();
                abInfo.path = abPath;
                abInfo.canUnload = canUnload;
                abInfo.ab = www.assetBundle;
                mAssetBundles.Set(abPath, abInfo);
              
                if (callback != null)
                {
                    callback(abInfo.ab);
                }
            }
            else
            {
                  Logger.LogError("ResMgr load " + abPath + " failed:" + www.error);
            }
        }
    }

    /// <summary>
    /// 根据依赖递归加载所有依赖资源，再加载主资源   android不能用
    /// </summary>
    private IEnumerator _LoadRecursively(string assetName, ABCallback callback, bool canUnload) {
      
    
        assetName = assetName.ToLower();

        if (manifest == null)
        {
            yield return StartCoroutine(GetAssetBundleManifest());
        }
       
        if (manifest != null) {

            string[] allDependencies = GetAssetBundleDependencies(assetName);

            for (int i = 0; i < allDependencies.Length; i++)
            {
                StartCoroutine(_LoadRecursively(allDependencies[i], null, canUnload));
            }
        }

        //已加载
        ABInfo abInfo;
        if (mAssetBundles.TryGetValue(assetName, out abInfo))
        {
            abInfo.canUnload = abInfo.canUnload && canUnload;
            if (callback != null)
            {
                callback(abInfo.ab);
            }
        }else {
           
            AssetBundle assetBundle = AssetBundle.LoadFromFile(PathUtils.ABPath+ assetName);
 
            if (assetBundle != null) {
                assetBundle.LoadAllAssets();
                abInfo = new ABInfo();
                abInfo.path = assetName;
                abInfo.canUnload = canUnload;
                abInfo.ab = assetBundle;
                mAssetBundles.Set(assetName, abInfo);
                if (callback != null)
                {
                    callback(abInfo.ab);
                }
            }else {
               
            }
        }
    }

    /// <summary>
    ///  加载总ManifestAssetBundle
    /// </summary>
    private IEnumerator GetAssetBundleManifest()
    {
        string manifestUrl =  PathUtils.LoadABPath + "Android";
       // AssetBundle manifestAB = AssetBundle.LoadFromFile(manifestUrl);
       
        Logger.Log("manifestUrl:"+ manifestUrl);

        WWW www = new WWW(manifestUrl);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            AssetBundle ab = www.assetBundle;
            if (ab != null)
            {
                manifest = (AssetBundleManifest)ab.LoadAsset("AssetBundleManifest");
                ab.Unload(false);  // 释放AssetBundle false是否销毁已经加载出来的相关资源
            }
        }
        else
        {
            Logger.LogError("GetAssetBundleManifest error:"+www.error);
        }
    }

    /// <summary>
    /// 获取某个ab包所有的依赖
    /// </summary>
    public string[] GetAssetBundleDependencies(string assetbundleName)
    {
        return manifest.GetAllDependencies(assetbundleName); 
    }

    private IEnumerator _LoadAssetFromAB(string abPath, string resName, GameObject go, System.Type type, ObjCallback callback)
    {
        if (string.IsNullOrEmpty(resName))
        {
            resName = GetResName(abPath);
        }

        //yield return Utils.StartConroutine(_LoadAB(abPath, null, true));
        yield return StartCoroutine(_LoadAB(abPath, null, true));
        ABInfo abInfo;
        if (!mAssetBundles.TryGetValue(abPath, out abInfo))
            yield break;

        UnityEngine.Object asset = abInfo.ab.LoadAsset(resName, type);
        if (asset == null)
            yield break;

        if (go == null)
            yield break;

        ABRef abRef = go.GetComponent<ABRef>();
        if (abRef == null)
        {
            abRef = go.AddComponent<ABRef>();
        }

        abRef.SetABPath(abPath);

        if (callback != null)
        {
            callback(asset);
        }
    }

    private IEnumerator _CreateFromAB(string abPath, string resName, ObjCallback callback)
    {
       
        if (string.IsNullOrEmpty(resName))
        {
            resName = GetResName(abPath);
        }

        yield return StartCoroutine(_LoadAB(abPath,null,true));
         ABInfo abInfo;
        if (!mAssetBundles.TryGetValue(abPath.ToLower(), out abInfo))
            yield break;

        GameObject go = abInfo.ab.LoadAsset(resName, typeof(GameObject)) as GameObject;
        if (go == null)
            yield break;

        go = Instantiate(go) as GameObject;
      
        ABRef abRef = go.AddComponent<ABRef>();
        abRef.SetABPath(abPath);

        if (callback != null)
        {
            callback(go);
        }
    }

    private bool OnCacheOverflow(string abPath, ABInfo abInfo)
    {
        if (!abInfo.canUnload)
            return false;

        if (abInfo.refCount > 0)
            return false;


        abInfo.ab.Unload(true);
        abInfo.ab = null;

#if UNITY_EDITOR
        Debug.Log("[ResMgr]Unload " + abPath);
#endif
        return true;
    }

    private ABInfo FindABInfo(string abPath)
    {
        ABInfo info;
        mAssetBundles.TryGetValue(abPath, out info);
        return info;
    }

    private string GetAssetBundlePathForWWW(string abPath)
    {
        return PathUtils.dataPath + abPath;
    }

    private sealed class ABRef : MonoBehaviour
    {
        [SerializeField]
        private string abPath;

        private bool mInited = false;

        private void Awake()
        {
            if (!mInited && !string.IsNullOrEmpty(abPath))
            {
                string path = abPath;
                abPath = "";
                SetABPath(path);
            }

            mInited = true;
        }

        private void OnDestroy()
        {
            SetABPath(null);
        }

        public void SetABPath(string path)
        {
            mInited = true;

            if (abPath == path)
                return;

            if (!string.IsNullOrEmpty(abPath))
            {
                ABInfo abInfo = ResMgr.Ins.FindABInfo(abPath);
                if (abInfo != null)
                {
                    --abInfo.refCount;
                }
            }

            abPath = path;

            if (!string.IsNullOrEmpty(abPath))
            {
                ABInfo abInfo = ResMgr.Ins.FindABInfo(abPath);
                if (abInfo != null)
                {
                    ++abInfo.refCount;
                }
            }
        }
    }

    private sealed class ABInfo
    {
        public string path;
        public AssetBundle ab = null;
        public bool canUnload = true;
        public int refCount = 0;
    }

    private static string GetResName(string path)
    {
        int begin = path.LastIndexOf('/') + 1;
        int end = path.LastIndexOf('.');
        return path.Substring(begin, end - begin);
    }

    //从图集中，并找出sprite
    public static Sprite SpriteFormAtlas(Object[] _atlas, string _spriteName)
    {
        for (int i = 0; i < _atlas.Length; i++)
        {
            if (_atlas[i].GetType() == typeof(UnityEngine.Sprite))
            {
                if (_atlas[i].name == _spriteName)
                {
                    return (Sprite)_atlas[i];
                }
            }
        }
        Debug.LogWarning("图片名:" + _spriteName + ";在图集中找不到");
        return null;
    }
}


