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

    private GameManager gm;
    private Transform objParent;


    [Header("=====上部图标的位置=====")]
    public Vector3 role1Pos = new Vector3(-6f, 4f, 0);
    public Vector3 role2Pos = new Vector3(-4f, 4f, 0);

    [Header("=====中部图标的位置=====")]
    public Vector3 notice1Pos = new Vector3(-6f, 0, 0);
    public Vector3 notice2Pos = new Vector3(-4f, 0, 0);

    [Header("=====下部图标的位置=====")]
    public Vector3 thing1Pos = new Vector3(-6f, -2f, 0);
    public Vector3 thing2Pos = new Vector3(-4f, -2f, 0);
    public Vector3 thing3Pos = new Vector3(-2f, -2f, 0);

    [Header("=====移动图标的位置=====")]
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

    private SqliteConnection dbconn;
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

        if (!created && SceneManager.GetActiveScene().name != "loading" && gm.sceneName != "1.welcome")
        {

            created = true;
            destroyed = false;

            SceneObjControl();
        }
    }
 

    public void SceneObjControl()
    {

        string conn = "data source= " + Application.persistentDataPath + "/location.db"; //Path to database.
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM SceneObjList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("Scene")) == gm.sceneName)
            {

                string ReadyEventName = reader.GetString(reader.GetOrdinal("NeedEventReady"));
                string FinishEventName = reader.GetString(reader.GetOrdinal("NeedEventFinish"));
                string FinishEventName2 = reader.GetString(reader.GetOrdinal("NeedEvent2Finish"));
                string StartEventName = reader.GetString(reader.GetOrdinal("NeedEventStart"));
                string StartEventName2 = reader.GetString(reader.GetOrdinal("NeedEvent2Start"));

                bool isready = IsTrue(ReadyEventName, "ready");
                bool isfinish = IsTrue(FinishEventName, "finish");
                bool isfinish2 = IsTrue(FinishEventName2, "finish");
                bool isstart = IsTrue(StartEventName, "start");
                bool isstart2 = IsTrue(StartEventName2, "start");

                if ((isready == true || ReadyEventName == "no") && (isfinish == true || FinishEventName == "no") && (isfinish2 == true || FinishEventName2 == "no") && (isstart == true || StartEventName == "no") && (isstart2 == true || StartEventName2 == "no"))
                {
                    Vector3 pos = GetPosition(reader.GetString(reader.GetOrdinal("Position")));
                    CreateObj(reader.GetString(reader.GetOrdinal("ObjName")), pos);
                    //print(reader.GetString(reader.GetOrdinal("ObjName")) + " creat!");
                }
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

       

        if (checkInDialogStart)
        {
            print("start " + gm.dm.curName);
            CheckPlayerState(gm.dm.curName);
            CheckSetEventStart(gm.dm.curName);
            CheckSetEventFinsh(gm.dm.curName);
            checkInDialogStart = false;
        }


        if (checkInDialogFinish)
        {
            print("finish " + gm.dm.curName);
            CheckNextEvent(gm.dm.curName);
            checkInDialogFinish = false;
        }


        CheckEventCanAutoHappend();

        dbconn.Close();
        dbconn.Dispose();
        dbconn = null;

    }


    public void DestroyAll()
    {
        int ChildCount = objParent.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            Destroy(objParent.GetChild(i).gameObject);
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
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT EventState " + "FROM EventData" + " WHERE " + " EventName " + " = " + "'" + NeedCheckEventName + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        string str = reader["EventState"].ToString();
        bool istrue = (str == NeedCheckEventState);
        if (NeedCheckEventName!="no")
        {
            //print(NeedCheckEventName + "'s " + NeedCheckEventState + " state is " + istrue);
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
        return istrue;
    }


    public void CheckEventCanAutoHappend()
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM AutoEventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {

            string StartEvent = reader.GetString(reader.GetOrdinal("EventName"));
            string NeedScene = reader.GetString(reader.GetOrdinal("NeedScene"));
            string NeedEventStart = reader.GetString(reader.GetOrdinal("NeedEventStart"));
            if (gm.sceneName == NeedScene)
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

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    public void CheckPlayerState(string eventName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("PlayerState")) != "no")
                {
                    string stateName = reader.GetString(reader.GetOrdinal("PlayerState"));

                    SqliteCommand dbcmd1 = dbconn.CreateCommand();
                    string sqlQuery1 = "UPDATE " + "PlayerState" + " SET " + "value" + "=" + "value+1" + " WHERE " + "name" + "=" + "'" + stateName + "EXP" + "'";
                    dbcmd1.CommandText = sqlQuery1;
                    SqliteDataReader reader1 = dbcmd1.ExecuteReader();
                    //eventSQL.UpdateEventState("PlayerState", "value", "value+1", "name", "=", "'" + stateName + "EXP" + "'");

                    reader1.Close();
                    reader1 = null;

                    dbcmd1.Cancel();
                    dbcmd1.Dispose();
                    dbcmd1 = null;

                    CheckLVUP(stateName);
                }
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    /// <summary>
    /// 检验是否可以升级
    /// </summary>
    /// <param name="stateName"></param>
    private void CheckLVUP(string stateName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM PlayerState";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        //SqliteDataReader reader = eventSQL.ReadFullTable("PlayerState");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("name")) == (stateName + "EXP"))
            {
                if (reader.GetInt32(reader.GetOrdinal("value")) == 2)
                {
                    SqliteCommand dbcmd1 = dbconn.CreateCommand();
                    string sqlQuery1 = "UPDATE " + "PlayerState" + " SET " + "value" + "=" + "value+1" + " WHERE " + "name" + "=" + "'" + stateName + "LV" + "'";
                    dbcmd1.CommandText = sqlQuery1;
                    SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                    reader1.Close();
                    reader1 = null;

                    dbcmd1.Cancel();
                    dbcmd1.Dispose();
                    dbcmd1 = null;
                    //eventSQL.UpdateEventState("PlayerState", "value", "value+1", "name", "=", "'" + stateName + "LV" + "'");
                    gm.ps.LVup = true;
                }
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    /// <summary>
    /// 使用这个后，会改变当前事件名curName
    /// </summary>
    /// <param name="eventName">未改变的curName</param>
    public void CheckNextEvent(string eventName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        //SqliteDataReader reader = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {

            bool isExist = reader.GetString(reader.GetOrdinal("EventName")) == eventName;
            //print(isExist);
            if (isExist)
            {
                if (reader.GetString(reader.GetOrdinal("NextEvent")) != "no")
                {
                    gm.dm.curName = reader.GetString(reader.GetOrdinal("NextEvent"));
                    gm.dm.StartDialog();
                    print("next " + gm.dm.curName);
                }
                else
                {
                    
                }
                break;
            }
            else
            {
                gm.dm.curName = null;
                //print(gm.dm.curName);
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    public void CheckSetEventFinsh(string eventName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        //SqliteDataReader readList = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("SetEventFinish")) != "no")
                {
                    string setEventName = reader.GetString(reader.GetOrdinal("SetEventFinish"));

                    SqliteCommand dbcmd1 = dbconn.CreateCommand();
                    string sqlQuery1 = "UPDATE " + "EventData" + " SET " + "EventState" + "=" + "'finish'" + " WHERE " + "EventName" + "=" + "'" + setEventName + "'";
                    dbcmd1.CommandText = sqlQuery1;
                    SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                    reader1.Close();
                    reader1 = null;

                    dbcmd1.Cancel();
                    dbcmd1.Dispose();
                    dbcmd1 = null;
                 
                    //eventSQL.UpdateEventState("EventData", "EventState", "'finish'", "EventName", "=", "'" + setEventName + "'");
                    //print(setEventName);
                }
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    public void CheckSetEventStart(string eventName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        //SqliteDataReader reader = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("SetEventStart")) != "no")
                {
                    string setEventName = reader.GetString(reader.GetOrdinal("SetEventStart"));


                    SqliteCommand dbcmd1 = dbconn.CreateCommand();
                    string sqlQuery1 = "UPDATE " + "EventData" + " SET " + "EventState" + "=" + "'start'" + " WHERE " + "EventName" + "=" + "'" + setEventName + "'";
                    dbcmd1.CommandText = sqlQuery1;
                    SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                    reader1.Close();
                    reader1 = null;

                    dbcmd1.Cancel();
                    dbcmd1.Dispose();
                    dbcmd1 = null;

                    //eventSQL.UpdateEventState("EventData", "EventState", "'start'", "EventName", "=", "'" + setEventName + "'");
                    //print(setEventName);
                }
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }


}
