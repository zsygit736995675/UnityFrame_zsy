using UnityEngine;
using System.Collections.Generic;

namespace ClientCore
{
class ${class_name}
{
    public static void Build() {
        InitSceneMap();
        InitPrefabMap();
        InitTextureMap();
		InitTxtMap();
    }

    static void InitSceneMap() {
        % for key in scene_map.keys():
		FireEngine.FireSystem.ResMgr.RegisterScnBundleIdx("${key}", "${scene_map[key]}");
        % endfor
    }

    static void InitPrefabMap() {
        % for key in prefab_map.keys():
		FireEngine.FireSystem.ResMgr.RegisterAssetBundleIdx("${key}", "${prefab_map[key]}");
        % endfor
    }

    static void InitTextureMap() {
        % for key in texture_map.keys():
		FireEngine.FireSystem.ResMgr.RegisterAssetBundleIdx("${key}", "${texture_map[key]}");
        % endfor
    }
    
	static void InitTxtMap() {
		% for key in txt_map.keys():
		FireEngine.FireSystem.ResMgr.RegisterAssetBundleIdx("${key}", "${txt_map[key]}");
		% endfor
	}
}
}
