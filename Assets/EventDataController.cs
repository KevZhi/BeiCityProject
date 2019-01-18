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
            gm.dc.quitDialog = true;
            gm.bgmc.active = true;
        }
	}

    public void TryToChangeEventData(string eventName)
    {

        string conn = "data source= " + Application.persistentDataPath + "/location.db"; //Path to database.
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
                SqliteCommand dbcmd1 = dbconn.CreateCommand();
                string sqlQuery1 = "UPDATE EventData SET EventState = 'start' WHERE EventName = " + "'" + setEventName + "'";
                dbcmd1.CommandText = sqlQuery1;
                SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                reader1.Close();
                reader1 = null;

                dbcmd1.Cancel();
                dbcmd1.Dispose();
                dbcmd1 = null;

            }

            if (reader.GetString(reader.GetOrdinal("SetEventStart2")) != "no")
            {
                string setEventName = reader.GetString(reader.GetOrdinal("SetEventStart2"));
                SqliteCommand dbcmd1 = dbconn.CreateCommand();
                string sqlQuery1 = "UPDATE EventData SET EventState = 'start' WHERE EventName = " + "'" + setEventName + "'";
                dbcmd1.CommandText = sqlQuery1;
                SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                reader1.Close();
                reader1 = null;

                dbcmd1.Cancel();
                dbcmd1.Dispose();
                dbcmd1 = null;

            }

            if (reader.GetString(reader.GetOrdinal("SetEventFinish")) != "no")
            {
                string setEventName = reader.GetString(reader.GetOrdinal("SetEventFinish"));

                SqliteCommand dbcmd1 = dbconn.CreateCommand();
                string sqlQuery1 = "UPDATE EventData SET EventState = 'finish' WHERE EventName = " + "'" + setEventName + "'";
                dbcmd1.CommandText = sqlQuery1;
                SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                //print("set " + setEventName + " finish");

                reader1.Close();
                reader1 = null;

                dbcmd1.Cancel();
                dbcmd1.Dispose();
                dbcmd1 = null;
            }

            if (reader.GetString(reader.GetOrdinal("SetEventFinish2")) != "no")
            {
                string setEventName2 = reader.GetString(reader.GetOrdinal("SetEventFinish2"));

                SqliteCommand dbcmd2 = dbconn.CreateCommand();
                string sqlQuery2 = "UPDATE EventData SET EventState = 'finish' WHERE EventName = " + "'" + setEventName2 + "'";
                dbcmd2.CommandText = sqlQuery2;
                SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                reader2.Close();
                reader2 = null;

                dbcmd2.Cancel();
                dbcmd2.Dispose();
                dbcmd2 = null;
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
