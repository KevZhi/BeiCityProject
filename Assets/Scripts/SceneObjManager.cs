using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System;

public class SceneObjManager : MonoBehaviour {

    public bool created;
    public bool destroyed;

    public bool checkInDialogStart;
    public bool checkInDialogFinish;

    public SQLiteHelper eventSQL;

    private GameManager gm;
    private Transform objParent;

    private string curSceneName;

    public Vector3 role1Pos = new Vector3(-6f, 4f, 0);
    public Vector3 role2Pos = new Vector3(-4f, 4f, 0);

    public Vector3 notice1Pos = new Vector3(-6f, 0, 0);
    public Vector3 notice2Pos = new Vector3(-4f, 0, 0);

    public Vector3 thing1Pos = new Vector3(-6f, -2f, 0);
    public Vector3 thing2Pos = new Vector3(-4f, -2f, 0);
    public Vector3 thing3Pos = new Vector3(-2f, -2f, 0);
 
    public Vector3 position1Pos = new Vector3(7f, 5.3f, 0);
    public Vector3 position2Pos = new Vector3(7f, 4f, 0);
    public Vector3 position3Pos = new Vector3(7f, 2.7f, 0);
 
    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start () {
        objParent = gm.objRoot.transform;
	}
	
	// Update is called once per frame
	void Update () {

        if (!destroyed)
        {
            if (SceneManager.GetActiveScene().name == "loading")
            {
                DestroyAll();
                destroyed = true;
                created = false;
            }
        }

        if (destroyed)
        {
            if (SceneManager.GetActiveScene().name != "loading")
            {
                DestroyAll();
                destroyed = false;
                created = false;
            }
        }

        if (!created && SceneManager.GetActiveScene().name != "loading")
        {

            created = true;
            destroyed = false;


          
            curSceneName = SceneManager.GetActiveScene().name;
            eventSQL = new SQLiteHelper("data source=" + Application.persistentDataPath + "/location.db");
            SqliteDataReader readList = eventSQL.ReadFullTable("SceneObjList");

   
            while (readList.Read())
            {
                if (readList.GetString(readList.GetOrdinal("Scene")) == curSceneName)
                {

                    string ReadyEventName = readList.GetString(readList.GetOrdinal("NeedEventReady"));
                    string FinishEventName = readList.GetString(readList.GetOrdinal("NeedEventFinish"));
                    string FinishEventName2 = readList.GetString(readList.GetOrdinal("NeedEvent2Finish"));
                    string StartEventName = readList.GetString(readList.GetOrdinal("NeedEventStart"));
                    string StartEventName2 = readList.GetString(readList.GetOrdinal("NeedEvent2Start"));

                    bool isready = IsTrue(ReadyEventName, "ready");
                    bool isfinish = IsTrue(FinishEventName, "finish");
                    bool isfinish2 = IsTrue(FinishEventName2, "finish");
                    bool isstart = IsTrue(StartEventName, "start");
                    bool isstart2 = IsTrue(StartEventName2, "start");

                    if ((isready == true || ReadyEventName == "no") && (isfinish == true || FinishEventName == "no") && (isfinish2 == true || FinishEventName2 == "no") && (isstart == true || StartEventName == "no") && (isstart2 == true || StartEventName2 == "no"))
                    {
                        Vector3 pos = GetPosition(readList.GetString(readList.GetOrdinal("Position")));
                        CreateObj(readList.GetString(readList.GetOrdinal("ObjName")), pos);
                        print(readList.GetString(readList.GetOrdinal("ObjName")) + " creat!");
                    }
                }
            }


          

            if (checkInDialogStart)
            {
                print("start "+ gm.dm.curName);
                CheckPlayerState(gm.dm.curName);
                CheckSetEventStart(gm.dm.curName);
                CheckSetEventFinsh(gm.dm.curName);
                checkInDialogStart = false;
            }


            if (checkInDialogFinish)
            {
                print("finish " + gm.dm.curName);
                CheckNextEvent(gm.dm.curName);
                print("next " + gm.dm.curName);
                checkInDialogFinish = false;
            }
        

            CheckEventCanAutoHappend();

            eventSQL.CloseConnection();
        }
    }

