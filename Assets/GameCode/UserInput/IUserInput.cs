using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUserInput : MonoBehaviour
{
    [Header("=====Output singals=====")]

    //1.pressing signal

    //2.trigger once signal
    public bool nextDialog;

    //3.double triggger

    [Header("=====Others=====")]
    public bool inputEnable = true;
    public bool dialogEnable = false;
}
