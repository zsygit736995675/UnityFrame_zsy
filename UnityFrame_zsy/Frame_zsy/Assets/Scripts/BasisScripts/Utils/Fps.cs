using UnityEngine;

public class Fps : MonoBehaviour
{
    private float fEllapseTime = 0.0f;
    private float fFps = 0.0f;
    private float fLastRealtime = 0.0f;
    private int m_iLastFrameCount = 0;

    // Update is called once per frame
    void Update()
    {
        if (fLastRealtime == 0.0f)
            fLastRealtime = Time.realtimeSinceStartup;
        float fDeltaTime = Time.realtimeSinceStartup - fLastRealtime;
        fEllapseTime += fDeltaTime;
        if (fEllapseTime > 1)
        {
            fFps = (int)Mathf.CeilToInt((Time.frameCount - m_iLastFrameCount) / fEllapseTime);
            m_iLastFrameCount = Time.frameCount;
            fEllapseTime = 0.0f;
        }
        fLastRealtime = Time.realtimeSinceStartup;
    }

    void OnGUI()
    {
        GUIStyle bb = new GUIStyle();
        bb.normal.textColor = new Color(1, 0, 0, 1);
        bb.richText = true;
        GUI.Label(new Rect(0, Screen.height - 15, 200, 100), fFps.ToString(), bb);
    }
}
