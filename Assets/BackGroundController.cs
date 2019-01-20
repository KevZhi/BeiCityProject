using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour {

    public GameManager gm;
    public Image background;

    public bool active;

    void Start () {
        background = GameObject.Find("BG").GetComponent<Image>();
        gm = this.GetComponent<GameManager>();
    }

	void Update () {
        if(active)
        {
            active = false;
            //print("sceneChange!");
            ChangeBG();
        }
    }

    public void ChangeBG()
    {
        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT image FROM BackgroundList WHERE scene = " + "'" + gm.testScene.curScene + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        string str = reader["image"].ToString();
        //print(str);

        if (str != "")
        {
            background.sprite = Resources.Load("Background\\" + str, typeof(Sprite)) as Sprite;
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
