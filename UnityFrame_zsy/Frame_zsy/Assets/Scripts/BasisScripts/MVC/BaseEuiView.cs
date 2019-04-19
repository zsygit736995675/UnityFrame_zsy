using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

    public class BaseEuiView : IBaseView
    {
        protected BaseController _controller;
        protected Canvas _myParent;
        protected int _key;
        protected bool _isInit;
        protected bool _isPreload;
        protected string[] _resources = null;

        // btn close
        protected Button mBtnClose = null;
        protected GameObject mFrame; //等于自身  
        protected OpenSource source;//打开源

        /**
         * 构造函数
         * @param $controller 所属模块
         * @param $parent 父级
         */
        public BaseEuiView(BaseController controller, Canvas parent, int id)
        {
            this._controller = controller;
            this._myParent = parent;
            this._isInit = false;
            this._isPreload = false;
            LoadUIByLocal(id);
        }

        public void LoadUIByLocal (int id)
        {
            GuiConfig con = GuiConfig.GetConfig(id);
            GameObject gameObject = Resources.Load<GameObject>("UI/" + con.ResName);
            GameObject obj = GameObject.Instantiate(gameObject);
            this.mFrame = obj;
            this.setVisible(false);
            this.initUI();
        }

        public GameObject Frame
        {
            set { mFrame = value; }
            get { return mFrame; }
        }

        /**
         * 获取我的父级
         * @returns {egret.DisplayObjectContainer}
         */
        public Canvas myParent
        {
            get { return this._myParent; }
        }

        /**
         * 设置初始加载资源
         * @param resources
         */
        public void setResources(string[] resources)
        {
            this._resources = resources;
        }

        /**
         * EUI View key
        */
        public void register(int key)
        {
            this._key = key;
        }

        /**
         * 是否已经初始化
         * @returns {boolean}
         */
        public bool isInit()
        {
            return this._isInit;
        }

        public void setViewTop()
        {
          Transform parent =  this.Frame.transform.parent;
            if (parent != null)
            {
                this.Frame.transform.SetAsLastSibling();
            }
        }

        /// <summary>
        /// 设置打开源
        /// </summary>
        public void setOpenSource(OpenSource source)
        {
            this.source = source;
        }

        /**
         * 面板是否显示
         * @return
         *
         */
        public bool isShow()
        {
            if (this.Frame == null)
                return true;
            return this.Frame.activeSelf;
        }

        /**
         * 添加到父级
         */
        public void addToParent()
        {
            if (_myParent == null || this.Frame == null)
                return;
            Transform transform = ObjUtils.FindChild(_myParent.transform, "Camera");
            //Transform transform = _myParent.transform;
            if (transform == null)
            {
                Debug.Log(" Camera == null");
            }
            this.Frame.transform.SetParent(transform);
            this.Frame.GetComponent<RectTransform>().offsetMax = Vector3.zero;
            this.Frame.GetComponent<RectTransform>().offsetMin = Vector3.zero;
            this.Frame.transform.localScale = Vector3.one;
            this.Frame.transform.localPosition = Vector3.zero;
        }

        /**
         * 从父级移除
         */
        public void removeFromParent()
        {
            if (this.Frame.transform.parent != null)
            {
                this.Frame.transform.SetParent(null);
                GameObject.Destroy(this.Frame);
            }
        }

        /**
         *对面板进行显示初始化，用于子类继承
         *
         */
        public virtual void initUI()
        {
            this._isInit = true;
            mBtnClose = FindChild<Button>("closeBtn");
            if(mBtnClose!=null)
            mBtnClose.onClick.AddListener(OnBackBtnClick);
            initData();
        }

        /**
         *对面板数据的初始化，用于子类继承
         *
         */
        public virtual void initData()
        {
        }

        /**
         * 销毁
         */
        public void destroy()
        {
            this._controller = null;
            this._myParent = null;
            this._resources = null;
            this._isPreload = false;
            this.removeFromParent();
        }

        /**
         * 面板开启执行函数，用于子类继承
         * @param param 参数
         */
        public virtual void open(params object[] args)
        {
            setVisible(true);
            setOpenSource(OpenSource.game);
        }

        /**
         * 面板关闭执行函数，用于子类继承
         * @param param 参数
         */
        public virtual void close(params object[] args)
        {
            setVisible(false);
        }

        /// <summary>
        /// 通用返回键事件 
        /// </summary>
        public virtual void OnBackBtnClick()
        {
            if (App.ViewManager.isShow(_key))
            {
                App.ViewManager.close(_key);
            }
        }

        /**
         * 关闭自身面板
         */
        public void onClose()
        {
            App.ViewManager.close(this._key);
        }

        /**
         * 是否已经预加载资源
         * @returns {boolean}
         */
        public bool isPreload()
        {
            return this._isPreload;
        }

        /**
         * 设置是否隐藏
         * @param value
         */
        public void setVisible(bool value)
        {
            if (this.Frame == null)
            {
                    return;
            }

            if (this.Frame.activeSelf != value)
            {
                this.Frame.SetActive(value);
            }
        }

        /// <summary>
        /// 查找子物体（递归查找）  
        /// </summary> 
        /// <param name="trans">父物体</param>
        /// <param name="goName">子物体的名称</param>
        /// <returns>找到的相应子物体</returns>
        public Transform FindChild(string goName)
        {
            if (mFrame == null || string.IsNullOrEmpty(goName))
                return null;
            Transform go = null;
            go = ObjUtils.FindChild(mFrame.transform, goName);
            return go;
        }

        /// <summary>
        /// 查找子物体（递归查找）  where T : UnityEngine.Object
        /// </summary> 
        /// <param name="trans">父物体</param>
        /// <param name="goName">子物体的名称</param>
        /// <returns>找到的相应子物体</returns>
        public T FindChild<T>(string goName) where T : UnityEngine.Object
        {
            T go = null;
            if (mFrame == null || string.IsNullOrEmpty(goName))
                return null;
            go = ObjUtils.FindChild<T>(mFrame.transform, goName);
            return go;
        }

        /// <summary>
        /// 查找子物体（递归查找）  where T : UnityEngine.Object
        /// </summary> 
        /// <param name="parent">父物体</param>
        /// <param name="goName">子物体的名称</param>
        /// <returns>找到的相应子物体</returns>
        public T FindChild<T>(Transform parent, string goName) where T : UnityEngine.Object
        {
            T go = null;
            if (mFrame == null || string.IsNullOrEmpty(goName))
            {
                return null;
            }
            if (parent != null)
            {
                go = ObjUtils.FindChild<T>(parent, goName);
            }
            else
            {
                go = ObjUtils.FindChild<T>(mFrame.transform, goName);
            }
            return go;
        }

    }
