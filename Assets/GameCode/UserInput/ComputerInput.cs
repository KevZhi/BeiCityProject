using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerInput : IUserInput
{
    private void Awake()
    {
        EventCenter.AddListener(EventType.DialogStart, SetDialogEnble);
        EventCenter.AddListener(EventType.DialogFinish, SetDialogDisable);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.DialogStart, SetDialogEnble);
        EventCenter.RemoveListener(EventType.DialogFinish, SetDialogDisable);
    }
    /// <summary>
    /// 开启对话状态
    /// </summary>
    private void SetDialogEnble()
    {
        dialogEnable = true;
    }

    /// <summary>
    /// 关闭对话状态
    /// </summary>
    private void SetDialogDisable()
    {
        dialogEnable = false;
    }

    void Update()
    {
        if (inputEnable)
        {
            if (dialogEnable)
            {
                nextDialog = Input.GetMouseButtonDown(0);
                if (nextDialog)
                {
                    EventCenter.Broadcast(EventType.NextSentence);
                }
            }
           
        }
    }
}