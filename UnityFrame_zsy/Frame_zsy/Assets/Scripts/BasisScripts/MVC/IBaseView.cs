using System;


    /**
     * 2019/1/2.
     * View基类接口
     */
    public interface IBaseView
    {
        /**
         *   设置置顶
         */
        void setViewTop( );

        /**
         *   打开源
         */
        void setOpenSource(OpenSource source);
        /**
         * View key
        */
        void register(int key);

        /**
         * 是否已经初始化
         * @returns {boolean}
         */
        bool isInit();

        /**
         * 面板是否显示
         * @return
         *
         */
        bool isShow();

        /**
         * 添加到父级
         */
        void addToParent();

        /**
         * 从父级移除
         */
        void removeFromParent();

        /**
         *对面板进行显示初始化，用于子类继承
         *
         */
        void initUI();

        /**
         *对面板数据的初始化，用于子类继承
         *
         */
        void initData();

        /**
         * 面板开启执行函数，用于子类继承
         * @param param 参数
         */
        void open(params object[] args);

        /**
         * 面板关闭执行函数，用于子类继承
         * @param param 参数
         */
        void close(params object[] args);

        /**
         * 销毁
         */
        void destroy();

        /**
         * 设置是否隐藏
         * @param value
         */
        void setVisible(bool value);
    }
