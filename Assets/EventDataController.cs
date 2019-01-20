using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class EventDataController : MonoBehaviour {

    public GameManager gm;

    public bool active;

    void Start () {
        gm = this.GetComponent<GameManager>();
    }

	void Update () {
        if (active)
        {
            active = false;
            TryToChangeEventData(gm.dm.curName);
            //print("eventdata");
            gm.soc.active = true;
            gm.dc.activeNext = true;
            gm.bgmc.active = true;
            gm.tc.active = true;
        }
	}

    public void TryToChangeEventData(string eventName)
    {

        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM EventList WHERE name = " + "'" + eventName + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {

            if (reader.GetString(reader.GetOrdinal("SetEventStart")) != "no")
            {
                string setEventName = reader.GetString(reader.GetOrdinal("SetEventStart"));

                string conn2 = "data source= " + Application.persistentDataPath + "/location.db";
                SqliteConnection dbconn2 = new SqliteConnection(conn2);
                dbconn2.Open();

                SqliteCommand dbcmd2 = dbconn2.CreateCommand();
                string sqlQuery2 = "UPDATE EventData SET EventState = 'start' WHERE EventName = " + "'" + setEventName + "'";
                dbcmd2.CommandText = sqlQuery2;
                SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                reader2.Close();
                reader2 = null;

                dbcmd2.Cancel();
                dbcmd2.Dispose();
                dbcmd2 = null;

                dbconn2.Close();
                dbconn2.Dispose();
                dbconn2 = null;

            }

            if (reader.GetString(reader.GetOrdinal("SetEventStart2")) != "no")
            {
                string setEventName = reader.GetString(reader.GetOrdinal("SetEventStart2"));

                string conn2 = "data source= " + Application.persistentDataPath + "/location.db";
                SqliteConnection dbconn2 = new SqliteConnection(conn2);
                dbconn2.Open();

                SqliteCommand dbcmd2 = dbconn2.CreateCommand();
                string sqlQuery2 = "UPDATE EventData SET EventState = 'start' WHERE EventName = " + "'" + setEventName + "'";
                dbcmd2.CommandText = sqlQuery2;
                SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                reader2.Close();
                reader2 = null;

                dbcmd2.Cancel();
                dbcmd2.Dispose();
                dbcmd2 = null;

                dbconn2.Close();
                dbconn2.Dispose();
                dbconn2 = null;
            }

            if (reader.GetString(reader.GetOrdinal("SetEventFinish")) != "no")
            {
                string setEventName = reader.GetString(reader.GetOrdinal("SetEventFinish"));

                string conn2 = "data source= " + Application.persistentDataPath + "/location.db";
                SqliteConnection dbconn2 = new SqliteConnection(conn2);
                dbconn2.Open();

                SqliteCommand dbcmd2= dbconn2.CreateCommand();
                string sqlQuery2 = "UPDATE EventData SET EventState = 'finish' WHERE EventName = " + "'" + setEventName + "'";
                dbcmd2.CommandText = sqlQuery2;
                SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                //print("set " + setEventName + " finish");

                reader2.Close();
                reader2 = null;

                dbcmd2.Cancel();
                dbcmd2.Dispose();
                dbcmd2 = null;

                dbconn2.Close();
                dbconn2.Dispose();
                dbconn2 = null;
            }

            if (reader.GetString(reader.GetOrdinal("SetEventFinish2")) != "no")
            {
                string setEventName2 = reader.GetString(reader.GetOrdinal("SetEventFinish2"));

                string conn2 = "data source= " + Application.persistentDataPath + "/location.db";
                SqliteConnection dbconn2 = new SqliteConnection(conn2);
                dbconn2.Open();

                SqliteCommand dbcmd2 = dbconn2.CreateCommand();
                string sqlQuery2 = "UPDATE EventData SET EventState = 'finish' WHERE EventName = " + "'" + setEventName2 + "'";
                dbcmd2.CommandText = sqlQuery2;
                SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                reader2.Close();
                reader2 = null;

                dbcmd2.Cancel();
                dbcmd2.Dispose();
                dbcmd2 = null;

                dbconn2.Close();
                dbconn2.Dispose();
                dbconn2 = null;
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
