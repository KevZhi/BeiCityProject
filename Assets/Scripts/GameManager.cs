﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

[RequireComponent(typeof(ScriptManager))]
[RequireComponent(typeof(ComputerInput))]
public class GameManager : MonoBehaviour {
    
    public string sceneName;

    private PlayerDataStructure playerData;

    private bool isExist;

    public string savedText;

    [Header("=====公共调用组件=====")]
    public GameObject objRoot;

    public DialogManager dm;
    public DialogController dc;

    public MenuManager mm;

    public TestSceneChange testScene;
    public EventDataController edc;
    public SceneObjectController soc;
    public TestSceneChange tsc;
    public PlayerStateController psc;
    public BackgroundMusicController bgmc;
    public TargetController tc;
    public BackgroundController bgc;
    public EffectController ec;
    //new
    //public ScriptManager scriptManager;
    //public KeyboardInput keyboardInput;
    private void Awake()
    {
        //scriptManager = GetComponent<ScriptManager>();
        //keyboardInput = GetComponent<KeyboardInput>();
        //dm = this.GetComponent<DialogManager>();

        //dc = this.GetComponent<DialogController>();
        //mm = this.GetComponent<MenuManager>();

        //testScene = this.GetComponent<TestSceneChange>();
        //edc = this.GetComponent<EventDataController>();
        //soc = this.GetComponent<SceneObjectController>();
        //tsc = this.GetComponent<TestSceneChange>();
        //psc = this.GetComponent<PlayerStateController>();
        //bgmc = this.GetComponent<BackgroundMusicController>();
        //tc = this.GetComponent<TargetController>();
        //bgc = this.GetComponent<BackgroundController>();
        //ec = this.GetComponent<EffectController>();
    }

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
        //savedText = System.DateTime.Now +  "\n" +  tc.target.text;
    }

    public void SavePlayerData()
    {
        string saveDataPath = Application.persistentDataPath +"/" + EventSystem.current.currentSelectedGameObject.name + ".db";

        WWW loadDB = new WWW("file://" + Application.persistentDataPath + "/location.db");

        File.WriteAllBytes(saveDataPath, loadDB.bytes);

        playerData = new PlayerDataStructure(sceneName,savedText);

        File.WriteAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json", JsonUtility.ToJson(playerData));

    }

    public void LoadPlayerData()
    {

        isExist = File.Exists(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".db");

        if (isExist)
        {
            LoadEventData();
            LoadPlayerState();

            mm.OpenOrCloseLoadPanel(false);
            mm.OpenOrCloseGameMenu(false);

            playerData = JsonUtility.FromJson<PlayerDataStructure>(File.ReadAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json"));
            Globe.nextSceneName = playerData.CurScene;
            SceneManager.LoadScene("loading");

        }
    }

    public void LoadEventData()
    {
        string connSaver = "data source= " + Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".db"; //Path to database.
        SqliteConnection dbconnSaver = new SqliteConnection(connSaver);
        dbconnSaver.Open(); //Open connection to the database.

        string connLocal = "data source= " + Application.persistentDataPath + "/location.db"; //Path to database.
        SqliteConnection dbconnLocal = new SqliteConnection(connLocal);
        dbconnLocal.Open(); //Open connection to the database.

        SqliteCommand dbcmdSaver = dbconnSaver.CreateCommand();
        string sqlQuerySaver = "SELECT * " + "FROM EventData";
        dbcmdSaver.CommandText = sqlQuerySaver;
        SqliteDataReader readerSaver = dbcmdSaver.ExecuteReader();

        while (readerSaver.Read())
        {

            SqliteCommand dbcmdLocal = dbconnLocal.CreateCommand();
            string sqlQueryLocal = "UPDATE " + "EventData" + " SET " + "EventState" + "=" + "'"+ readerSaver.GetString(readerSaver.GetOrdinal("EventState")) + "'"+ " WHERE " + "EventName" + "=" + "'" + readerSaver.GetString(readerSaver.GetOrdinal("EventName")) + "'";
            dbcmdLocal.CommandText = sqlQueryLocal;
            SqliteDataReader readerLocal = dbcmdLocal.ExecuteReader();

            readerLocal.Close();
            readerLocal = null;

            dbcmdLocal.Cancel();
            dbcmdLocal.Dispose();
            dbcmdLocal = null;
       
        }

        dbconnLocal.Close();
        dbconnLocal.Dispose();
        dbconnLocal = null;

        readerSaver.Close();
        readerSaver = null;

        dbcmdSaver.Cancel();
        dbcmdSaver.Dispose();
        dbcmdSaver = null;

        dbconnSaver.Close();
        dbconnSaver.Dispose();
        dbconnSaver = null;

    }

    public void LoadPlayerState()
    {
        string connSaver = "data source= " + Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".db"; //Path to database.
        SqliteConnection dbconnSaver = new SqliteConnection(connSaver);
        dbconnSaver.Open(); //Open connection to the database.

        string connLocal = "data source= " + Application.persistentDataPath + "/location.db"; //Path to database.
        SqliteConnection dbconnLocal = new SqliteConnection(connLocal);
        dbconnLocal.Open(); //Open connection to the database.

        SqliteCommand dbcmdSaver = dbconnSaver.CreateCommand();
        string sqlQuerySaver = "SELECT * " + "FROM PlayerState";
        dbcmdSaver.CommandText = sqlQuerySaver;
        SqliteDataReader readerSaver = dbcmdSaver.ExecuteReader();

        while (readerSaver.Read())
        {

            SqliteCommand dbcmdLocal = dbconnLocal.CreateCommand();
            string sqlQueryLocal = "UPDATE " + "PlayerState" + " SET " + "value" + "=" + readerSaver.GetInt32(readerSaver.GetOrdinal("value")) + " WHERE " + "name" + "=" + "'" + readerSaver.GetString(readerSaver.GetOrdinal("name")) + "'";
            dbcmdLocal.CommandText = sqlQueryLocal;
            SqliteDataReader readerLocal = dbcmdLocal.ExecuteReader();

            readerLocal.Close();
            readerLocal = null;

            dbcmdLocal.Cancel();
            dbcmdLocal.Dispose();
            dbcmdLocal = null;

        }

        dbconnLocal.Close();
        dbconnLocal.Dispose();
        dbconnLocal = null;

        readerSaver.Close();
        readerSaver = null;

        dbcmdSaver.Cancel();
        dbcmdSaver.Dispose();
        dbcmdSaver = null;

        dbconnSaver.Close();
        dbconnSaver.Dispose();
        dbconnSaver = null;

    }

    public void NewGame()
    {
        Globe.nextSceneName = "floor2";
        SceneManager.LoadScene("loading");
    }

    public void ContinueToTitle()
    {
        mm.beginning.SetActive(false);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

}
