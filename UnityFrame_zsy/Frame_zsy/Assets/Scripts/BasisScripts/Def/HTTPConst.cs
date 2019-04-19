
public class HTTPConst
{
    #region Ӧ�÷������ӿ�
    public const string getRoleAllInfo = "role/getRoleAllInfo";  //��ȡ�����Ϣ

    public const string getUserUnLock = "unlock/getUserUnLock"; //��ȡ����
    public const string addUserUnLock = "unlock/addUserUnLock"; //����{"roleId":1,"uId":19,"type":7,"extendId":1}

    public const string getRoleClothesState = "unlock/getClothsByUser";//��ȡ��ǰ��ɫ��װ״̬
    public const string setRoleClothesState = "unlock/addUserCloths";//���õ�ǰ��ɫ��װ״̬

    public const string reward = "star/reward"; //{"roleId":19,"uId":19,"starNum":577,"getType":1}
    public const string getUserStarNum = "star/getUserStarNum";

    public const string getRoleInfo = "role/getRoleInfo";
    public const string getUserEggNum = "user/egg/getUserEggNum";  // Post ����ΪUID//{"code": "0000","msg": "�û�Ť��ȯΪ","data": "OQ=="}

    public const string getActivateState = "user/activity/getActivityInfo";//��ѯ���Ϣ
    public const string getActivateList = "user/activity/getAllActivity";//��ѯ��б�
    public const string exchangeFruits = "user/activity/exchangeFruits";//�һ���
    public const string exchangeEgg = "user/activity/exchangeEgg";//�ʹ��һ��Ť����
    public const string getActivityEgg = "user/activity/getActivityEgg";//���ջ�Ť�����ѯ
    
    #endregion Ӧ�÷������ӿ�


    #region ��Ϸ�������ӿ�
    #region ��Ϸһ�����ݽӿ�
    // ��ȡ��Ϸ���ð汾����Ϣ
    public const string getConfigVersion = "init/config/version"; // GET����
    // ��ȡ��Ϸ����
    public const string getConfigInfo = "init/config"; // GET����
    // ��ɫż���ʼ��
    public const string initGameRole = "init/idol/{0}";  //roleID  json roleName ����gameRoleID  POST����

    // �����������
    public const string updateGameStarNum = "init/user/{0}/update"; //UID JSON starCount ����  POST����
    
    // ��ȡ�ֿ⻨������
    public const string getFlower = "user/{0}/flower"; //GameUID  GET����
    // ��ȡ�ֿ���������
    public const string getSeed = "user/{0}/seed"; // GameUID  GET����
    // ��ȡ�ֿ�ȫ������
    public const string getWarehouse = "user/{0}/warehouse"; // GameUID  GET����
    // ��ȡ��һ�����Ϣ
    public const string getUserGardenInfo = "user/{0}/garden";    //GameUID  GET����    // ��ʼ���������
    public const string initGameUser = "init/user/{0}"; //UID  JSON starCount  username ����ganmeUID  POST����
    // ��ȡ������ʱ���
    public const string getServerTimestamp = "server/timestamp?{0}"; // �ͻ��˵�ǰtimestamp  GET����
    
    // �һ���� uId star
    public const string exchangeCoin = "action/coin/exchange"; // JSON  GameUID ����ֵ  POST����
    // ������ uId fId fCount unit�̶�ֵ10
    public const string shopBuyFlower = "action/flower/buy"; // JSON  GameUID ��ID ������ �̶�ֵ10  POST����
    // ��ֲ���� uId uGId fId fCount
    public const string plantFlower = "action/flower/plant"; // JSON  GameUID ��һ�԰ID ��ID ������  POST����
    // �ջ��� uId uGId uGPId
    public const string harvestFlower = "action/flower/harvest"; // JSON  GameUID ��һ�԰ID ��԰��ֲID  POST����
    // ���ͻ��� iId uId fId fCount
    public const string sendFlower = "action/flower/send";  //JSON  GameRoleId GameUID ��ID ������  POST����
    // ���ۻ���/���� uId fId fType fCount
    public const string shopSellFlower = "action/flower/sell"; // JSON  GameUID ��ID ������[101]����[102]���� ������  POST����
    // ������԰ uId gId
    public const string unlockGarden = "action/garden/unlock"; // JSON  GameUID ��԰����ID  POST����
    // ������԰ uId gId uGId
    public const string upgradeGarden = "action/garden/upgrade"; // JSON  GameUID ��԰����ID ��һ�԰ID  POST����
    // �������� uId
    public const string expandBag = "action/warehouse/expand"; // JSON GameUID  POST����
    #endregion ��Ϸһ�����ݽӿ�


    #region ��Ϸ�������ݽӿ�
    // ��ȡ�������ָ������ 
    public const string getGuideStep = "user/{0}/guide"; // JSON GameUID GET����
    // ��ȡ�����������б�
    public const string getRobRandom = "user/{0}/rob/random";  //GameUID GET����
    // ��ȡ�����б��һҳ
    public const string getRobRevenge = "user/{0}/rob/revenge";  //GameUID GET����
    
    // ǹ�� uId rUId rType
    public const string robFlower = "action/flower/rob";  // JSON GameUID �������GameUID ��������[100400]���[100410]����  POST����
    // ������������������� gStep 
    public const string updateGuideStep = "user/{0}/guide";  // JSON GameUID ��ǰ��������ID�ַ���,��ʽ���磺1001,2001,3001  POST����
    // ���ٳ���  uId
    public const string quickHarvest = "action/guide/harvest";  // JSON GameUID  POST����
    // ����Ť��/ʵ��Ť���齱 uId pType
    public const string prizePlay = "action/prize/play"; // JSON  GameUID pType[400]����[500]ʵ��  POST����
    // ����ʵ��齱��¼��ַ
    public const string prizePhone = "user/{0}/prize"; // JSON GameUID  userPrizeId ��ҽ�Ʒ��¼ID phone�ֻ���  POST����
    //Ť��ʵ��ȷ�ϸ�����Ϣ
    public const string changeEnter = "/app/{0}/prize/{1}/decompn";  //Json  appId   userPrizeId
    #endregion ��Ϸ�������ݽӿ�


    #region ��Ϸ�������ݽӿ�
    // ��ȡ��Ҷ���
    public const string getUserOrder = "/user/{0}/order"; // GameUID GET����
    // ��ʼ����Ҷ���
    public const string initUserOrder = "/init/user/{0}/order"; // JSON GameUID POST����
    // ���Ͷ��� uId uOId
    public const string doneUserOrder = "/action/order/done"; // JSON GameUID ��Ҷ���ID  POST����
    // �������� uId uOId
    public const string giveUpUserOrder = "/action/order/giveup"; // JSON GameUID ��Ҷ���ID  POST����
    // ʹ�õ��� uId uGId uGPId pCode[20101̫��20102����]
    public const string useGameProp = "/action/prop/use"; // JSON GameUID ��һ�԰ID ��԰��ֲID ����CODE POST����
    #endregion ��Ϸ�������ݽӿ�


    #region ��Ϸ�������ݽӿ�
    //��ȡŤ�������б�  
    public const string getCapsuleList = "/init/config/prize/theme"; //GET���� 
    //��ȡ����Ť������
    public const string getAllCapsuleReward = "/init/config/prize";    //GET���� 

    #endregion ��Ϸ�������ݽӿ�


    #endregion ��Ϸ�������ӿ�

}

