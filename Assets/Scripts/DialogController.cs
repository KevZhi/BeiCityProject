using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class DialogController : MonoBehaviour {

    private GameManager gm;
    public bool next;

    public bool isloading;


    public bool quitDialog;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
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
            if (next)
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
                        gm.edc.active = true;//改变事件列表状态
                        gm.dm.QuitDialog();
                        //TryToLoadNextEvent(gm.dm.curName);
                    }
                }
            }
        }

        if (gm.testScene.hasChange)
        {
            TryToAutoHappendEvent();
        }

        if (quitDialog)
        {
            quitDialog = false;
            TryToLoadNextEvent(gm.dm.curName);
        }
    }


    public void TryToAutoHappendEvent()
    {
        string conn = "data source= " + Application.persistentDataPath + "/location.db";
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

            bool isready = IsTrue(dbconn, StartEvent, "ready");

            if (isready)
            {
                if (NeedEventStart == "no")
                {
                    //print(StartEvent + " auto start!");
                    gm.dm.curName = StartEvent;
                    gm.dm.StartDialog();
                }
                else
                {
                    bool isstart = IsTrue(dbconn, NeedEventStart, "start");
                    if (isstart)
                    {
                        //print(StartEvent + " auto start!");
                        gm.dm.curName = StartEvent;
                        gm.dm.StartDialog();
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
    public bool IsTrue(SqliteConnection dbconn, string NeedCheckEventName, string NeedCheckEventState)
    {
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

        return istrue;
    }

    public void TryToLoadNextEvent(string eventName)
    {
        string conn = "data source= " + Application.persistentDataPath + "/location.db";
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
                gm.dm.StartDialog();
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

}
