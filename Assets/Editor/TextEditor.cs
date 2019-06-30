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
        GetWindow(typeof(TextEditor));
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
        editName = EditorGUILayout.TextArea(editName);
        EditorGUILayout.Space();
        if (GUILayout.Button("Open"))
        {

            string path = EditorUtility.OpenFilePanel("Open text", Application.dataPath + scriptPath, "txt");
            if (path != "")
            {
                //EditorGUILayout.LabelField(path.Substring(0, path.LastIndexOf('/')), options);

                editName = path.Replace(path.Substring(0, path.LastIndexOf('/') + 1), "");
                editName = editName.Substring(0, editName.LastIndexOf('.'));
                //Debug.Log(path);

                //scriptContent = DoCreateText.Read(scriptPath + editName + ".txt");
                LoadScript(editName);
            }

        }
        EditorGUILayout.Space();
        //scriptContent = EditorGUILayout.TextArea(scriptContent);
        for (int i = 0; i < txt.Count; i++)
        {
            //LoadSentence(i);
            EditorGUILayout.BeginVertical("box");
            HandleData(LoadSentence(i));
            txt[i] = EditorGUILayout.TextArea(txt[i]);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }


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

    public void HandleData(ScriptData data)
    {
        string type = "";
        if (data == null)
            return;

        switch (data.type)
        {
            case ScriptType.Dialogue:
                type = "Dialogue";
                type = EditorGUILayout.TextArea(type);
                break;
            case ScriptType.FadeInRole:
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
