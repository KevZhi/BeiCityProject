using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

public class GameManager : MonoBehaviour {

    public string sceneName;

    private PlayerDataStructure playerData;
    private EventDataStructure eventData;

    public GameObject commonMenu;

    public GameObject wantToSave;
    public GameObject wantToLoad;
    public GameObject saveComplete;
    public GameObject wantToLeave;

    private bool isExist;

    public GameObject menuUI;
    public GameObject savePanel;
    public GameObject loadPanel;
    public GameObject statePanel;
    public GameObject optionPanel;
    public GameObject targetPanel;
    public GameObject titleMenu;
    public GameObject loadInTitlePanel;

    public GameObject buffPanel;
    public GameObject noticePanel;

    public GameObject showMenuBtn;
    public GameObject saveBtn;
    public GameObject loadBtn;
    public GameObject stateBtn;
    public GameObject optionBtn;
    public GameObject titleBtn;
    public GameObject closeMenuBtn;

    public GameObject black;
    public GameObject begin;
    public GameObject log;
    public GameObject AnswerOrNot;
    public GameObject HelpOrNot;
    public GameObject ObserveOrNot;
    public GameObject ChatOrNot;
    public GameObject SitOrNot;

    public GameObject sceneMask;

    [Header("=====公共调用组件=====")]
    public GameObject objRoot;
    public PlayerState ps;
    public DialogManager dm;
    public DialogController dc;
    public InteractionManager im;
    public SceneObjManager sm;

    private void Awake()
    {
        commonMenu = GameObject.Find("CommonMenu");
        objRoot = GameObject.Find("objRoot");

        ps = this.GetComponent<PlayerState>();
        dm = this.GetComponent<DialogManager>();
        im = this.GetComponent<InteractionManager>();
        dc = this.GetComponent<DialogController>();
        sm = this.GetComponent<SceneObjManager>();
    }

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }


    public void SavePlayerData()
    {
        string saveDataPath = Application.persistentDataPath +"/" + EventSystem.current.currentSelectedGameObject.name + ".db";

        WWW loadDB = new WWW(Application.persistentDataPath + "/location.db");

        File.WriteAllBytes(saveDataPath, loadDB.bytes);

        playerData = new PlayerDataStructure(sceneName);

        File.WriteAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json", JsonUtility.ToJson(playerData));
        
        //saveComplete.SetActive(true);
        //saveComplete.GetComponentInChildren<Text>().text = "存档完成";
    }

    public void LoadPlayerData()
    {

        isExist = File.Exists(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".db");

        if (isExist)
        {
            LoadEventData();
            LoadPlayerState();

            playerData = JsonUtility.FromJson<PlayerDataStructure>(File.ReadAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json"));
            Globe.nextSceneName = playerData.CurScene;
            SceneManager.LoadScene("loading");

            CancelLoadPanel();
            titleMenu.SetActive(false);
            loadInTitlePanel.SetActive(false);
            CloseMenu();
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

    public void WantToSave()
    {
        wantToSave.SetActive(true);
        wantToSave.GetComponentInChildren<Text>().text = "若存档位置已有记录\n旧的存档将会被覆盖";
        menuUI.SetActive(false);
    }

    public void WantToLoad()
    {
        wantToLoad.SetActive(true);
        wantToLoad.GetComponentInChildren<Text>().text = "若读取已有存档\n未保存的进度将会失去";
        menuUI.SetActive(false);
    }

    public void WantToLeave()
    {
        wantToLeave.SetActive(true);
        wantToLeave.GetComponentInChildren<Text>().text = "返回标题画面\n未保存的进度将会失去";
        menuUI.SetActive(false);
    }

    public void CancelLeave()
    {
        wantToLeave.SetActive(false);
        menuUI.SetActive(true);
    }

    public void CallSavePanel()
    {
        menuUI.SetActive(false);

        saveComplete.SetActive(false);
        wantToSave.SetActive(false);
        wantToLoad.SetActive(false);
        savePanel.SetActive(true);
        loadPanel.SetActive(false);
    }

    public void CallLoadPanel()
    {
        menuUI.SetActive(false);

        saveComplete.SetActive(false);
        wantToSave.SetActive(false);
        wantToLoad.SetActive(false);
        savePanel.SetActive(false);
        loadPanel.SetActive(true);
    }

    public void CancelSavePanel()
    {
        menuUI.SetActive(true);
 
        saveComplete.SetActive(false);
        wantToSave.SetActive(false);
        wantToLoad.SetActive(false);
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
    }

    public void CancelLoadPanel()
    {
        menuUI.SetActive(true);
 
        saveComplete.SetActive(false);
        wantToSave.SetActive(false);
        wantToLoad.SetActive(false);
        savePanel.SetActive(false);
        loadPanel.SetActive(false);
    }

    public void CallStatePanel()
    {
        //SQLem.ShowPlayerState();
        menuUI.SetActive(false);
        statePanel.SetActive(true);
    }

    public void CancelStatePanel()
    {
        menuUI.SetActive(true);
        statePanel.SetActive(false);
    }

    public void CallOptionPanel()
    {
        menuUI.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void CancelOptionPanel()
    {
        if (SceneManager.GetActiveScene().name == "1.welcome")
        {
            titleMenu.SetActive(true);
        }
        else
        {
            menuUI.SetActive(true);
        }
  
        optionPanel.SetActive(false);
    }

    public void ShowMenu()
    {
        showMenuBtn.SetActive(false);
        closeMenuBtn.SetActive(true);

        saveBtn.SetActive(true);
        loadBtn.SetActive(true);
        stateBtn.SetActive(true);
        optionBtn.SetActive(true);
        titleBtn.SetActive(true);

        sceneMask.SetActive(true);

        //roleRoot.SetActive(false);
        //posRoot.SetActive(false);

        targetPanel.SetActive(false);
    }

    public void CloseMenu()
    {
        showMenuBtn.SetActive(true);
        closeMenuBtn.SetActive(false);

        saveBtn.SetActive(false);
        loadBtn.SetActive(false);
        stateBtn.SetActive(false);
        optionBtn.SetActive(false);
        titleBtn.SetActive(false);

        sceneMask.SetActive(false);

        //roleRoot.SetActive(true);
        //posRoot.SetActive(true);

        targetPanel.SetActive(true);
    }

    public void LoadInTitle()
    {
        titleMenu.SetActive(false);
        loadInTitlePanel.SetActive(true);
    }

    public void CancelLoadInTitle()
    {
        titleMenu.SetActive(true);
        loadInTitlePanel.SetActive(false);
    }

    public void CallOptionInTitle()
    {
        titleMenu.SetActive(false);
        optionPanel.SetActive(true);
    }

    public void NewGame()
    {
        Globe.nextSceneName = "floor2";
        SceneManager.LoadScene("loading");
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    public void GoToTitle()
    {
        Globe.nextSceneName = "1.welcome";
        SceneManager.LoadScene("loading");
        sceneMask.SetActive(false);
        wantToLeave.SetActive(false);
    }

    public void StartGame()
    {
        black.SetActive(false);
        begin.SetActive(false);
    }

    public void ShowLog()
    {
        titleMenu.SetActive(false);
        sceneMask.SetActive(true);
        log.SetActive(true);
    }

    public void CancelLog()
    {
        titleMenu.SetActive(true);
        sceneMask.SetActive(false);
        log.SetActive(false);
    }

}
