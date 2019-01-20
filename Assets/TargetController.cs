using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class TargetController : MonoBehaviour {

    public GameManager gm;
    public Text target;

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
            TryToChangeTargetText();

        }
	}

    public void TryToChangeTargetText()
    {
        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM TargetText";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {

            string NeedEventStartName = reader.GetString(reader.GetOrdinal("NeedEventStart"));
            string Text = reader.GetString(reader.GetOrdinal("Text"));

            string conn2 = "data source= " + Application.persistentDataPath + "/location.db";
            SqliteConnection dbconn2 = new SqliteConnection(conn2);
            dbconn2.Open();

            SqliteCommand dbcmd2 = dbconn2.CreateCommand();
            string sqlQuery2 = "SELECT EventState FROM EventData WHERE EventName = " + "'" + NeedEventStartName + "'";
            dbcmd2.CommandText = sqlQuery2;
            SqliteDataReader reader2 = dbcmd2.ExecuteReader();

            if (reader2["EventState"].ToString() == "start")
            {
                target.text = Text;
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
