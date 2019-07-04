using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class BackgroundMusicController : MonoBehaviour {

    public GameManager gm;

    public string audioName;

    public AudioSource bgm;
    public AudioClip clip;

    public bool active;

    // Use this for initialization
    void Start () {
        gm = this.GetComponent<GameManager>();		
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            //print("audio");
            active = false;
            TryToChangeBGM();
        }
	}

    public void LoadAudio()
    {
        clip = Resources.Load("AudioClips/" + audioName) as AudioClip;
        bgm.clip = clip;
        bgm.Play();
    }

    public void StopAudio()
    {
        bgm.clip = null;
        bgm.Stop();
    }

    public void TryToChangeBGM()
    {
        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM AudioList WHERE scene = " + "'" + gm.tsc.curScene + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {           
            string NeedEvent = reader.GetString(reader.GetOrdinal("NeedEventFinish"));
            string AudioName = reader.GetString(reader.GetOrdinal("AudioName"));

            string conn2 = "data source= " + Application.persistentDataPath + "/location.db";
            SqliteConnection dbconn2 = new SqliteConnection(conn2);
            dbconn2.Open();

            SqliteCommand dbcmd2 = dbconn2.CreateCommand();
            string sqlQuery2 = "SELECT EventState FROM EventData WHERE EventName = " + "'" + NeedEvent + "'";
            dbcmd2.CommandText = sqlQuery2;
            SqliteDataReader reader2 = dbcmd2.ExecuteReader();
            string need = reader2["EventState"].ToString();

            bool canPlay = (need == "" || need == "finish");

            //print(canPlay);

            if (canPlay)
            {
                if (bgm.clip == null || bgm.clip.name != AudioName)
                {
                    //print("play audio " + AudioName);
                    audioName = AudioName;
                    LoadAudio();
                }
            }
            else
            {
                StopAudio();
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
