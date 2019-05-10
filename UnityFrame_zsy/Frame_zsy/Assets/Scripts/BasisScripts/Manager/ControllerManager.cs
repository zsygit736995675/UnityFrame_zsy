using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

    /// <summary>
    /// 控制器管理
    /// </summary>
    public class ControllerManager : BaseClass<ControllerManager>
    {

        public void RegisterAllController()
        {
           this.register((int)ControllerConst.GameMainUI,new GameController()); 
        }

        private Dictionary<int, BaseController> _modules = new Dictionary<int, BaseController>();

        /**
         * 构造函数
         */
        public ControllerManager()
        {
            this._modules.Clear();
        }

        /**
         * 清空处理
         */
        public void clear()
        {
            this._modules.Clear();
        }

        /**
         * 动态添加的Controller
         * @param key 唯一标识
         * @param manager Manager
         */
        public void register(int key, BaseController control)
        {
            if (this.isExists(key))
                return;
            this._modules.Add(key, control);
        }

        /**
         * 动态移除Controller
         * @param key 唯一标识
         *
         */
        public void unregister(int key)
        {
            if (!this.isExists(key))
                return;

            this._modules.Remove(key);
        }

        /**
         * 是否已经存在Controller
         * @param key 唯一标识
         * @return Boolean
         *
         */
        public bool isExists(int key)
        {
            return this._modules.ContainsKey(key);
        }

        public BaseModel getControllerModel(ControllerConst controllerConst)
        {
            return getControllerModel((int)controllerConst);
        }

        /**
         * 获取指定Controller的Model对象
         * @param controllerD Controller唯一标识
         * @returns {BaseModel}
         */
        public BaseModel getControllerModel(int controllerD)
        {
            BaseController manager = this._modules[controllerD];
            if (manager != null)
            {
                return manager.getModel();
            }
            return null;
        }

        public BaseController getController(ControllerConst controllerConst)
        {
            return getController((int)controllerConst);
        }

        /**
         * 获取指定Controller对象
         * @param controllerD Controller唯一标识
         * @returns {BaseModel}
         */
        public BaseController getController(int controllerD)
        {
            BaseController manager = this._modules[controllerD];
            if (manager != null)
            {
                return manager;
            }
            return null;
        }

        /**
         * 加载预加载界面
        */
        private void loadingPreloadUI(Action callback)
        {
            App.ViewManager.onLoadingPreloadUI(callback);
        }
    }

