using UnityEngine;
using System.Collections;
using System.Collections.Generic;

    /// <summary>
    /// 预制体加载类
    /// </summary>
    public class ResourceLoader : BaseClass<ResourceLoader>
    {
        #region Load

        public Object Load(string filePath)
        {
            return Resources.Load(filePath);
        }

        public Object[] LoadAll(string filePath)
        {
            return Resources.LoadAll(filePath);
        }

        public T Load<T>(string filePath) where T : Object
        {
            return Resources.Load<T>(filePath);
        }

        public Dictionary<string, T> LoadAll<T>(string filePath) where T : Object
        {
            Dictionary<string, T> result = new Dictionary<string, T>();

            T[] ts = Resources.LoadAll<T>(filePath);

            if (ts.Length == 0)
            {
                Debug.LogError("资源加载失败！路径为'Resources/" + filePath + "'！");
                return null;
            }

            for (int i = 0; i < ts.Length; i++)
            {
                result.Add(ts[i].name, ts[i]);
            }

            return result;
        }

        #endregion

        #region API

        /// <summary>
        /// 同步加载并实例化一个预制体
        /// </summary>
        /// <returns>预制体对应的GameObject</returns>
        /// <param name="filePath">预制体存储目录</param>
        /// <param name="name">GameObject名称</param>
        public GameObject InstantiatePrefab(string filePath, string name)
        {
            Object prefab = Load("Prefabs/" + filePath);

            if (null == prefab)
            {
                Debug.LogError("预制体文件未找到，路径为：'Prefabs/" + filePath + "'！");
                return null;
            }

            GameObject newObj = Object.Instantiate(prefab) as GameObject;
            newObj.name = name;

            return newObj;
        }

        /// <summary>
        /// 同步加载并实例化一个预制体，预制体会被初始化
        /// </summary>
        /// <returns>预制体对应的GameObject</returns>
        /// <param name="filePath">预制体存储目录</param>
        /// <param name="name">GameObject名称</param>
        /// <param name="parent">GameObject父物体</param>
        /// <param name="identity">GameObject是否进行一致化</param>
        /// <param name="isUIPanel">是否为UIPanel预制体</param>
        public GameObject InstantiateIdentityPrefab(string filePath, string name, Transform parent, bool isUIPanel = false)
        {
            GameObject newObj = InstantiatePrefab(filePath, name);

            if (newObj == null)
            {
                return null;
            }

            Transform newTrans = newObj.transform;

            newTrans.SetParent(parent);

            newTrans.SetAsLastSibling();

            newTrans.localPosition = Vector3.zero;
            newTrans.localRotation = Quaternion.identity;
            newTrans.localScale = Vector3.one;

            if (isUIPanel)
            {
                RectTransform rect = newTrans.GetComponent<RectTransform>();
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }

            return newObj;
        }

        #endregion


        #region GC

        private bool _GCing = false;

        public void CallGC()
        {
            if (_GCing)
            {
                return;
            }

            Coroutiner.Start(_ResourcesGC());
        }

        private IEnumerator _ResourcesGC()
        {
            _GCing = true;

            yield return null;
            yield return Resources.UnloadUnusedAssets();

            _GCing = false;
        }

        #endregion

    }

