using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// uiitem 基类
/// 19.3.15 by zsy
/// </summary>
public class BaseUiItem {

    protected GameObject entity;
    protected Transform parent;

    protected int id;
    protected int index;

    public int ID {get { return id; }}
    public int Index { get { return index; } }
    public GameObject Entity { set { entity = value; } get { return entity; } }
    public Transform Parent { get { return parent; } }


    /// <summary>
    /// 初始化ui
    /// </summary>
    public virtual void InitUI()
    {

    }

    /// <summary>
    ///  设置
    /// </summary>
    public virtual void SetData(int id,Transform parent,int index,string name)
    {
        this.id = id;
        this.index = index;

        if (name != null)
        {
            GameObject obj= Resources.Load<GameObject>("UI/Item/"+name);
            entity = GameObject.Instantiate<GameObject>(obj);
        }
        if (parent != null&&entity!=null)
        {
            entity.transform.SetParent(parent);
            entity.transform.localPosition =  Vector3.zero;
            entity.transform.localScale = Vector3.one;
            RectTransform rect = entity.GetComponent<RectTransform>();
        }
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void Destory()
    {
        GameObject.Destroy(this.entity);
    }

    /// <summary>
    /// 查找子物体（递归查找）
    /// </summary> 
    public  T FindChild<T>(string goName, Transform parent = null) where T : UnityEngine.Object
    {
        T go = null;
        if (entity == null || string.IsNullOrEmpty(goName))
        {
            return null;
        }

        if (parent != null)
        {
           
        }
        else
        {
           
        }
        return go;
    }


}
