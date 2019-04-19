using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ui脚本模板
/// </summary>
public class UIInfo
{
    private string file1;
    private string body1;
    private string body2;

    public string File1
    {
        get { return file1; }
        set { file1 = value; }
    }

    public string Body1
    {
        get { return body1; }
        set { body1 = value; }
    }

    public string Body2
    {
        get { return body2; }
        set { body2 = value; }
    }
  
    /// <param name="name">属性名称</param>
    /// <param name="contrastName">属性类型</param>
    /// <param name="path">子物体路径，因为直接通过路径查找耗时比较多，改为直接父物体查找</param>
    /// <param name="parentName">父物体</param>
    public UIInfo(string name, string contrastName, string path,string parentName)
    {
        file1 = string.Format("private {0} {1} = null", contrastName, name);
        body1 = string.Format("{0} = FindChild<{1}>({2}.transform,\"{3}\")", name, contrastName, parentName, name);
        body2 = string.Format("{0}=null", name);
    }

}
