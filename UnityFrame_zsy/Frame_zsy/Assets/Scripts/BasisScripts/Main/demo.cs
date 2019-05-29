using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;


public class demo : MonoBehaviour {

    private Button atlasBtn;
    private Button sceneBtn;
    private Button sourceBtn;
    private Button configBtn;
    private Button uiBtn;
    private Button modelBtn;
    private Button eventBtn;
    private Button wwwImageBtn;
    private Button cameraBtn;

    private Text logText;

    private Canvas canvas;

    private StringBuilder logBuilder;

    void Awake()
    {

//#if !UNITY_EDITOR
//        this.enabled = false;
//        return;
//#endif
        canvas = GameObject.FindGameObjectWithTag("demo").GetComponent<Canvas>();
        logText = ObjUtils.FindChild<Text>(canvas.transform, "LogTxt");
        atlasBtn = ObjUtils.FindChild<Button>(canvas.transform, "atlasBtn");
        sceneBtn = ObjUtils.FindChild<Button>(canvas.transform, "sceneBtn");
        sourceBtn = ObjUtils.FindChild<Button>(canvas.transform, "sourceBtn");
        configBtn = ObjUtils.FindChild<Button>(canvas.transform, "configBtn");
        uiBtn = ObjUtils.FindChild<Button>(canvas.transform, "uiBtn");
        modelBtn = ObjUtils.FindChild<Button>(canvas.transform, "modelBtn");
        eventBtn = ObjUtils.FindChild<Button>(canvas.transform, "eventBtn");
        wwwImageBtn = ObjUtils.FindChild<Button>(canvas.transform, "wwwImageBtn");
        cameraBtn = ObjUtils.FindChild<Button>(canvas.transform, "cameraBtn");

        logText.text = "";
        logBuilder = new StringBuilder();

        atlasBtn.onClick.AddListener(AtlasTest);
        sceneBtn.onClick.AddListener(()=> { StartCoroutine(SceneTest()); });
        sourceBtn.onClick.AddListener(SourceTest);  
        configBtn.onClick.AddListener(ConfigTest);
        uiBtn.onClick.AddListener(UITest);
        modelBtn.onClick.AddListener(ModelTest);
        eventBtn.onClick.AddListener(EventTest);


        wwwImageBtn.onClick.AddListener(()=> {
            Image img = wwwImageBtn.GetComponentInChildren<Image>();
            WWWEngine.SetGameAsyncImage(img, "http://u3d-model.oss-cn-beijing.aliyuncs.com/model/Res/Ver3/testimg.jpg");
        });

       
    }

    public void SetLogText(string text)
    {
        logBuilder.Append(text + "\n\n");
    }

    private void Update()
    {
        logText.text = logBuilder.ToString();
    }

    /// <summary>
    /// 加载图集
    /// </summary>
    void AtlasTest()
    {
        //Object[] _atlas=  Resources.LoadAll("uimain");
        //Sprite _sprite = SpriteFormAtlas(_atlas, "g_bg_1");
        //Debug.LogError(_sprite);
        //icon.sprite = _sprite;

        ResMgr.Ins.LoadAB("uimainatlas.unity3d", (AssetBundle ab) =>
        {
            Logger.Log(ab.GetAllAssetNames().Length);
            Logger.Log(ab.GetAllAssetNames()[0]);
            Sprite[] spriteObj = ab.LoadAllAssets<Sprite>();
            Sprite _sprite = ResMgr.SpriteFormAtlas(spriteObj, "song");
            atlasBtn.GetComponentInChildren<Image>().sprite = _sprite;
        });
    }


    /// <summary>
    /// 加载场景
    /// </summary>
    IEnumerator SceneTest()
    {
        WWW www = new WWW(PathUtils.ABPath + "testscene.unity.unity3d");
        yield return www;
        AssetBundle ab = www.assetBundle;
        AsyncOperation asy = SceneManager.LoadSceneAsync("testscene", LoadSceneMode.Single); //sceneName不能加后缀,只是场景名称
        yield return asy;
        www.Dispose();
    }

    /// <summary>
    /// 音效加载
    /// </summary>
    void SourceTest()
    {
        ResMgr.Ins.LoadAB("mainmusic.unity3d", (AssetBundle ab) =>
        {
            AudioSource source = this.GetComponent<AudioSource>();
            //AudioClip[] sous = ab.LoadAllAssets<AudioClip>();
            //source.clip = sous[0];
            AudioClip cli = ab.LoadAsset<AudioClip>("music_field");
            source.clip = cli;
            source.Play();
        });
    }

    /// <summary>
    /// 表格测试
    /// </summary>
    void ConfigTest()
    {
        ResMgr.Ins.LoadAB("exp_bytes.unity3d", (AssetBundle ab) =>
        {
            BaseConfig.isUseLocalTable = App.Ins.UseLocalConfig;
            BaseConfig.mGameConfig = ab;
            ConfFact.Register();
            BaseConfig.mGameConfig = null;
            Logger.Log(StrConfig.GetConfig(960).str);
        }, true);
    }

    /// <summary>
    /// ui测试
    /// </summary>
    void UITest()
    {
        ResMgr.Ins.CreateFromAB("uiprefab.unity3d", "uiMain", (Object o) =>
        {
            GameObject obj = o as GameObject;

            obj.transform.SetParent(canvas.transform);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<RectTransform>().offsetMax = Vector3.zero;
            obj.GetComponent<RectTransform>().offsetMin = Vector3.zero;

            obj.transform.localPosition = Vector3.zero;
        });
    }

    /// <summary>
    /// 模型加载
    /// </summary>
    void ModelTest()
    {
        ResMgr.Ins.CreateFromAB("modelprefab.unity3d", "jiuyue", (Object o) =>
        {
            GameObject obj = o as GameObject;
            obj.transform.localRotation = new Quaternion(0,180,0,0);
            obj.transform.localPosition = new Vector3(0,0,-7);

            Animator ani = obj.GetComponent<Animator>();
            ani.Play("shy");
        });
    }

    /// <summary>
    /// 事件测试 
    /// </summary>
    void EventTest()
    {
       App.EventMgrHelper.PushEvent(EventDef.Test);
    }


   


}
