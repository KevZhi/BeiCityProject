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
    //public Image left1;
    //public Image left2;
    //public Image center;
    //public Image right1;
    //public Image rightPos2;

    public GameObject dialogImage;
    public GameObject roleName;
    public GameObject detail;

    //private List<string> dialogues_list;//存放dialogues的list
    public List<string> dialoguesList = new List<string>();
    //public int dialogue_index;//对话索引
    public int dialoguesIndex;
    public int dialoguesCount;
    //public int dialogue_count;//对话数量

    private Transform allPortrait;

    //private string role;
    //private string role_detail;
    //private string portrait;
    //private string rolePos;
    //private string state;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    void Start()
    {
        allPortrait = portraitImage.transform;
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

            dialoguesList.Add(role + "," + detail + "," + portrait + "," + rolePos + "," + showorhide);
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

    public void CheckPortrait(string portrait , string rolePos , string isshow)
    {
        if (portrait != "no")
        {
            Image img = allPortrait.Find(rolePos).GetComponent<Image>();
            img.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
            if (isshow == "show")
            {
                img.gameObject.SetActive(true);
            }
            if (isshow == "hide")
            {
                img.gameObject.SetActive(false);
            }
        }
    }

    public void QuitDialog()
    {
        gm.objRoot.SetActive(true);
        gm.mm.ShowOrHideGameMenuBtn(true);
        SetDialogUI(false);

        //print("reset");
        ResetRolePortrait();

    }

    public void DialoguesHandle(int dialogueIndex)
    {
        string[] role_detail_array = dialoguesList[dialogueIndex].Split(',');

        string role = role_detail_array[0];
        string role_detail = role_detail_array[1];
        string role_portrait = role_detail_array[2];
        string role_position = role_detail_array[3];
        string role_showorhide = role_detail_array[4];

        roleName.GetComponent<Text>().text = role;
        detail.GetComponent<Text>().text = role_detail;

        CheckPortrait(role_portrait,role_position,role_showorhide);

        //LoadRolePortrait(rolePos);
        //CheckState(state);
    }

    //public void Dialogues_handle(int dialogue_index)
    //{
    //    string[] role_detail_array = dialogues_list[dialogue_index].Split(',');

    //    role = role_detail_array[0];
    //    role_detail = role_detail_array[1];
    //    rolePos = role_detail_array[2];
    //    portrait = role_detail_array[3];
    //    state = role_detail_array[4];

    //    roleName.GetComponent<Text>().text = role;
    //    detail.GetComponent<Text>().text = role_detail;

    //    LoadRolePortrait(rolePos);
    //    CheckState(state);
    //}

    public void ResetRolePortrait()
    {
  
        int ChildCount = allPortrait.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            allPortrait.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    public void SetDialogUI(bool isopen)
    {
        dialogImage.SetActive(isopen);
        gm.mm.maskWhite.SetActive(isopen);
        portraitImage.SetActive(isopen);
        //gm.objRoot.SetActive(!isopen);
    }

    //选项
    public void ChooseDecision(string choose)
    {
        curName = curName + choose;
        if (Resources.Load("Text/" + curName))
        {
            gm.dc.activeStart = true;
        }
        else
        {
            QuitDialog();
        }
    }

    public void CheckState(string stateDetail)
    {
        //图像
        //if (stateDetail == "+left1")
        //{
        //    leftPos1.gameObject.SetActive(true);
        //}
        //if (stateDetail == "-left1")
        //{
        //    leftPos1.gameObject.SetActive(false);
        //}
        //if (stateDetail == "+left2")
        //{
        //    leftPos2.gameObject.SetActive(true);
        //}
        //if (stateDetail == "-left2")
        //{
        //    leftPos2.gameObject.SetActive(false);
        //}
        //if (stateDetail == "+right1")
        //{
        //    rightPos1.gameObject.SetActive(true);
        //}
        //if (stateDetail == "-right1")
        //{
        //    rightPos1.gameObject.SetActive(false);
        //}
        //if (stateDetail == "+right2")
        //{
        //    rightPos2.gameObject.SetActive(true);
        //}
        //if (stateDetail == "-right2")
        //{
        //    rightPos2.gameObject.SetActive(false);
        //}
        //if (stateDetail == "+center")
        //{
        //    centerPos.gameObject.SetActive(true);
        //}
        //if (stateDetail == "-center")
        //{
        //    centerPos.gameObject.SetActive(false);
        //}

        //if (stateDetail == "+left2+right1")
        //{
        //    rightPos1.gameObject.SetActive(true);
        //    leftPos2.gameObject.SetActive(true);
        //}
        //if (stateDetail == "+left2-right1")
        //{
        //    rightPos1.gameObject.SetActive(false);
        //    leftPos2.gameObject.SetActive(true);
        //}
        //if (stateDetail == "+center-right1")
        //{
        //    rightPos1.gameObject.SetActive(false);
        //    centerPos.gameObject.SetActive(true);
        //}
        if (stateDetail == "+black")
        {
            //gm.black.SetActive(true);
        }
        if (stateDetail == "-black")
        {
            //gm.black.SetActive(false);
        }
        //选项
        if (stateDetail == "AnswerOrNot")
        {
            gm.dc.istalking = false;
            gm.mm.AnswerOrNot.SetActive(true);
        }
        if (stateDetail == "HelpOrNot")
        {
            gm.dc.istalking = false;
            gm.mm.HelpOrNot.SetActive(true);
        }
        if (stateDetail == "ObserveOrNot")
        {
            gm.dc.istalking = false;
            //print("stop");
            gm.mm.ObserveOrNot.SetActive(true);
        }
        if (stateDetail == "ChatOrNot")
        {
            gm.dc.istalking = false;
            gm.mm.ChatOrNot.SetActive(true);
        }
        if (stateDetail == "SitOrNot")
        {
            gm.dc.istalking = false;
            gm.mm.SitOrNot.SetActive(true);
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
