using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System;

public class TextEditor : EditorWindow
{

    string scriptName = "TextName";
    string editName = "";
    string scriptContent = "";
    bool isCreate = false;
    bool isEdit = false;
    bool isTrue = false;
    private static string scriptPath = "Resources/Text/";

    [MenuItem("Window/Text Editor")]
    static void Init()
    {
       var window =  GetWindow(typeof(TextEditor));
       
    }

    void OnGUI()
    {
        //var options = new[] { GUILayout.Width(200), GUILayout.Height(30) };

        isCreate = EditorGUILayout.BeginToggleGroup("CreateText", isCreate);
        if (isCreate)
        {
            isEdit = false;
            ShowCreatePanel();
        }
        else
        {
            RemoveNotification();
        }
        EditorGUILayout.EndToggleGroup();
        isEdit = EditorGUILayout.BeginToggleGroup("EditText", isEdit);
        if (isEdit)
        {
            isCreate = false;
            ShowEditPanel();
        }
        else
        {
            RemoveNotification();
        }
        EditorGUILayout.EndToggleGroup();
    }

    /// <summary>
    /// 新建自定义脚本
    /// </summary>
    /// <param name="scriptName"></param>
    static void CreateScript(string scriptName)
    {
        DoCreateText.CreateFile(scriptPath + scriptName + ".txt");
    }

    private void ShowCreatePanel()
    {
        var options = new[] { GUILayout.Width(200), GUILayout.Height(30) };

        //ShowNotification(new GUIContent("CreateText"));
        EditorGUILayout.BeginHorizontal("box");
        EditorGUILayout.LabelField("ScriptName", options);
        scriptName = EditorGUILayout.TextArea(scriptName);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Create"))
        {
            CreateScript(scriptName);
        }
    }

    private void ShowEditPanel()
    {
        
        var options = new[] { GUILayout.Width(200), GUILayout.Height(30) };
        //ShowNotification(new GUIContent("EditText"));
        EditorGUILayout.LabelField("Text Name : " + editName);
        EditorGUILayout.Space();
        if (GUILayout.Button("Open"))
        {

            string path = EditorUtility.OpenFilePanel("Open text", Application.dataPath + scriptPath, "txt");
            if (path != "")
            {
                dataTypesIndexs = new List<int>();
                dataTypes = new List<string>();

                editName = path.Replace(path.Substring(0, path.LastIndexOf('/') + 1), "");
                editName = editName.Substring(0, editName.LastIndexOf('.'));
                //Debug.Log(path);

                //scriptContent = DoCreateText.Read(scriptPath + editName + ".txt");
                LoadScript(editName);
                //path = "";
            }

        }

        EditorGUILayout.Space();
        //scriptContent = EditorGUILayout.TextArea(scriptContent);

        if (dataTypesIndexs.Count< txt.Count)
        {
            for (int i = 0; i < txt.Count; i++)
            {
                //LoadSentence(i);
                
                HandleData(LoadSentence(i));
                //txt[i] = EditorGUILayout.TextArea(txt[i]);
                //if (EditorGUILayout.TextArea(txt[i]) == txt[i])
                //{
                //    break;
                //}
            }
        }
        
        for (int i = 0; i < dataTypesIndexs.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(dataTypes[i]);
            EditorGUILayout.Space();
            dataTypesIndexs[i] = EditorGUILayout.Popup(dataTypesIndexs[i], types);
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
        }

        Debug.Log("data types index count " + dataTypesIndexs.Count);
        Debug.Log("data types " + dataTypes.Count);
    }

    private static List<string> txt = new List<string>();
   
    private void LoadScript(string txtFileName)
    {
        txt = new List<string>();
        StreamReader stream = new StreamReader(Application.dataPath + "/" + scriptPath + txtFileName + ".txt");
        while (!stream.EndOfStream)
        {
            txt.Add(stream.ReadLine());

        }
        stream.Close();

        //for (int i = 0; i < txt.Count; i++)
        //{
        //    //LoadSentence(i);
        //    EditorGUILayout.BeginVertical("box");
        //    HandleData(LoadSentence(i),i);
        //    //txt[i] = EditorGUILayout.TextArea(txt[i]);
        //    //if (EditorGUILayout.TextArea(txt[i]) == txt[i])
        //    //{
        //    //    break;
        //    //}

        //    EditorGUILayout.EndVertical();
        //    EditorGUILayout.Space();
        //}
    }

