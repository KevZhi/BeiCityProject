using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class ScriptLoad{
    //public static ScriptLoad instance;
    private static int index = 0;
    private static List<string> txt = new List<string>();

    private static string scriptPath = "Assets/Resources/Text/";

    //void Awake()
    //{
    //    //instance = this;
    //    index = 0;
    //}

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
    }

    public static ScriptData LoadNext()
    {
        //Console.WriteLine(index);
        if (index < txt.Count)
        {

            string[] datas = txt[index].Split('，');

            //int type = int.Parse(datas[0]);
            string type = datas[0];
            //Console.WriteLine(type);
            //Debug.Log(type);
            if (type == "0")
            {
                string picName = datas[1];
                index++;
                return new ScriptData(type, picName);
            }
            else
            {

                string pos = datas[1];
                string name = datas[2];
                string talk = datas[3];
                string picName = datas[4];
                index++;
                return new ScriptData(type, pos, name, talk, picName);
            }

        }
        else
        {
            return null;
        }
    }
}
