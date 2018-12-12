using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

public class MenuManager : MonoBehaviour {

    private GameManager gm;

    public GameObject titleMenu;
    public GameObject gameMenuBtn;
    public GameObject gameMenu;

    public GameObject maskWhite;
    public GameObject maskUI;
    public GameObject maskBlack;

    public GameObject wantToDo;

    public GameObject savePanel;
    public GameObject loadPanel;
    public GameObject statePanel;
    public GameObject optionPanel;
    public GameObject targetPanel;
    public Text targetText;
    public GameObject logPanel;

    public GameObject buffPanel;
    //public GameObject noticePanel;

    public Text ShamLVText;
    public Text PassiveLVText;
    public Text RebelLVText;
    public Text SelfishLVText;
    public Text EvilLVText;

    public GameObject warning;

    public GameObject AnswerOrNot;
    public GameObject HelpOrNot;
    public GameObject ObserveOrNot;
    public GameObject ChatOrNot;
    public GameObject SitOrNot;

    //public GameObject sceneMask;

    public GameObject beginning;
    private float timer;

    public bool levelUp;

    public bool canotDo;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
        //beginning.SetActive(true);
    }

    private void Update()
    {
        if (levelUp)
        {
            if (timer >= 1f)
            {
                levelUp = false;
                timer = 0;
                warning.SetActive(false);
            }
            else
            {
                timer += Time.deltaTime;
                warning.SetActive(true);
                warning.GetComponentInChildren<Text>().text = "等级提升了";
            }
        }
        if (canotDo)
        {
            if (timer >= 1f)
            {
                canotDo = false;
                timer = 0;
                warning.SetActive(false);
            }
            else
            {
                timer += Time.deltaTime;
                warning.SetActive(true);
                warning.GetComponentInChildren<Text>().text = "所需等级不足";
            }
        }
    }

    public void ShowOrHideTitleMenu(bool isShow)
    {
        titleMenu.SetActive(isShow);
    }

    public void ShowOrHideTitleMenuChild(bool isShow)
    {
        int ChildCount = titleMenu.transform.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            titleMenu.transform.GetChild(i).gameObject.SetActive(isShow);
        }
    }

    public void ShowOrHideGameMenuBtn(bool isShow)
    {
        gameMenuBtn.SetActive(isShow);
        targetPanel.SetActive(isShow);
    }

    public void ShowOrHideGameMenu(bool isShow)
    {
        gameMenu.SetActive(isShow);
        int ChildCount = gameMenu.transform.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            gameMenu.transform.GetChild(i).gameObject.SetActive(isShow);
        }
    }

    public void OpenOrCloseGameMenu(bool isOpen)
    {
        gm.objRoot.SetActive(!isOpen);
        ShowOrHideGameMenuBtn(!isOpen);
        maskUI.SetActive(isOpen);
        ShowOrHideGameMenu(isOpen);
    }

    public void OpenOrCloseLoadPanel(bool isOpen)
    {
        if (gm.sceneName=="1.welcome")
        {
            ShowOrHideTitleMenuChild(!isOpen);
        }
        else
        {
            ShowOrHideGameMenu(!isOpen);
        }
        loadPanel.SetActive(isOpen);
    }

    public void OpenOrCloseSavePanel(bool isOpen)
    {
        ShowOrHideGameMenu(!isOpen);
        savePanel.SetActive(isOpen);
    }

    public void OpenOrCloseStatePanel(bool isOpen)
    {
        ShowOrHideGameMenu(!isOpen);
        statePanel.SetActive(isOpen);
        if (isOpen)
        {
            string conn = "data source= " + Application.persistentDataPath + "/location.db"; //Path to database.
            SqliteConnection dbconn = new SqliteConnection(conn);
            dbconn.Open(); //Open connection to the database.

            SqliteCommand dbcmd = dbconn.CreateCommand();
            string sqlQuery = "SELECT * " + "FROM PlayerState";
            dbcmd.CommandText = sqlQuery;
            SqliteDataReader reader = dbcmd.ExecuteReader();

            while(reader.Read())
            {
                string name =  reader.GetString(reader.GetOrdinal("name"));
                if (name == "ShamLV")
                {
                    ShamLVText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
                }
                if (name == "PassiveLV")
                {
                    PassiveLVText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
                }
                if (name == "RebelLV")
                {
                    RebelLVText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
                }
                if (name == "SelfishLV")
                {
                    SelfishLVText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
                }
                if (name == "EvilLV")
                {
                    EvilLVText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
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

    public void OpenOrCloseOptionPanel(bool isOpen)
    {
        if (gm.sceneName == "1.welcome")
        {
            ShowOrHideTitleMenuChild(!isOpen);
        }
        else
        {
            ShowOrHideGameMenu(!isOpen);
        }
        optionPanel.SetActive(isOpen);
    }

    public void OpenOrCloseLogPanel(bool isOpen)
    {
        if (gm.sceneName == "1.welcome")
        {
            ShowOrHideTitleMenuChild(!isOpen);
        }
        else
        {
            ShowOrHideGameMenu(!isOpen);
        }
        logPanel.SetActive(isOpen);
    }

    public void OpenOrCloseWantToDo(string something)
    {
       
        if (something == "save")
        {
            ShowOrHideGameMenu(false);
            wantToDo.SetActive(true);
            wantToDo.GetComponentInChildren<Text>().text = "储存将覆盖已有存档";
        }
        if (something == "load")
        {
            ShowOrHideGameMenu(false);
            wantToDo.SetActive(true);
            wantToDo.GetComponentInChildren<Text>().text = "读取将失去当前进度";
        }
        if (something == "leave")
        {
            ShowOrHideGameMenu(false);
            wantToDo.SetActive(true);
            wantToDo.GetComponentInChildren<Text>().text = "返回标题将失去当前进度";
        }
        if (something == "back")
        {
            ShowOrHideGameMenu(true);
            wantToDo.SetActive(false);
            wantToDo.GetComponentInChildren<Text>().text = "warning";
        }
    }

    public void DoSomething()
    {
        wantToDo.SetActive(false);
        if (wantToDo.GetComponentInChildren<Text>().text == "储存将覆盖已有存档")
        {
            savePanel.SetActive(true);
        }
        if (wantToDo.GetComponentInChildren<Text>().text == "读取将失去当前进度")
        {
            loadPanel.SetActive(true);
        }
        if (wantToDo.GetComponentInChildren<Text>().text == "返回标题将失去当前进度")
        {
            Globe.nextSceneName = "1.welcome";
            SceneManager.LoadScene("loading");
        }
    }

}