    /// <summary>
    /// 逐条解析文本
    /// </summary>
    /// <returns></returns>
    public ScriptData LoadSentence(int index)
    {
        if (index < txt.Count)
        {

            string[] datas = txt[index].Split('|');
            ScriptType type = (ScriptType)Enum.Parse(typeof(ScriptType), datas[0]);
            //Debug.Log(type);
            switch (type)
            {
                case ScriptType.Dialogue:
                    string role = datas[1];
                    string talk = datas[2];
                    return new ScriptData(type, role, talk);
                case ScriptType.FadeInRole:
                    RolePos pos = (RolePos)Enum.Parse(typeof(RolePos), datas[1]);
                    string picName = datas[2];
                    return new ScriptData(type, pos, picName);
                case ScriptType.FadeOutRole:
                    RolePos pos1 = (RolePos)Enum.Parse(typeof(RolePos), datas[1]);
                    return new ScriptData(type, pos1);
                default:
                    Debug.Log("error ScriptType");
                    return null;
            }
        }
        else
        {
            return null;
        }
    }
    List<int> dataTypesIndexs = new List<int>();
    List<string> dataTypes = new List<string>();
    string[] types = new string[] {
            ScriptType.Dialogue.ToString(),
            ScriptType.FadeInRole.ToString(),
            ScriptType.FadeOutRole.ToString()
        };
    private void HandleData(ScriptData data)
    {
        //indexs = new List<int>();
        string type = "";
        if (data == null)
            return;

        //EditorGUILayout.LabelField(ScriptType.Dialogue.ToString());
        //gUIContent.
        switch (data.type)
        {
            case ScriptType.Dialogue:
                dataTypesIndexs.Add(0);
                dataTypes.Add(ScriptType.Dialogue.ToString());
                //EditorGUILayout.DropdownButton(new GUIContent("a" + "b"), FocusType.Keyboard, options);
                //index = EditorGUILayout.Popup(index, types);
                type = "Dialogue";
                type = EditorGUILayout.TextArea(type);
                break;
            case ScriptType.FadeInRole:
                dataTypesIndexs.Add(1);
                dataTypes.Add(ScriptType.FadeInRole.ToString());
                //EditorGUILayout.DropdownButton(new GUIContent("a" + "b"), FocusType.Keyboard, options);
                //index = EditorGUILayout.Popup(index, types);
                type = "FadeInRole";
                type = EditorGUILayout.TextArea(type);
                //gm.keyboardInput.action = true;
                //switch (data.pos)
                //{
                //    case RolePos.Left:
                //        SetImage(left, data.picName);
                //        EventCenter.Broadcast(EventType.NextSentence);
                //        TweenEffect.SetImageFadeIn(left);
                //        break;
                //    case RolePos.Right:
                //        SetImage(right, data.picName);
                //        EventCenter.Broadcast(EventType.NextSentence);
                //        TweenEffect.SetImageFadeIn(right);
                //        break;
                //    default:
                //        Debug.Log("error RolePos");
                //        break;
                //}
                break;
            case ScriptType.FadeOutRole:
                dataTypesIndexs.Add(2);
                dataTypes.Add(ScriptType.FadeOutRole.ToString());
                type = "FadeOutRole";
                type = EditorGUILayout.TextArea(type);
                //switch (data.pos)
                //{
                //    case RolePos.Left:
                //        TweenEffect.SetImageFadeOut(left);
                //        EventCenter.Broadcast(EventType.NextSentence);
                //        break;
                //    case RolePos.Right:
                //        TweenEffect.SetImageFadeOut(right);
                //        EventCenter.Broadcast(EventType.NextSentence);
                //        break;
                //    default:
                //        Debug.Log("error RolePos");
                //        break;
                //}
                break;
            default:
                Debug.Log("HandleData data.type");
                break;
        }
    }
}
