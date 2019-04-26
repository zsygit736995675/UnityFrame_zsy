using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class demo : MonoBehaviour {

    public Image icon;

     void Awake()
    {
#if !UNITY_EDITOR
        this.enabled = false;
        return;
#endif
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 40), "测试按钮"))
        {
            Object[] _atlas=  Resources.LoadAll("uimain");
            Sprite _sprite = SpriteFormAtlas(_atlas, "g_bg_1");
            Debug.LogError(_sprite);

            icon.sprite = _sprite;
            this.enabled = false;
        }
        return;
    }

    //从图集中，并找出sprite
    private Sprite SpriteFormAtlas(Object[] _atlas, string _spriteName)
    {
        for (int i = 0; i < _atlas.Length; i++)
        {
            if (_atlas[i].GetType() == typeof(UnityEngine.Sprite))
            {
                if (_atlas[i].name == _spriteName)
                {
                    return (Sprite)_atlas[i];
                }
            }
        }
        Debug.LogWarning("图片名:" + _spriteName + ";在图集中找不到");
        return null;
    }








}
