using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class TestDialog : MonoBehaviour {

    public List<string> dialoguesList = new List<string>();
    public int dialoguesCount;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void StartDialog()
    {
        dialoguesList = new List<string>();

        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM EventDetail WHERE name = " + "'" + "aaa" + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        //print(reader.Read());
        //print(reader.GetString(reader.GetOrdinal("detail")));
        while (reader.Read())
        {
            //print(reader.GetString(reader.GetOrdinal("detail")));
            string detail = reader.GetString(reader.GetOrdinal("detail"));
            string role = reader.GetString(reader.GetOrdinal("role"));
            dialoguesList.Add(role + "," + detail);
        }

        dialoguesCount = dialoguesList.Count;

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
