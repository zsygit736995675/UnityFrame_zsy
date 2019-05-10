using UnityEngine;
using UnityEditor;

/// <summary>
/// 修改game视图分辨率
/// </summary>
public class ResolutionChanger : EditorWindow
{
    protected string strSizeWidth = "800";
    protected string strSizeHeight = "600";

   
    public static void Init()
    {
        EditorWindow.GetWindow(typeof(ResolutionChanger));
    }

    static public void SetSize(int pWidth, int pHeight)
    {
        PlayerSettings.defaultScreenWidth = pWidth;
        PlayerSettings.defaultScreenHeight = pHeight;

        //WindowInfos winInfo = new WindowInfos();
        //winInfo.main.position = new Rect(winInfo.main.position.x,0,Screen.currentResolution.width-pWidth-winInfo.main.position.x,Screen.currentResolution.height);

        Rect R = GetMainGameView().position;
        R.width = pWidth;
        R.height = pHeight + 17;
        R.x = Screen.currentResolution.width - pWidth;
        R.y = 38;
        GetMainGameView().position = R;
    }

    void OnGUI()
    {

        GUILayout.Label("iOS", EditorStyles.boldLabel);

        if (GUILayout.Button("iPhone 4S: 960 x 640"))
        {
            SetSize(960, 640);
        }
        if (GUILayout.Button("iPad: 1024 x 768"))
        {
            SetSize(1024, 768);
        }
        if (GUILayout.Button("iphone 5: 1136 x 640"))
        {
            SetSize(1136, 640);
        }
        GUILayout.Label("Android", EditorStyles.boldLabel);

        if (GUILayout.Button("Nexus S: 800 x 480"))
        {
            SetSize(800, 480);
        }
        if (GUILayout.Button("Motorola Milestone 2: 854 x 480"))
        {
            SetSize(854, 480);
        }
        if (GUILayout.Button("AOC Breeze: 800 x 600"))
        {
            SetSize(800, 600);
        }
        if (GUILayout.Button("Samsung Galaxy S III: 1280 x 720"))
        {
            SetSize(1280, 720);
        }
        if (GUILayout.Button("Samsung Galaxy Tab: 1024 x 600"))
        {
            SetSize(1024, 600);
        }
        if (GUILayout.Button("Samsung Galaxy Tab: 1280 x 800"))
        {
            SetSize(1280, 800);
        }
        if (GUILayout.Button("xiaomi 3: 1920 x 1080"))
        {
            SetSize(1920, 1080);
        }
        GUILayout.Label("Custom Size:", EditorStyles.boldLabel);
        strSizeWidth = GUILayout.TextField(strSizeWidth);
        strSizeHeight = GUILayout.TextField(strSizeHeight);

        if (GUILayout.Button("Set Custom Size: " + strSizeWidth + " x " + strSizeHeight))
        {
            SetSize(int.Parse(strSizeWidth), int.Parse(strSizeHeight));
        }
    }

    public static EditorWindow GetMainGameView()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetMainGameView.Invoke(null, null);
        return (EditorWindow)Res;
    }
}