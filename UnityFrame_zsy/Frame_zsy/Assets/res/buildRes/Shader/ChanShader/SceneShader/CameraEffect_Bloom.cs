using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraEffect_Bloom : MonoBehaviour {

	public Shader bloomShader;//当前应用的shader
	[Range(0.0f,1.5f)]
	public float threshold = 0.25f;//采样阀值
	[HideInInspector]
	public bool normalizeThreshold = false;
	[Range(0.0f,2.5f)]
	public float intensity = 1.0f;//强度
	[Range(0.25f,5.5f)]
	public float blurSize = 1.0f;//模糊像素
	[Range(1,4)]
	public int blurIterations = 2;//模糊次数

	private Material curMaterial;//当前应用的材质

	public Material material
	{
		get{ 
			if (curMaterial == null) {
				curMaterial = new Material (bloomShader);
				curMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return curMaterial;
		}
	}

	// Use this for initialization
	//开始时检查
	//如果计算机不支持图像处理
	//或者无curshader或者当前shader在终端用户的图形卡上不能用
	//就取消这个脚本
	void Start () {

		bloomShader = Shader.Find ("Hidden/Game_CameraEffect_Bloom");

		if (!SystemInfo.supportsImageEffects) {
			enabled = false;
			return;
		}
		if (!bloomShader && !bloomShader.isSupported) {
			enabled = false;
		}
	}
	
	//Graphics.Blit()(拷贝圆纹理到目标渲染纹理)
	void OnRenderImage(RenderTexture source,RenderTexture destination)
	{
		if (bloomShader != null) {

			int rtW = source.width / 4;
			int rtH = source.height / 4;
			RenderTexture rt = RenderTexture.GetTemporary (rtW, rtH, 0,source.format);
			rt.filterMode = FilterMode.Bilinear;

			if (normalizeThreshold == true) {
				material.SetVector ("_ThresholdParams", new Vector2 (1.0f / (1.0f - threshold), -threshold * (1.0f / (1.0f - threshold))));
			} else {
				material.SetVector ("_ThresholdParams", new Vector2 (1.0f, -threshold));
			}

			material.SetFloat ("_Spread", blurSize);
			material.SetFloat ("_BloomIntensity", intensity);
			Graphics.Blit (source, rt, material,0);

			//downscale
			for (int i = 0; i < blurIterations-1; i++) {
				RenderTexture rt2 = RenderTexture.GetTemporary (rt.width / 2, rt.height / 2, 0, source.format);
				rt2.filterMode = FilterMode.Bilinear;

				material.SetFloat ("_Spread",blurSize);
				Graphics.Blit (rt, rt2, material, 1);
				RenderTexture.ReleaseTemporary (rt);
				rt = rt2;
			}
			//upscale
			for (int i = 0; i < blurIterations -1; i++) {
				RenderTexture rt2 = RenderTexture.GetTemporary (rt.width * 2, rt.height * 2, 0, source.format);
				rt2.filterMode = FilterMode.Bilinear;

				material.SetFloat("_Spread", blurSize);
				Graphics.Blit(rt, rt2, material, 1);
				RenderTexture.ReleaseTemporary(rt);
				rt = rt2;
			}

			material.SetTexture ("_BloomTex", rt);
			Graphics.Blit (source, destination, material, 3);

			RenderTexture.ReleaseTemporary (rt);
		} else {
			Graphics.Blit (source, destination);
		}
	}

	//当前脚本不启用的时候
	void OnDisable()
	{
		if (curMaterial != null) {
			DestroyImmediate (curMaterial);//如果当前材质不为null,删除当前材质
		}
	}
}