    public GameObject CreateObj(string objName, Vector3 pos)
    {
        GameObject prefeb = Resources.Load(objName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);

        obj.name = objName;

        obj.transform.SetParent(objParent, false);
        obj.transform.localPosition = pos;

        return obj;
    }

    public Vector3 GetPosition(string posName)
    {
        Vector3 pos = (Vector3)this.GetType().GetField(posName).GetValue(this);
        return pos;
    }

    public bool IsTrue(string NeedCheckEventName,string NeedCheckEventState)
    {
        SqliteDataReader reader = eventSQL.MyReadTable("EventData", "EventState", "EventName", "=", "'" + NeedCheckEventName + "'");
        string str = reader["EventState"].ToString();
        bool istrue = (str == NeedCheckEventState);
        if (NeedCheckEventName!="no")
        {
            //print(NeedCheckEventName + "'s " + NeedCheckEventState + " state is " + istrue);
        }
        return istrue;
    }

    public void DestroyAll()
    {
        int ChildCount = objParent.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            Destroy(objParent.GetChild(i).gameObject);
        }
    }

    public void CheckEventCanAutoHappend()
    {
        SqliteDataReader readList = eventSQL.ReadFullTable("AutoEventList");
        while (readList.Read())
        {

            string StartEvent = readList.GetString(readList.GetOrdinal("EventName"));
            string NeedScene = readList.GetString(readList.GetOrdinal("NeedScene"));
            string NeedEventStart = readList.GetString(readList.GetOrdinal("NeedEventStart"));
            if (curSceneName == NeedScene)
            {
                bool isready = IsTrue(StartEvent, "ready");
                bool isstart = IsTrue(NeedEventStart, "start");
                if (isready && (isstart == true || NeedEventStart == "no"))
                {
                    print(StartEvent + "auto start!");
                    gm.dm.curName = StartEvent;
                    gm.dm.StartDialog();
                }
            }
        }
    }

    public void CheckPlayerState(string eventName)
    {
        SqliteDataReader reader = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("PlayerState")) != "no")
                {
                    string stateName = reader.GetString(reader.GetOrdinal("PlayerState"));
                    eventSQL.UpdateEventState("PlayerState", "value", "value+1", "name", "=", "'" + stateName + "EXP" + "'");
                    CheckLVUP(stateName);
                }
            }
        }
    }

    /// <summary>
    /// 使用这个后，会改变当前事件名curName
    /// </summary>
    /// <param name="eventName">未改变的curName</param>
    public void CheckNextEvent(string eventName)
    {
        SqliteDataReader reader = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("NextEvent")) != "no")
                {
                    gm.dm.curName = reader.GetString(reader.GetOrdinal("NextEvent"));
                    gm.dm.StartDialog();
                }
            }
        }
    }

    public void CheckSetEventFinsh(string eventName)
    {
        SqliteDataReader readList = eventSQL.ReadFullTable("EventList");
        while (readList.Read())
        {
            if (readList.GetString(readList.GetOrdinal("EventName")) == eventName)
            {
                if (readList.GetString(readList.GetOrdinal("SetEventFinish")) != "no")
                {
                    string setEventName = readList.GetString(readList.GetOrdinal("SetEventFinish"));
                    eventSQL.UpdateEventState("EventData", "EventState", "'finish'", "EventName", "=", "'" + setEventName + "'");
                    //print(setEventName);
                }
            }
        }
    }

    public void CheckSetEventStart(string eventName)
    {
        SqliteDataReader reader = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("SetEventStart")) != "no")
                {
                    string setEventName = reader.GetString(reader.GetOrdinal("SetEventStart"));
                    eventSQL.UpdateEventState("EventData", "EventState", "'start'", "EventName", "=", "'" + setEventName + "'");
                    //print(setEventName);
                }
            }
        }
    }
    /// <summary>
    /// 检验是否可以升级
    /// </summary>
    /// <param name="stateName"></param>
    private void CheckLVUP(string stateName)
    {
        SqliteDataReader reader = eventSQL.ReadFullTable("PlayerState");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("name")) == (stateName + "EXP"))
            {
                if (reader.GetInt32(reader.GetOrdinal("value")) == 2)
                {
                    eventSQL.UpdateEventState("PlayerState", "value", "value+1", "name", "=", "'" + stateName + "LV" + "'");
                    gm.ps.LVup = true;
                }
            }
        }
    }

}
