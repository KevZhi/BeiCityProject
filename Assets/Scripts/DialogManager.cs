using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System;
using System.Xml;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour {

    private GameManager gm;

    public string curName;

    public GameObject portraitImage;
    public GameObject dialogImage;
    public GameObject roleName;
    public GameObject detail;
    public GameObject optionPanel;
    public GridLayoutGroup optionsGroup;

    public float panelWith;
    public float panelHeight;

    public List<string> dialoguesList = new List<string>();
    public int dialoguesIndex;
    public int dialoguesCount;

    private Transform allPortrait;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    void Start()
    {
        allPortrait = portraitImage.transform;
        optionsGroup = optionPanel.GetComponent<GridLayoutGroup>();
    }

    public void StartDialog()
    {
        dialoguesList = new List<string>();

        dialoguesIndex = 0;
        dialoguesCount = 0;

        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM EventDetail WHERE name = " + "'" + curName + "'" + " ORDER BY orderID ASC";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        //print(reader.Read());
        //print(reader.GetString(reader.GetOrdinal("detail")));
        while (reader.Read())
        {
            //print(reader.GetString(reader.GetOrdinal("detail")));

            string role = CheckName(dbconn, reader.GetString(reader.GetOrdinal("role")));
            string detail = reader.GetString(reader.GetOrdinal("detail"));
            string portrait = reader.GetString(reader.GetOrdinal("portrait"));
            string rolePos = reader.GetString(reader.GetOrdinal("position"));
            string showorhide = reader.GetString(reader.GetOrdinal("showOrHide"));
            string addition = reader.GetString(reader.GetOrdinal("addition"));

            dialoguesList.Add(role + "," + detail + "," + portrait + "," + rolePos + "," + showorhide + "," + addition);
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

        DialoguesHandle(0);


        gm.objRoot.SetActive(false);
        gm.mm.ShowOrHideGameMenuBtn(false);
        SetDialogUI(true);

        gm.psc.active = true;

    }

    public string CheckName(SqliteConnection dbconn,string nickname)
    {
        string truename = nickname;

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM NameList WHERE nickname = " + "'" + nickname + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        if (reader.Read())
        {
            truename = reader.GetString(reader.GetOrdinal("truename"));
        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;


        return truename;
    }

    public void DialoguesHandle(int dialogueIndex)
    {
        string[] role_detail_array = dialoguesList[dialogueIndex].Split(',');

        string role = role_detail_array[0];
        string role_detail = role_detail_array[1];
        string role_portrait = role_detail_array[2];
        string role_position = role_detail_array[3];
        string role_showorhide = role_detail_array[4];
        string role_addition = role_detail_array[5];

        roleName.GetComponent<Text>().text = role;
        detail.GetComponent<Text>().text = role_detail;

        if (role_portrait != "no")
        {
            CheckPortrait(role_portrait, role_position, role_showorhide);
        }

        if (role_addition != "no")
        {
            CheckAddition(role_addition);
        }
    }

    public void CheckPortrait(string portrait , string rolePos , string isshow)
    {
        string[] portrait_detail = portrait.Split('、');
        string[] rolePos_detail = rolePos.Split('、');
        string[] isshow_detail = isshow.Split('、');

        int count = portrait_detail.Length;
        //print(count);
        for (int i = 0; i < count; i++)
        {
            Image img = allPortrait.Find(rolePos_detail[i]).GetComponent<Image>();
            img.sprite = Resources.Load("Pictures\\" + portrait_detail[i], typeof(Sprite)) as Sprite;
            if (isshow_detail[i] == "show")
            {
                img.gameObject.SetActive(true);
            }
            if (isshow_detail[i] == "hide")
            {
                img.gameObject.SetActive(false);
            }
        }
    }

    public void CheckAddition(string addition)
    {
        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM AdditionList WHERE name = " + "'" + addition + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        string type = reader["type"].ToString();

        if (type == "option")
        {
            gm.dc.istalking = false;
            optionPanel.SetActive(true);

            string A = reader["optionA"].ToString();
            string B = reader["optionB"].ToString();

            CreateOption("A", A);
            CreateOption("B", B);

        }
        if (type == "move")
        {
            string scene = reader["name"].ToString();
            SceneManager.LoadScene(scene);
        }
        if (type == "effect")
        {
            string effect = reader["name"].ToString();
            if (effect == "timegone")
            {
                gm.ec.goreset = true;
            }
            if (effect == "fallsleep")
            {
                gm.ec.fallsleepreset = true;
            }
            if (effect == "wakeup")
            {
                gm.ec.wakeupreset = true;
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

    public GameObject CreateOption(string name, string text)
    {
        //print("create!");
        GameObject prefeb = Resources.Load("option") as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);
        obj.name = name;
        obj.transform.Find("Text").GetComponent<Text>().text = text;
        Button button = obj.GetComponent<Button>();

        obj.transform.SetParent(optionPanel.transform, false);

        button.onClick.AddListener(delegate () {
            LoadBranch(name);
        });

        return obj;
    }

    public void LoadBranch(string obj)
    {
        curName = curName + obj;
        gm.dc.activeStart = true;

        int ChildCount = optionPanel.transform.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }
        optionPanel.SetActive(false);
    }


    public void QuitDialog()
    {
        gm.objRoot.SetActive(true);
        gm.mm.ShowOrHideGameMenuBtn(true);
        SetDialogUI(false);

        //print("reset");
        ResetRolePortrait();

    }

    public void ResetRolePortrait()
    {

        int ChildCount = allPortrait.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            allPortrait.GetChild(i).gameObject.SetActive(false);
        }
    }



  
    private void OnGUI()
    {
        panelWith = Mathf.Abs(2 * optionPanel.GetComponent<RectTransform>().rect.x);
        panelHeight = Mathf.Abs(2 * optionPanel.GetComponent<RectTransform>().rect.y);

        optionsGroup.padding.left = (int)(panelWith * 0.05f / 2);
        optionsGroup.padding.right = (int)(panelWith * 0.05f / 2);
        optionsGroup.spacing = new Vector2(panelWith * 0.05f, panelWith * 0.05f);
        optionsGroup.cellSize = new Vector2(panelWith * 0.2f, panelHeight * 0.6f);
    }


    
    public void SetDialogUI(bool isopen)
    {
        dialogImage.SetActive(isopen);
        gm.mm.maskWhite.SetActive(isopen);
        portraitImage.SetActive(isopen);
        //gm.objRoot.SetActive(!isopen);
    }

    public void CheckState(string stateDetail)
    {
        if (stateDetail == "+black")
        {
            //gm.black.SetActive(true);
        }
        if (stateDetail == "-black")
        {
            //gm.black.SetActive(false);
        }

        //移动
        if (stateDetail == "GoToClass15")
        {
            Globe.nextSceneName = "class15";
            SceneManager.LoadScene("loading");
        }
        if (stateDetail == "GoToSupportClass15")
        {
            Globe.nextSceneName = "supportClass15";
            SceneManager.LoadScene("loading");
        }

    }

}
