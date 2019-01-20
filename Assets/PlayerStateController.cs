using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class PlayerStateController : MonoBehaviour {

    public GameManager gm;
    public bool active;
    
    // Use this for initialization
    void Start () {
        gm = this.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

        if (active)
        {
            active = false;
            TryToChangePlayerState(gm.dm.curName);
        }
	}

    public void TryToChangePlayerState(string eventName)
    {

        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT PlayerState FROM EventList WHERE name = "+"'" + eventName + "'";
        dbcmd.CommandText = sqlQuery;

        SqliteDataReader reader = dbcmd.ExecuteReader();
        string stateName = reader["PlayerState"].ToString();
        bool canEXP = (stateName != "no" && stateName != "");

        string conn2 = "data source= " + Application.persistentDataPath + "/location.db";
        SqliteConnection dbconn2 = new SqliteConnection(conn2);
        dbconn2.Open();

        SqliteCommand dbcmd2 = dbconn2.CreateCommand();
        string sqlQuery2 = "SELECT EventState FROM EventData WHERE EventName = " + "'" + eventName + "'";
        dbcmd2.CommandText = sqlQuery2;

        SqliteDataReader reader2 = dbcmd2.ExecuteReader();
        string needReady = reader2["EventState"].ToString();
        bool canChange = (needReady == "" || needReady == "ready");

        if (canEXP && canChange)
        {
            SqliteCommand dbcmd1 = dbconn2.CreateCommand();
            string sqlQuery1 = "UPDATE PlayerState SET value = value+1 WHERE name = " + "'" + stateName + "EXP" + "'";
            dbcmd1.CommandText = sqlQuery1;
            SqliteDataReader reader1 = dbcmd1.ExecuteReader();

            //print(stateName + " 经验上升 ");
            TryToLevelUp(dbconn2, stateName);

            reader1.Close();
            reader1 = null;

            dbcmd1.Cancel();
            dbcmd1.Dispose();
            dbcmd1 = null;
        }

        reader2.Close();
        reader2 = null;

        dbcmd2.Cancel();
        dbcmd2.Dispose();
        dbcmd2 = null;

        dbconn2.Close();
        dbconn2.Dispose();
        dbconn2 = null;

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn.Dispose();
        dbconn = null;
    }

    public void TryToLevelUp(SqliteConnection dbconn, string stateName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM PlayerState WHERE name = " + "'" + stateName + "LV" + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        SqliteCommand dbcmd2 = dbconn.CreateCommand();
        string sqlQuery2 = "SELECT * FROM PlayerState WHERE name =" + "'" + stateName + "EXP" + "'";
        dbcmd2.CommandText = sqlQuery2;
        SqliteDataReader reader2 = dbcmd2.ExecuteReader();

        string lv = reader["value"].ToString();

        //print(reader["name"].ToString() + " = " + reader["value"].ToString() + " " + reader2["name"].ToString() + " = " + reader2["value"].ToString());
        string exp = reader2["value"].ToString();

        if ((lv == "0" && exp == "2") 
            || (lv == "1" && exp == "4") 
            || (lv == "2" && exp == "8") 
            || (lv == "3" && exp == "16")
            || (lv == "4" && exp == "32"))
        {
            SqliteCommand dbcmd1 = dbconn.CreateCommand();
            string sqlQuery1 = "UPDATE PlayerState SET value = value+1 WHERE name = " + "'" + stateName + "LV" + "'";
            dbcmd1.CommandText = sqlQuery1;
            SqliteDataReader reader1 = dbcmd1.ExecuteReader();

            reader1.Close();
            reader1 = null;

            dbcmd1.Cancel();
            dbcmd1.Dispose();
            dbcmd1 = null;

            //print(stateName + " LV up!");
            //gm.mm.levelUp = true;
        }


        reader2.Close();
        reader2 = null;

        dbcmd2.Cancel();
        dbcmd2.Dispose();
        dbcmd2 = null;

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }
}
