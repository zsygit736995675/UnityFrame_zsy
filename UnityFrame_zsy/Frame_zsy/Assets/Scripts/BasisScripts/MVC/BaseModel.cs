    /**
     * 
     * Model基类
     */
    public class BaseModel
    {
        private BaseController _controller;

        /**
         * 构造函数
         * @param $controller 所属模块
         */
        public BaseModel(BaseController controller)
        {
            this._controller = controller;
            this._controller.setModel(this);
        }


    }
