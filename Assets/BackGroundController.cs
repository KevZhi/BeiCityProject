using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class BackGroundController : MonoBehaviour {

    public GameManager gm;
    public Image background;

    void Start () {
        background = GameObject.Find("BG").GetComponent<Image>();
        gm = this.GetComponent<GameManager>();
    }

	void Update () {
        if(gm.testScene.hasChange)
        {
            //print("sceneChange!");
            ChangeBG();
        }
    }

    public void ChangeBG()
    {
        string conn = "data source= " + Application.persistentDataPath + "/location.db"; //Path to database.
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
