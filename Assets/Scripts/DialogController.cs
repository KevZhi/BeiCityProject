using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;

public class DialogController : MonoBehaviour {

    private GameManager gm;
    public bool istalking;

    public bool isloading;

    public bool activeAtuo;
    public bool activeNext;
    public bool activeStart;

    public bool canStart;

    void Start () {
        gm = this.GetComponent<GameManager>();
    }

	void Update () {

        if (gm.sceneName == "loading")
        {
            isloading = true;
        }
        else
        {
            isloading = false;
        }

        if (!isloading)
        {
            if (istalking)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.LeftControl))
                {
                    gm.dm.dialogue_index++;
                    if (gm.dm.dialogue_index < gm.dm.dialogue_count)
                    {
                        gm.dm.Dialogues_handle(gm.dm.dialogue_index);//那就载入下一条对话
                    }
                    else
                    {
                        istalking = false;
                        gm.edc.active = true;//改变事件列表状态
                        gm.dm.QuitDialog();
                    }
                }
            }
        }

        if (activeStart)
        {
            activeStart = false;
            TryToLoadEvent(gm.dm.curName);
        }    

        if (activeAtuo)
        {
            activeAtuo = false;
            TryToAutoHappendEvent();
        }

        if (activeNext)
        {
            activeNext = false;
            TryToLoadNextEvent(gm.dm.curName);
        }

        if (canStart)
        {
            canStart = false;
            istalking = true;

            gm.dm.StartDialog();
        }
    }


    public void TryToAutoHappendEvent()
    {
        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM AutoEventList WHERE scene = " + "'" + gm.tsc.curScene + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            string StartEvent = reader.GetString(reader.GetOrdinal("name"));
            string NeedEventStart = reader.GetString(reader.GetOrdinal("NeedEventStart"));

            bool isready = IsTrue(StartEvent, "ready");

            if (isready)
            {
                if (NeedEventStart == "no")
                {
                    //print(StartEvent + " auto start!");
                    gm.dm.curName = StartEvent;
                    activeStart = true;
                }
                else
                {
                    bool isstart = IsTrue(NeedEventStart, "start");
                    if (isstart)
                    {
                        //print(StartEvent + " auto start!");
                        gm.dm.curName = StartEvent;
                        activeStart = true;
                    }
                }
            }

        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn.Dispose();
        dbconn = null;

    }
    public bool IsTrue(string NeedCheckEventName, string NeedCheckEventState)
    {
        string conn = "data source= " + Application.persistentDataPath + "/location.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT EventState FROM EventData WHERE EventName = " + "'" + NeedCheckEventName + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        string str = reader["EventState"].ToString();
        bool istrue = (str == NeedCheckEventState);

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn.Dispose();
        dbconn = null;

        return istrue;
    }

    public void TryToLoadNextEvent(string eventName)
    {
        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT NextEvent FROM EventList WHERE name = " + "'" + eventName + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        if (reader.Read())
        {
            string str = reader["NextEvent"].ToString();
            if (str != "no")
            {
                gm.dm.curName = str;
                activeStart = true;
            }
            else
            {

            }
        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn.Dispose();
        dbconn = null;

    }

    public void TryToLoadEvent(string eventName)
    {

        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM EventList WHERE name = " + "'" + eventName + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        string stateName = reader["NeedPlayerState"].ToString();
        string needLV = reader["NeedLV"].ToString();
        if (stateName != "no" && stateName != "")
        {
            string conn2 = "data source= " + Application.persistentDataPath + "/location.db";
            SqliteConnection dbconn2 = new SqliteConnection(conn2);
            dbconn2.Open();

            SqliteCommand dbcmd2 = dbconn2.CreateCommand();
            string sqlQuery2 = "SELECT value FROM PlayerState WHERE name = " + "'" + stateName + "LV" + "'";
            dbcmd2.CommandText = sqlQuery2;
            SqliteDataReader reader2 = dbcmd2.ExecuteReader();

            string player = reader2["value"].ToString();
            //print(player);

            canStart = (Convert.ToInt32(player) >= Convert.ToInt32(needLV));

            if (canStart == false)
            {
                //print(eventName + " 需要的" + stateName + "等级 = " + needLV + " 现在的等级 = " + player);
                gm.mm.canotDo = true;
            }

            reader2.Close();
            reader2 = null;

            dbcmd2.Cancel();
            dbcmd2.Dispose();
            dbcmd2 = null;

            dbconn2.Close();
            dbconn2.Dispose();
            dbconn2 = null;

        }
        else
        {
            canStart = true;
        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn.Dispose();
        dbconn = null;

    }
}
