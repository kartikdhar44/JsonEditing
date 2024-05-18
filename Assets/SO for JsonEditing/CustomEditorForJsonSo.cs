using UnityEngine;
using UnityEditor;
using Sh;
using System.Collections.Generic;


[CustomEditor(typeof(ScriptableObjectForJson))]
[CanEditMultipleObjects]
public class CustomEditorForScriptableObj : Editor
{
    
    SerializedProperty _filePath;
    SerializedProperty nodes;
    SerializedProperty _nodeList;
    void OnEnable()
    { 
   
        _filePath = serializedObject.FindProperty("filePath");
        nodes = serializedObject.FindProperty("fileStruct");
        _nodeList = serializedObject.FindProperty("nodeList");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        ScriptableObjectForJson mySo = (ScriptableObjectForJson)target;
       

        EditorGUILayout.PropertyField(_filePath);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Get File"))
        {
            mySo.SetFile();
        }
        GUILayout.EndHorizontal();

      
        if (GUILayout.Button("Open Json File"))
        {
            mySo.OpenJsonFile();
        }
        EditorGUILayout.PropertyField(nodes);
        EditorGUILayout.PropertyField(_nodeList);
        serializedObject.ApplyModifiedProperties();
    }
    void funct(Dictionary<string, object> data, List<Node> nodeList,Node root, int level)
    {
        
    }
}
