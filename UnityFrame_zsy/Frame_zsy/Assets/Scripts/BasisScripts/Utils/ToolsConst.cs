using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ToolsConst {

    public const string UIPrefabPath = "Assets/StreamingAssets/UI/prefab";
    public const string UI_CODE_PATH = @"Assets/Scripts/view/";
    public const string UI_AB_PATH = @"Assets/StreamingAssets/ui";
    public const string UI_PREFAB_PATH = @"Assets/UI/prefab";
    public const string TMPPREFABPATH= @"Assets/tempUIPrefab/";
    public const  string AudiosOutPutPath = "";
    public const  string AssetBundlesOutPutPath = "Assets/StreamingAssets";
    public const string SceneOutPutPath = "/StreamingAssets/scene";
    public const string UIOutPutPath = "Assets/StreamingAssets/ui";
    public const string RolesOutPutPath = "Assets/StreamingAssets/role";
    public const string EffectOutPutPath = "Assets/StreamingAssets/effect";
    public const string ImagesOutPutPath = "Assets/StreamingAssets/icon";
    public const string FontsOutPutPath = "Assets/StreamingAssets/fonts";

    public const string BundleFile = "/BundleResources";

    //贴图路径
    public static List<string> GetCommonSceneTexturePath() {
        List<string> scenePaths = new List<string>();
        scenePaths.Add(Application.dataPath + BundleFile + "/Texture");
        return scenePaths;
    }

    //场景路径
    public static List<string> GetCommonSceneFilePath() {
        List<string> scenePaths = new List<string>();
        scenePaths.Add(Application.dataPath+ BundleFile+"/Scene");
        return scenePaths;
    } 

    //UI路径
    public static List<string> GetCommonUIFilePath()
    {
        List<string> uiPaths = new List<string>();
        uiPaths.Add(Application.dataPath + BundleFile+"/UI/prefab");
        uiPaths.Add(Application.dataPath + BundleFile+ "/UI/common");
        return uiPaths;
    }

    //Role路径
    public static List<string> GetCommonUIRolePath()
    {
        List<string> uiPaths = new List<string>();
        uiPaths.Add(Application.dataPath + BundleFile + "/Role/prefab");
        return uiPaths;
    }


    //图片路径
    public static List<string> GetCommonImageFilePath()
    {
        List<string> uiPaths = new List<string>();
        string[] names = Directory.GetFiles(Application.dataPath + BundleFile + "/Atalas");
        for (int i = 0; i < names.Length; i++)
        {
            string name = names[i];
            if (name.Contains(".meta"))
            {
             int index=  name.IndexOf(".meta");
              name=  name.Remove(index);
              uiPaths.Add(name);
            }
        }
         // uiPaths.Add(Application.dataPath + BundleFile + "/Atalas/uiback");
        return uiPaths;
    }

    //字体路径
    public static List<string> GetCommonFontFilePath()
    {
        List<string> uiPaths = new List<string>();
        uiPaths.Add(Application.dataPath + BundleFile + "/Fonts");
        //uiPaths.Add(Application.dataPath + "/BaseRes/Fonts");
        return uiPaths;
    }

    //表格路径
    public static List<string> GetCommonConfigFilePath() {
        List<string> configPaths = new List<string>();
        configPaths.Add(Application.dataPath + BundleFile + "/OtherPack/Config");
        return configPaths;
    }

    //音效路径
    public static List<string> GetCommonAudioFilePath()
    {
        //目前没有通用的声音文件，声音文件都是像UI一样统一管理
        List<string> paths = new List<string>();
        paths.Add(Application.dataPath + BundleFile + "/Sound"); 
        return paths;
    }

    //特效路径
    public static List<string> GetCommonEffectFilePath()
    {
        //将客户端的表和共享的表打包
        List<string> uiPaths = new List<string>();
      //  uiPaths.Add(Application.dataPath + BundleFile + "/Effect/effect_zhandou");
      //  uiPaths.Add(Application.dataPath + BundleFile + "/Effect/effect_ui");
        return uiPaths;
    }

    //动画路径
    public static List<string> GetCommonAnimationFilePath()
    {
        //将客户端的表和共享的表打包
        List<string> uiPaths = new List<string>();
        string[] names = Directory.GetFiles(Application.dataPath + BundleFile + "/Animation");
        for (int i = 0; i < names.Length; i++)
        {
            string name = names[i];
            if (name.Contains(".meta"))
            {
                int index = name.IndexOf(".meta");
                name = name.Remove(index);
                uiPaths.Add(name);
            }
        }
      //  uiPaths.Add(Application.dataPath + BundleFile + "/Animation/miner");
        return uiPaths;
    }

    //other路径
    public static List<string> GetCommonOtherFilePath()
    {
        List<string> uiPaths = new List<string>();
        uiPaths.Add(Application.dataPath + BundleFile + "/Tools/prefab");
        return uiPaths;
    }

    
}
