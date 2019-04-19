using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    /// <summary>
    /// 协程
    /// </summary>
    public class Coroutiner : MonoBehaviour
    {
        public delegate void CallbackFunc();
        public static Coroutiner instance = null;

        public static void Init()
        {
        
            UnityEngine.GameObject go = new UnityEngine.GameObject("Coroutiner");
            go.AddComponent<Coroutiner>();
            UnityEngine.GameObject.DontDestroyOnLoad(go);
        }

        private void Awake()
        {
            instance = this;
        }

        static public Coroutine Start(IEnumerator result)
        {
            if(instance == null)
            {
                Init();
            }
            return instance.StartCoroutine(result);
        }

        static public void Stop(IEnumerator result)
        {
            instance.StopCoroutine(result);
        }

        static public void Stop(Coroutine routine)
        {
            instance.StopCoroutine(routine);
        }

        static public void StopAll()
        {
            instance.StopAllCoroutines();
        }

        public void OnApplicationQuit()
        {
            instance = null;
        }
    }
