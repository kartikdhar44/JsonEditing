using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Profiling.Memory.Experimental;
using UnityEngine.WSA;
using System;
using System.Runtime.InteropServices;
using System.Xml;

namespace Sh
{
    [CreateAssetMenu(menuName = "SoForJson")]
    [ExecuteInEditMode]
    [System.Serializable]
    public class ScriptableObjectForJson : ScriptableObject
    {
        public string filePath;
        public Node fileStruct;
        public List<Node> nodeList;
       public void SetFile()
        {
            filePath=EditorUtility.OpenFilePanel("Get the json file to be edited", "C:", "");
        }
        public void OpenJsonFile()
        {
            var reader = new JsonFx.Json.JsonReader();
            if (filePath==null || filePath.Length == 0)
            {
                throw new ArgumentNullException(filePath, "Please specify file path");
            }
            if(File.Exists(filePath))
            {
                string json=File.ReadAllText(filePath);
                Dictionary<string,object> data = (Dictionary<string,object>)reader.Read(json);
                 nodeList = new List<Node>();
                if (data != null)
                {
                    ProcessNestedData(data,fileStruct,null,nodeList,0);
                }
                else
                {
                    Debug.LogError("Failed to Parse Json Data");
                }
            }
            else
            {
                Debug.LogError("File not found: " + filePath);
            }
        }

        private void ProcessNestedData(Dictionary<string,object> data,Node node,Node prev,List<Node> nodeList,int level)
        {
             node = new Node();
            int i = 0;
            foreach ( var kvp in data)
            {
                node.name = kvp.Key;
                Debug.Log(node.name);
                if (kvp.Value is Dictionary<string, object>)
                {
                    node.children = new List<Node>();
                    Dictionary<string, object> dict=(Dictionary<string, object>)kvp.Value;
                    for(int j = i; j >= 0; j--)
                    {
                        node.children.Add(new Node());
                    }
                    
                    ProcessNestedData((Dictionary<string, object>)kvp.Value, node.children[i],node,nodeList, level+1);
                   if(prev!=null) prev.children[i] = node;
                    i++;
                }
                else
                {
                   node.obj = kvp.Value;
                }
                if(level==0) nodeList.Add(node);
            }
            if (level == 0) fileStruct = node;
        }
    }
  
    
    [Serializable]
    public class Node
    {
        public string name;
        public System.Object obj;
        public List<Node> children;
       Node(string name, System.Object obj)
        {
            this.name = name;
            this.obj = obj;
            children = null;
        }
       public Node()
        {
            children = null;
        }
    }
}
