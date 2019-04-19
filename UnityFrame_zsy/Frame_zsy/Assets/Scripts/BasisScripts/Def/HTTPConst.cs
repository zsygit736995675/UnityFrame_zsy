
public class HTTPConst
{
    #region 应用服务器接口
    public const string getRoleAllInfo = "role/getRoleAllInfo";  //获取玩家信息

    public const string getUserUnLock = "unlock/getUserUnLock"; //获取解锁
    public const string addUserUnLock = "unlock/addUserUnLock"; //解锁{"roleId":1,"uId":19,"type":7,"extendId":1}

    public const string getRoleClothesState = "unlock/getClothsByUser";//获取当前角色服装状态
    public const string setRoleClothesState = "unlock/addUserCloths";//设置当前角色服装状态

    public const string reward = "star/reward"; //{"roleId":19,"uId":19,"starNum":577,"getType":1}
    public const string getUserStarNum = "star/getUserStarNum";

    public const string getRoleInfo = "role/getRoleInfo";
    public const string getUserEggNum = "user/egg/getUserEggNum";  // Post 参数为UID//{"code": "0000","msg": "用户扭蛋券为","data": "OQ=="}

    public const string getActivateState = "user/activity/getActivityInfo";//查询活动信息
    public const string getActivateList = "user/activity/getAllActivity";//查询活动列表
    public const string exchangeFruits = "user/activity/exchangeFruits";//兑换码
    public const string exchangeEgg = "user/activity/exchangeEgg";//鲜果兑换活动扭蛋卷
    public const string getActivityEgg = "user/activity/getActivityEgg";//生日会扭蛋卷查询
    
    #endregion 应用服务器接口


    #region 游戏服务器接口
    #region 游戏一期内容接口
    // 获取游戏配置版本号信息
    public const string getConfigVersion = "init/config/version"; // GET请求
    // 获取游戏配置
    public const string getConfigInfo = "init/config"; // GET请求
    // 角色偶像初始化
    public const string initGameRole = "init/idol/{0}";  //roleID  json roleName 返回gameRoleID  POST请求

    // 更新玩家星能
    public const string updateGameStarNum = "init/user/{0}/update"; //UID JSON starCount 星能  POST请求
    
    // 获取仓库花束数据
    public const string getFlower = "user/{0}/flower"; //GameUID  GET请求
    // 获取仓库种子数据
    public const string getSeed = "user/{0}/seed"; // GameUID  GET请求
    // 获取仓库全部数据
    public const string getWarehouse = "user/{0}/warehouse"; // GameUID  GET请求
    // 获取玩家花田信息
    public const string getUserGardenInfo = "user/{0}/garden";    //GameUID  GET请求    // 初始化玩家数据
    public const string initGameUser = "init/user/{0}"; //UID  JSON starCount  username 返回ganmeUID  POST请求
    // 获取服务器时间戳
    public const string getServerTimestamp = "server/timestamp?{0}"; // 客户端当前timestamp  GET请求
    
    // 兑换金币 uId star
    public const string exchangeCoin = "action/coin/exchange"; // JSON  GameUID 星能值  POST请求
    // 购买花种 uId fId fCount unit固定值10
    public const string shopBuyFlower = "action/flower/buy"; // JSON  GameUID 花ID 花数量 固定值10  POST请求
    // 种植花束 uId uGId fId fCount
    public const string plantFlower = "action/flower/plant"; // JSON  GameUID 玩家花园ID 花ID 花数量  POST请求
    // 收获花束 uId uGId uGPId
    public const string harvestFlower = "action/flower/harvest"; // JSON  GameUID 玩家花园ID 花园种植ID  POST请求
    // 赠送花束 iId uId fId fCount
    public const string sendFlower = "action/flower/send";  //JSON  GameRoleId GameUID 花ID 花数量  POST请求
    // 出售花束/种子 uId fId fType fCount
    public const string shopSellFlower = "action/flower/sell"; // JSON  GameUID 花ID 花类型[101]花束[102]种子 花数量  POST请求
    // 解锁花园 uId gId
    public const string unlockGarden = "action/garden/unlock"; // JSON  GameUID 花园基础ID  POST请求
    // 升级花园 uId gId uGId
    public const string upgradeGarden = "action/garden/upgrade"; // JSON  GameUID 花园基础ID 玩家花园ID  POST请求
    // 花架扩容 uId
    public const string expandBag = "action/warehouse/expand"; // JSON GameUID  POST请求
    #endregion 游戏一期内容接口


    #region 游戏二期内容接口
    // 获取玩家新手指引进度 
    public const string getGuideStep = "user/{0}/guide"; // JSON GameUID GET请求
    // 获取抢花随机玩家列表
    public const string getRobRandom = "user/{0}/rob/random";  //GameUID GET请求
    // 获取仇人列表第一页
    public const string getRobRevenge = "user/{0}/rob/revenge";  //GameUID GET请求
    
    // 枪花 uId rUId rType
    public const string robFlower = "action/flower/rob";  // JSON GameUID 被抢玩家GameUID 抢花类型[100400]随机[100410]复仇  POST请求
    // 更新玩家新手引导进度 gStep 
    public const string updateGuideStep = "user/{0}/guide";  // JSON GameUID 当前引导步骤ID字符串,格式例如：1001,2001,3001  POST请求
    // 快速成熟  uId
    public const string quickHarvest = "action/guide/harvest";  // JSON GameUID  POST请求
    // 虚拟扭蛋/实物扭蛋抽奖 uId pType
    public const string prizePlay = "action/prize/play"; // JSON  GameUID pType[400]虚拟[500]实物  POST请求
    // 更新实物抽奖记录地址
    public const string prizePhone = "user/{0}/prize"; // JSON GameUID  userPrizeId 玩家奖品记录ID phone手机号  POST请求
    //扭蛋实物确认更换信息
    public const string changeEnter = "/app/{0}/prize/{1}/decompn";  //Json  appId   userPrizeId
    #endregion 游戏二期内容接口


    #region 游戏三期内容接口
    // 获取玩家订单
    public const string getUserOrder = "/user/{0}/order"; // GameUID GET请求
    // 初始化玩家订单
    public const string initUserOrder = "/init/user/{0}/order"; // JSON GameUID POST请求
    // 配送订单 uId uOId
    public const string doneUserOrder = "/action/order/done"; // JSON GameUID 玩家订单ID  POST请求
    // 放弃订单 uId uOId
    public const string giveUpUserOrder = "/action/order/giveup"; // JSON GameUID 玩家订单ID  POST请求
    // 使用道具 uId uGId uGPId pCode[20101太阳20102化肥]
    public const string useGameProp = "/action/prop/use"; // JSON GameUID 玩家花园ID 花园种植ID 道具CODE POST请求
    #endregion 游戏三期内容接口


    #region 游戏四期内容接口
    //获取扭蛋主题列表  
    public const string getCapsuleList = "/init/config/prize/theme"; //GET请求 
    //获取所有扭蛋奖励
    public const string getAllCapsuleReward = "/init/config/prize";    //GET请求 

    #endregion 游戏四期内容接口


    #endregion 游戏服务器接口

}

