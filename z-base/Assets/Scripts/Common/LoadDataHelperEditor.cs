#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LoadDataHelper))]
[CanEditMultipleObjects]
public class LoadDataHelperEditor : Editor
{
    [MenuItem("Tools/MyTool/Do It in C#")]
    static void DoIt()
    {
        EditorUtility.DisplayDialog("MyTool", "Do It in C# !", "OK", "");
    }
    public override void OnInspectorGUI()
    {
        LoadDataHelper myscript = (LoadDataHelper)target;

        if (GUILayout.Button("Load All Data"))
        {
            myscript.LoadAllData();
            EditorUtility.SetDirty(myscript);
        }
        base.OnInspectorGUI();
    }
}
#endif