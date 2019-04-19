using System;using System.Collections.Generic;using UnityEngine;public class DialogConfig : BaseConfig{    public static bool resLoaded = false;    private static Dictionary<int, DialogConfig> dic = new Dictionary<int, DialogConfig>();    public static List<DialogConfig> array = new List<DialogConfig>();    public static void Init()    {        LoadRes();    }    public readonly int id;    public readonly string name;    public readonly string Text;    public DialogConfig(int id, string name, string Text)    {        this.id = id;        this.name = name;        this.Text = Text;    }    private static void OnLoadFile( byte[] data)    { 		ReadStream rs = new ReadStream(data);        /*int file_len = */rs.ReadInt();        string flag = rs.ReadString();        if(flag != "DialogConfig")        {            LogWarning("invalid file flag" + flag);            return;        }        int col_cnt = rs.ReadShort();        if(col_cnt != 3)        {            LogWarning("col cnt invalid" + col_cnt + " : " + 3);            return;        }        int row_cnt = rs.ReadInt();        for(int i = 0; i < row_cnt; i++)        {            Add_Item(rs);        }        resLoaded = true;    }    private static void Add_Item(ReadStream rs)    {        int id = rs.ReadInt();        string name = rs.ReadString();        string Text = rs.ReadString();        DialogConfig new_obj_DialogConfig = new DialogConfig(id, name, Text);                if(dic.ContainsKey(id))        {            LogWarning("duplicate key: " + id);            return;        }        dic.Add(id, new_obj_DialogConfig);        array.Add(new_obj_DialogConfig);    }    private static void LoadRes()    {        if(resLoaded) return;        byte[] data = GetAsset("DialogConfig.bytes");		OnLoadFile(data);    }    public static DialogConfig GetConfig( int id )    {    	DialogConfig config;    	if ( dic.TryGetValue(id, out config ) )    	{    		return config;    	}    	else    	{    		return null;    	}    }}