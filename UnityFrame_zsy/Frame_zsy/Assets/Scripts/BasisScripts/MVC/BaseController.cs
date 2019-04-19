using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


    /**
     * Controller基类
     */
    public class BaseController : EventHandler
    {

        /**
         * 该模块使用的实体类
         */
        private BaseModel _model;

        /**
         * 构造函数
         */
        public BaseController()
        {

        }

        /*
         * 设置该模块使用的Model对象
         * @param model
         */
        public void setModel(BaseModel model)
        {
            this._model = model;
        }

        /**
         * 获取该模块的Model对象
         * @returns {BaseModel}
         */
        public BaseModel getModel()
        {
            return this._model;
        }

        /**
         * 获取指定Controller的Model对象
         * @param controllerD Controller唯一标识
         * @returns {BaseModel}
         */
        public BaseModel getControllerModel(int controllerD)
        {
            return App.ControllerManager.getControllerModel(controllerD);
        }
    }

