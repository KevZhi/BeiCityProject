using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptData{
    //0：代表载入背景图片 1：代表载入人物对话内容
    public string type;
    public string pos;
    public string name;
    public string talk;
    public string picName;

    public ScriptData(string type, string pos, string name, string talk, string picName)
    {
        this.type = type;
        this.pos = pos;
        this.name = name;
        this.talk = talk;
        this.picName = picName;
    }

    public ScriptData(string type, string picName)
    {
        this.type = type;
        this.picName = picName;
    }
}
