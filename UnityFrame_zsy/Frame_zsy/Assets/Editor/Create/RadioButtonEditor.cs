using UnityEditor;
using System.IO;
using UnityEngine;

public class RadioButtonEditor
{
    static string path = "Assets/Tools/Prefab/RadioButton.prefab";
    [MenuItem("GameObject/UI/RadioButton")]
    static void AddRadioButton()
    {
        ExportPrefab(path);
       //UnityEditor.Selection.activeGameObject.AddComponent<RadioButton>();
    }

    private static void ExportPrefab(string folderPath)
    {
        GameObject go = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        GameObject pre = GameObject.Instantiate(go);
        pre.transform.parent = UnityEditor.Selection.activeGameObject.transform;
        pre.name = "RadioButton";
        pre.transform.localPosition = Vector3.zero;
        pre.transform.localScale = Vector3.one;
    }
}

