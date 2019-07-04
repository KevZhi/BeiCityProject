using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptData{

    public ScriptType type;
    public RolePos pos;
    public string role;
    public string talk;
    public string picName;
    /// <summary>
    /// 载入图片
    /// </summary>
    /// <param name="type">fadeIn/OutPicture</param>
    /// <param name="pos"></param>
    /// <param name="picName"></param>
    public ScriptData(ScriptType type, RolePos pos, string picName)
    {
        this.type = type;
        this.pos = pos;
        this.picName = picName;
    }
    /// <summary>
    /// 图片消失
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    /// <param name="picName"></param>
    public ScriptData(ScriptType type, RolePos pos)
    {
        this.type = type;
        this.pos = pos;
    }

    /// <summary>
    /// 载入对话
    /// </summary>
    /// <param name="type">dialogue</param>
    /// <param name="pos"></param>
    /// <param name="name"></param>
    /// <param name="talk"></param>
    /// <param name="picName"></param>
    public ScriptData(ScriptType type, string role, string talk)
    {
        this.type = type;
        this.role = role;
        this.talk = talk;
    }

}

public enum ScriptType
{
    Dialogue,
    FadeInRole,
    FadeOutRole,
}

public enum RolePos
{
    Left,
    Right,
}