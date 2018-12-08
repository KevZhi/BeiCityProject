using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class EventDataManager : MonoBehaviour {

    private void Awake()
    {
    }

    void Start () {
    }

	void Update ()
    {
    }
    /// <summary>
    /// 检查是否拥有某事件的状态旗标
    /// </summary>
    /// <param name="fieldName">要查询的事件名</param>
    /// <returns></returns>
    public bool HasEvent(string fieldName)
    {
        return this.GetType().GetField(fieldName) != null;
    }
    /// <summary>
    /// 改变事件状态
    /// </summary>
    /// <param name="field">事件名</param>
    /// <param name="val">状态</param>
    public void SetEventState(string field, object val)
    {

        Type Ts = this.GetType();
        if (val.GetType() != Ts.GetField(field).FieldType)
        {
            val = Convert.ChangeType(val, Ts.GetField(field).FieldType);
        }
        Ts.GetField(field).SetValue(this, val);
    }
}
