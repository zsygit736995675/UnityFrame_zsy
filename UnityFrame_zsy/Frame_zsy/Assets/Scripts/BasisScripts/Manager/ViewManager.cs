using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


    /**
     *  ui打开方式  0游戏 1原生跳转  
     */
    public enum OpenSource
    {
        game=0,
        source
    }
         
    public class ViewManager : BaseClass<ViewManager>
    {
        /**
         * 已注册的UI
         */
        private Dictionary<int, IBaseView> _views = new Dictionary<int, IBaseView>();
        /**
         * 开启中UI
         */
        private List<int> _opens;
        /**
         * 面板父级
        */
        private Dictionary<int, Canvas> _viewParent = new Dictionary<int, Canvas>();

        /**
         * 需要预加载的界面
        */
        private List<int> _needPreload;
        /**
         * 切换场景需要关闭的界面
        */
        private List<int> _changeSceneClose;
        /**
         * 进入副本需要关闭的界面
        */
        private List<int> _enterFBClose;
        /**
         * 关闭时需要释放资源的界面
        */
        private List<int> _freeResClose;


        /**
          * 游戏背景层
          * @type {BaseSpriteLayer}
          */
        public Canvas Game_Bg;

        ///**
        // * 主游戏层
        // * @type {BaseSpriteLayer}
        // */
        public static Canvas Game_Main;

        /**
         * UI主界面
         * @type {BaseEuiLayer}
         */
        public Canvas UI_Main;
        /**
         * UI弹出框层
         * @type {BaseEuiLayer}
         */
        public Canvas UI_Popup;
        /**
         * UI警告消息层
         * @type {BaseEuiLayer}
         */
        public Canvas UI_Message;
        /**
         * UITips层
         * @type {BaseEuiLayer}
         */
        public Canvas UI_Tips;

        /**
         * 构造函数
         */
        public void Init()
        {
            this._views.Clear();
            this._opens = new List<int>();
            this._needPreload = new List<int>();
            this._enterFBClose = new List<int>();
            this._freeResClose = new List<int>();
            this._changeSceneClose = new List<int>();
        }

        /**
         * 清空处理
         */
        public void clear()
        {
            this.closeAll();
            this._views.Clear();
        }

        public void register(ViewDef key, IBaseView view)
        {
            register((int)key, view);
        }

        /**
         * 面板注册
         * @param key 面板唯一标识
         * @param view 面板
         */
        public void register(int key, IBaseView view)
        {
            if (view == null)
                return;

            if (this._views.ContainsKey(key))
                return;

            view.register(key);
            this._views.Add(key, view);

            this.inintViewParent(key, view);
            //this.initNeedPreloadUI(key);
            //this.initFreeResCloseUI(key);
            this.initEnterFBCloseUI(key);
            this.initChangeSceneCloseUI(key);
        }

        /**
         * 面板解除注册
         * @param key
         */
        public void unregister(int key)
        {
            if (!this._views.ContainsKey(key))
                return;

            this._views.Remove(key);
        }

        /**
         * 销毁一个面板
         * @param key 唯一标识
         * @param newView 新面板
         */
        public void destroy(int key, IBaseView newView = null)
        {
            IBaseView oldView = this.getView(key);
            if (oldView != null)
            {
                this.unregister(key);
                oldView.destroy();
                oldView = null;
            }
            this.register(key, newView);
        }

        public IBaseView open(ViewDef key, params object[] param)
        {
            return open((int)key, param);
        }
        /**
         * 开启面板
         * @param key 面板唯一标识
         * @param param 参数
         *
         */
        public IBaseView open(int key, params object[] param)
        {
            IBaseView view = this.getView(key);
            if (view == null)
            {
                return null;
            }

            if (view.isShow())
            {
                view.open(view, param);
                view.setViewTop();
                return view;
            }

            if (view.isInit())
            {
                view.addToParent();
                view.open(view, param);
            }
            else
            {
                GuiConfig config = GuiConfig.GetConfig(key);
                if (config == null)
                {

                    return null;
                }
                else
                {
                    this.setViewOpen(view, param, key);
                }
            }

            this.openUIPlaySound(key);
            this._opens.Add(key);
            this.setOpenDepth();
            view.setViewTop();
            return view;
        }

        private void setViewOpen(IBaseView view, params object[] param)
        {
            if (!view.isInit())
                view.initUI();
            view.initData();
            view.addToParent();
            view.setVisible(true);
            view.open(param);
        }

        private void setViewLoading(IBaseView view, string[] resources, params object[] param)
        {
            //view.setResources(resources);
            //App.EasyLoading.showLoading();
            //view.loadResource(function() {
            //    view.preloadTrue(true);
            //    this.setViewOpen(view, param);
            //    App.EasyLoading.hideLoading();
            //}.bind(this),null);
        }

        /**
         * 初始化view父级
*/
        private void inintViewParent(int key, IBaseView view)
        {
            Canvas can = null;
            if (this._viewParent.TryGetValue(key, out can))
            {
                if (can != null && view is BaseEuiView)
                {
                    view.addToParent();
                }
            }
            else
            {
                this._viewParent.Add(key, ((view as BaseEuiView).myParent));
                view.addToParent();
            }
        }

        /**
         * 设置正在打开界面的深度
        */
        private void setOpenDepth()
        {
            if (this._opens.Count <= 1)
                return;



            //GuiConfig config = null;
            //GuiConfig configNext = null;

            BaseEuiView view1 = null;
            BaseEuiView view2 = null;

            for (int index = 0; index < this._opens.Count; index++)
            {
                for (int indexNext = index + 1; indexNext < this._opens.Count; indexNext++)
                {
                    //config = GuiConfig.GetConfig(this._opens[index]);
                    //configNext = GuiConfig.GetConfig(this._opens[indexNext]);
                    //if (config != null && configNext != null)
                    //{
                    //    if (config.PosZ > configNext.PosZ)
                    //    {
                    //        view1 = this.getView(this._opens[index]) as BaseEuiView;
                    //        view2 = this.getView(this._opens[indexNext]) as BaseEuiView;
                    //        if (view1 != null && view2 != null)
                    //        {
                    //            var depth1:number = this._viewParent.getChildIndex(view1);
                    //            var depth2:number = this._viewParent.getChildIndex(view2);
                    //            if (depth1 != -1 && depth2 != -1 && depth1 < depth2)
                    //            {
                    //                this._viewParent.swapChildren(view1, view2);
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }

        /**
         * 置顶界面（界面在打开中才可以置顶）
        */
        public void topView(int key)
        {
            IBaseView view = this.getView(key);
            if (view == null)
            {
                Debug.Log("UI_" + key + "不存在");
                return;
            }

            if (!view.isShow())
            {
                Debug.Log("请先打开" + "UI_" + key);
                return;
            }

            bool boolean = view is BaseEuiView;
            if (boolean)
            {
                //int maxDepth = this._viewParent.transform.childCount;
                //this._viewParent.setChildIndex(view as BaseEuiView, maxDepth);
            }
            else
            {
                Debug.Log("view type error");
            }
        }

        public void close(ViewDef viewConst, params object[] param)
        {
            int key = (int)viewConst;
            close(key, param);
        }

        /**
         * 关闭面板
         * @param key 面板唯一标识
         * @param param 参数
         *
         */
        public void close(int key, params object[] param)
        {
            if (!this.isShow(key))
                return;

            IBaseView view = this.getView(key);
            if (view == null)
                return;

            int viewIndex = this._opens.IndexOf(key);
            if (key >= 0)
            {
                this._opens.RemoveRange(viewIndex, 1);
            }

            this.closeUIPlaySound(key);

            //view.removeFromParent();
            view.close(param);
        }

        /**
         * 关闭面板
         * @param view
         * @param param
         */
        public void closeView(IBaseView view, params object[] param)
        {
            foreach (var item in this._views)
            {
                if (item.Value == view)
                {
                    this.close(item.Key, param);
                    return;
                }
            }
        }

        /**
         * 根据唯一标识获取一个UI对象
         * @param key
         * @returns {any}
         */
        public IBaseView getView(int key)
        {
            IBaseView view = null;
            this._views.TryGetValue(key, out view);
            return view;
        }

        /**
         * 关闭所有开启中的UI
         */
        public void closeAll()
        {
            while (this._opens.Count > 0)
            {
                this.close(this._opens[0]);
            }
        }

        /**
         * 当前ui打开数量
         * @returns {number}
         */
        public int currOpenNum()
        {
            return this._opens.Count;
        }

        public bool isShow(ViewDef viewConst)
        {
            return isShow((int)viewConst);
        }

        /**
         * 检测一个UI是否开启中
         * @param key
         * @returns {boolean}
         */
        public bool isShow(int key)
        {
            return this._opens.IndexOf(key) != -1;
        }

        /**
         * 获取皮肤（界面在initUI方法里设置皮肤）
         */
        public string getSkin(int skinId)
        {
            //var config:GuiConfig = GuiConfig.GetConfig(skinId);
            //if (config != null)
            //{
            //    return "resource/skins/" + config.ResName;
            //}
            //else
            //{
            //    console.log("GuiConfig" + skinId + "error");
            //    return;
            //}
            return "";
        }

        /**
         * 角色死亡关闭界面
        */
        public void heroDeathCloseUI()
        {
            //GuiConfig config = null;
            //List<int> closeUI = new List<int>();
            //for (int index = 0; index < this._opens.Count; index++)
            //{
            //    config = GuiConfig.GetConfig(this._opens[index]);
            //    if (config != null && config.EscClose)
            //    {
            //        closeUI.push(this._opens[index]);
            //    }
            //}

            //for (int index = 0; index < closeUI.Count; index++)
            //{
            //    this.close(closeUI[index]);
            //}
        }

        /**
         * 关闭互斥界面
*/
        public void closeExclusionUI(int key)
        {
            //var config:GuiConfig = GuiConfig.GetConfig(key);
            //if (config == null)
            //{
            //    return;
            //}

            //for (int index = 0; index < config.exclusion.length; index++)
            //{
            //    this.close(config.exclusion[index]);
            //}
        }

        /**
         * 初始化切换场景关闭界面数据 
*/
        private void initChangeSceneCloseUI(int key)
        {
            //var config:GuiConfig = GuiConfig.GetConfig(key);
            //if (config != null && config.switchSceneIsClose)
            //{
            //    this._changeSceneClose.push(key);
            //}
        }

        /**
         * 切换场景需要关闭的界面
*/
        public void closeChangeSceneUI()
        {
            if (this._changeSceneClose == null)
                return;

            for (int index = 0; index < this._changeSceneClose.Count; index++)
            {
                this.close(this._changeSceneClose[index]);
            }
        }

        /**
         * 初始化进入副本需要关闭的界面数据
*/
        private void initEnterFBCloseUI(int key)
        {
            //GuiConfig config = GuiConfig.GetConfig(key);
            //if (config != null && config.dungeonHide)
            //{
            //    this._enterFBClose.push(key);
            //}
        }

        /**
         * 进入副本需要关闭的界面
*/
        public void closeEnterFBUI()
        {
            if (this._enterFBClose == null)
                return;

            for (int index = 0; index < this._enterFBClose.Count; index++)
            {
                this.close(this._enterFBClose[index]);
            }
        }


        /**
         * 加载登陆预加载
        */
        public void onLoadingPreloadUI(Action callback)
        {
            //IBaseView view = this.getView(ViewConst.Loading);
            //if (view == null)
            //{
            //    Log.Error("加载界面不存在");
            //    return;
            //}
            //else
            //{
            //    view.setResources(["ui_loading"]);
            //    App.EasyLoading.showLoading();
            //    view.loadResource(function() {
            //        view.setVisible(false);
            //        view.preloadTrue(true);
            //        App.EasyLoading.hideLoading();
            //        if (callback != null)
            //        {
            //            callback();
            //        }
            //    }.bind(this), function() {
            //        view.setVisible(false);
            //    }.bind(this));
            //}
        }

        /**
         * 加载需要预加载的界面(GuiConfig ReadyToLoad == true 需要预加载)
*/
        public void onLoadNeedPreloadUI()
        {
            if (this._needPreload == null)
                return;

            //for (int index = 0; index < this._needPreload.Count; index++)
            //{
            //    GuiConfig config = GuiConfig.GetConfig(this._needPreload[index]);
            //    if (config == null)
            //    {
            //        Debug.Log("GuiConfig " + this._needPreload[index] + " error");
            //        continue;
            //    }
            //    else if (config.readyLoadGroup == null)
            //    {
            //        Debug.Log("GuiConfig " + this._needPreload[index] + "'s readyLoadGroup is null");
            //        return;
            //    }
            //    else if (config.readyLoadGroup.length <= 0)
            //    {
            //        Debug.Log("ui " + this._needPreload[index] + "'s readyLoadGroup is empty");
            //        return;
            //    }

            //    IBaseView view = this.getView(this._needPreload[index]);
            //    if (view == null)
            //    {
            //        Debug.Log("UI_" + this._needPreload[index] + "不存在");
            //        continue;
            //    }
            //    else if (view.isShow())
            //    {
            //        Debug.Log("UI_" + this._needPreload[index] + "在显示中");
            //        continue;
            //    }
            //    else if (view.isInit())
            //    {
            //        Debug.Log("UI_" + this._needPreload[index] + "is already init");
            //        continue;
            //    }
            //    else if (view.isPreload())
            //    {
            //        Debug.Log("UI_" + this._needPreload[index] + "is already preload");
            //        continue;
            //    }
            //    else
            //    {
            //        view.setResources(config.readyLoadGroup);
            //        //view.loadResource(function() {
            //        //    view.setVisible(false);
            //        //    view.preloadTrue(true);
            //        //}.bind(this), function() {
            //        //    view.setVisible(false);
            //        //}.bind(this));
            //    }
            //}
        }


        /**
         * 打开界面时播放音乐
        */
        private void openUIPlaySound(int key)
        {
            //GuiConfig config = GuiConfig.GetConfig(key);
            //if (config != null && config.AudioOfOpen != "")
            //{
            //    App.SoundManager.playEffect(config.AudioOfOpen);
            //}
        }

        /**
         * 关闭界面时播放音乐
*/
        private void closeUIPlaySound(int key)
        {
            //GuiConfig config = GuiConfig.GetConfig(key);
            //if (config != null && config.AudioOfClose != "")
            //{
            //    App.SoundManager.playEffect(config.AudioOfClose);
            //}
        }
    }
