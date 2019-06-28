using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class ScriptLoad{

    private static int index = 0;
    private static List<string> txt = new List<string>();

    private static string scriptPath = "Assets/Resources/Text/";

    public static void LoadScripts(string txtFileName)
    {
        index = 0;
        txt = new List<string>();
        StreamReader stream = new StreamReader(scriptPath + txtFileName);
        while (!stream.EndOfStream)
        {
            txt.Add(stream.ReadLine());

        }
        stream.Close();
        EventCenter.Broadcast(EventType.DialogStart);
    }
    /// <summary>
    /// 逐条解析文本
    /// </summary>
    /// <returns></returns>
    public static ScriptData LoadNext()
    {
        if (index < txt.Count)
        {

            string[] datas = txt[index].Split('|');
            index++;
            //int type = int.Parse(datas[0]);
            ScriptType type = (ScriptType)Enum.Parse(typeof(ScriptType),datas[0]);
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
            EventCenter.Broadcast(EventType.DialogFinish);
            //Debug.Log("finish");
            return null;
        }
    }
}
