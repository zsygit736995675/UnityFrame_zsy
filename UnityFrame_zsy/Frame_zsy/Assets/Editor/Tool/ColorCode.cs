using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text.RegularExpressions;
using System;
public class ColorCode :  EditorWindow {
	Color matColor = Color.white;
	Color CToEColor= Color.white;
	
	string key = "";
	string CToEkey;
	int red,green,blue = 0;
	int r,g,b = 0;
	
	private static float divide255	= 1f / 255f;

	public static void init(){
		ColorCode window =	GetWindow<ColorCode>();
		window.Show ();
	}

	void OnGUI ()
	{
		EditorGUILayout.Space();
		DrawCustomProperties();
	}
	protected void DrawCustomProperties ()
	{
		EditorGUILayout.BeginVertical();
		GUILayout.Label("CodingToColor:");
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label("Coding:");
		key = EditorGUILayout.TextField(key, GUILayout.MinWidth(100f));
//		if(key.Length==6)
		matColor = ToClolor(key);

//		if(GUILayout.Button ("Transition",GUILayout.MaxWidth(150f)))
//		{
//			if(string.IsNullOrEmpty (key))
//				matColor = ToClolor(key);
//		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		matColor = EditorGUILayout.ColorField (matColor);
		GUILayout.Label("("+red.ToString ()+","+green.ToString()+","+blue.ToString ()+")");
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		GUILayout.Label("ColorToCoding:");
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal ();
		CToEColor = EditorGUILayout.ColorField (CToEColor);
		CToEkey = ToCoding(CToEColor);
//		if(GUILayout.Button ("Transition",GUILayout.MaxWidth(150f)))
//		{
//			if(CToEColor!=null)
//				CToEkey = ToCoding(CToEColor);
//		}

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ();
		GUILayout.Label("Coding:");
		CToEkey = EditorGUILayout.TextField(CToEkey, GUILayout.MinWidth(100f));
		GUILayout.Label("("+r.ToString ()+","+g.ToString()+","+b.ToString ()+")");
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical();
	}
	/** 将字符串转换成颜色*/
	public Color ToClolor(string color)
	{
		char[]rgb;
		//		color = color.TrimStart ('#');
		color = Regex.Replace (color.ToLower (),"[g-zG-Z]","");
		if(key.Length==6)
		{
			rgb = color.ToCharArray ();
			red = Convert.ToInt32(rgb[0].ToString ()+rgb[1].ToString(),16);
			green = Convert.ToInt32(rgb[2].ToString ()+rgb[3].ToString(),16);
			blue = Convert.ToInt32(rgb[4].ToString ()+rgb[5].ToString(),16);
			return new Color(red * divide255, green * divide255, blue * divide255);
		}
		red =0;
		green =0;
		blue =0;
		return Color.white;
//		switch(color.Length)
//		{
//		case 3:
//			rgb = color.ToCharArray ();
//			red = Convert.ToInt32(rgb[0].ToString ()+rgb[0].ToString(),16);
//			green = Convert.ToInt32(rgb[1].ToString ()+rgb[1].ToString(),16);
//			blue = Convert.ToInt32(rgb[2].ToString ()+rgb[2].ToString(),16);
//			return new Color(red * divide255, green * divide255, blue * divide255);
//		case 6:
//			rgb = color.ToCharArray ();
//			red = Convert.ToInt32(rgb[0].ToString ()+rgb[1].ToString(),16);
//			green = Convert.ToInt32(rgb[2].ToString ()+rgb[3].ToString(),16);
//			blue = Convert.ToInt32(rgb[4].ToString ()+rgb[5].ToString(),16);
//			return new Color(red * divide255, green * divide255, blue * divide255);
//		default:
//			return Color.white;
//		}
	}
	
	public string ToCoding(Color color)
	{
		r = Convert.ToInt32(color.r*255f);
		g = Convert.ToInt32(color.g*255f);
		b = Convert.ToInt32(color.b*255f);
		string sr = "",sg = "",sb = "";
		sr=r.ToString ("X").Length<2? "0"+r.ToString ("X"):r.ToString ("X");
		sg=g.ToString ("X").Length<2? "0"+g.ToString ("X"):g.ToString ("X");
		sb=b.ToString ("X").Length<2? "0"+b.ToString ("X"):b.ToString ("X");
		string s = sr+sg+sb;
		return s.ToLower();
	}
}
