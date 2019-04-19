using System;using System.Collections.Generic;using UnityEngine;public class SoundConfig : BaseConfig{    public static bool resLoaded = false;    private static Dictionary<int, SoundConfig> dic = new Dictionary<int, SoundConfig>();    public static List<SoundConfig> array = new List<SoundConfig>();    public static void Init()    {        LoadRes();    }    public readonly int id;    public readonly string name;    public readonly string audioName;    public readonly int type;    public readonly int isLoop;    public readonly int playNum;    public readonly float volume;    public SoundConfig(int id, string name, string audioName, int type, int isLoop, int playNum, float volume)    {        this.id = id;        this.name = name;        this.audioName = audioName;        this.type = type;        this.isLoop = isLoop;        this.playNum = playNum;        this.volume = volume;    }    private static void OnLoadFile( byte[] data)    { 		ReadStream rs = new ReadStream(data);        /*int file_len = */rs.ReadInt();        string flag = rs.ReadString();        if(flag != "SoundConfig")        {            LogWarning("invalid file flag" + flag);            return;        }        int col_cnt = rs.ReadShort();        if(col_cnt != 7)        {            LogWarning("col cnt invalid" + col_cnt + " : " + 7);            return;        }        int row_cnt = rs.ReadInt();        for(int i = 0; i < row_cnt; i++)        {            Add_Item(rs);        }        resLoaded = true;    }    private static void Add_Item(ReadStream rs)    {        int id = rs.ReadInt();        string name = rs.ReadString();        string audioName = rs.ReadString();        int type = rs.ReadInt();        int isLoop = rs.ReadInt();        int playNum = rs.ReadInt();        float volume = rs.ReadFloat();        SoundConfig new_obj_SoundConfig = new SoundConfig(id, name, audioName, type, isLoop, playNum, volume);                if(dic.ContainsKey(id))        {            LogWarning("duplicate key: " + id);            return;        }        dic.Add(id, new_obj_SoundConfig);        array.Add(new_obj_SoundConfig);    }    private static void LoadRes()    {        if(resLoaded) return;        byte[] data = GetAsset("SoundConfig.bytes");		OnLoadFile(data);    }    public static SoundConfig GetConfig( int id )    {    	SoundConfig config;    	if ( dic.TryGetValue(id, out config ) )    	{    		return config;    	}    	else    	{    		return null;    	}    }}