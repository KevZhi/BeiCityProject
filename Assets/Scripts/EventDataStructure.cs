using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System;

[System.Serializable]
public class EventDataStructure
{
    //事件状态
    public int Event001;
    public int Event002;

    //物体状态
    public int Desk01;
    public int Desk02;

    public EventDataStructure(
        int event001,
        int event002)
    {
        this.Event001 = event001;
        this.Event002 = event002;
    }
}
