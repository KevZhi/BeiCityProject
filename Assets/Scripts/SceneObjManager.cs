using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;
using System.IO;

public class SceneObjManager : MonoBehaviour {

    public bool created;
    public bool destroyed;

    public bool checkInDialogStart;
    public bool checkInDialogFinish;

    public bool inTitle;

    private GameManager gm;
    private Transform objParent;


    [Header("=====上部图标的位置=====")]
    public Vector3 role1Pos = new Vector3(-6f, 4f, 0);
    public Vector3 role2Pos = new Vector3(-4f, 4f, 0);

    [Header("=====中部图标的位置=====")]
    public Vector3 notice1Pos = new Vector3(-6f, 0, 0);
    public Vector3 notice2Pos = new Vector3(-4f, 0, 0);

    [Header("=====下部图标的位置=====")]
    public Vector3 thing1Pos = new Vector3(-6f, -2f, 0);
    public Vector3 thing2Pos = new Vector3(-4f, -2f, 0);
    public Vector3 thing3Pos = new Vector3(-2f, -2f, 0);

    [Header("=====移动图标的位置=====")]
    public Vector3 position1Pos = new Vector3(7f, 5.3f, 0);
    public Vector3 position2Pos = new Vector3(7f, 4f, 0);
    public Vector3 position3Pos = new Vector3(7f, 2.7f, 0);
 
    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
        objParent = gm.objRoot.transform;
    }

    void Update () {
        

        if (!destroyed)
        {
            if (SceneManager.GetActiveScene().name == "loading")
            {
                DestroyAll();
                destroyed = true;
                created = false;
            }
        }

        if (destroyed)
        {
            //DestroyAll();
            //destroyed = false;
            //created = false;
            if (SceneManager.GetActiveScene().name != "loading" && SceneManager.GetActiveScene().name != "1.welcome")
            {
                DestroyAll();
                destroyed = false;
                created = false;
            }
        }


        if (!created)
        {
            if (SceneManager.GetActiveScene().name != "loading")
            {
                //print("creat!");

                if (File.Exists(Application.persistentDataPath + "/location.db"))
                {

                    created = true;
                    destroyed = false;

                    SceneObjControl();
                }



                //if (SceneManager.GetActiveScene().name == "1.welcome")
                //{
                //    inTitle = true;
                //    checkInDialogFinish = true;
                //    print("check in title");
                //}
            }
           
        }
    }

    private SqliteConnection dbconn;

    public void SceneObjControl()
    {
        string conn = "data source= " + Application.persistentDataPath + "/location.db"; //Path to database.
        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM SceneObjList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("Scene")) == SceneManager.GetActiveScene().name)
            {

                string ReadyEventName = reader.GetString(reader.GetOrdinal("NeedEventReady"));
                string FinishEventName = reader.GetString(reader.GetOrdinal("NeedEventFinish"));
                string FinishEventName2 = reader.GetString(reader.GetOrdinal("NeedEvent2Finish"));
                string StartEventName = reader.GetString(reader.GetOrdinal("NeedEventStart"));
                string StartEventName2 = reader.GetString(reader.GetOrdinal("NeedEvent2Start"));

                bool isready = IsTrue(ReadyEventName, "ready");
                bool isfinish = IsTrue(FinishEventName, "finish");
                bool isfinish2 = IsTrue(FinishEventName2, "finish");
                bool isstart = IsTrue(StartEventName, "start");
                bool isstart2 = IsTrue(StartEventName2, "start");

                if ((isready == true || ReadyEventName == "no") && (isfinish == true || FinishEventName == "no") && (isfinish2 == true || FinishEventName2 == "no") && (isstart == true || StartEventName == "no") && (isstart2 == true || StartEventName2 == "no"))
                {
                    Vector3 pos = GetPosition(reader.GetString(reader.GetOrdinal("Position")));
                    CreateObj(reader.GetString(reader.GetOrdinal("ObjName")), pos);
                    //print(reader.GetString(reader.GetOrdinal("ObjName")) + " creat!");
                }
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

       

        if (checkInDialogStart)
        {
            checkInDialogStart = false;
            if (CheckLVCondition(gm.dm.curName))
            {
                print("start " + gm.dm.curName);
                /* 开始时检测 */
                CheckPlayerState(gm.dm.curName);

                CheckSetEventStart(gm.dm.curName);
                CheckSetEventFinsh(gm.dm.curName);
                //CheckSetEventStart(gm.dm.curName);
                //CheckSetEventFinsh(gm.dm.curName);
                //checkInDialogStart = false;
            }
            //CheckLVCondition(gm.dm.curName);
            //print("start " + gm.dm.curName);
            //CheckPlayerState(gm.dm.curName);
            //CheckSetEventStart(gm.dm.curName);
            //CheckSetEventFinsh(gm.dm.curName);
        }

        if (checkInDialogFinish)
        {
            if (gm.dm.curName != null)
            {
                print("finish " + gm.dm.curName);
            }
     

            /* 结束后检测 */
            //CheckAudioList();

            CheckNextEvent(gm.dm.curName);
            checkInDialogFinish = false;
        }

        CheckTargetText();
        //CheckAudioList();
        CheckEventCanAutoHappend();


        dbconn.Close();
        dbconn.Dispose();
        dbconn = null;

    }


    public void DestroyAll()
    {
        int ChildCount = objParent.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            Destroy(objParent.GetChild(i).gameObject);
        }
    }

    public GameObject CreateObj(string objName, Vector3 pos)
    {
        GameObject prefeb = Resources.Load(objName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);

        obj.name = objName;

        obj.transform.SetParent(objParent, false);
        obj.transform.localPosition = pos;

        return obj;
    }

    public Vector3 GetPosition(string posName)
    {
        Vector3 pos = (Vector3)this.GetType().GetField(posName).GetValue(this);
        return pos;
    }

    public bool IsTrue(string NeedCheckEventName,string NeedCheckEventState)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT EventState " + "FROM EventData" + " WHERE " + " EventName " + " = " + "'" + NeedCheckEventName + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        string str = reader["EventState"].ToString();
        //print(str);
        bool istrue = (str == NeedCheckEventState);
        if (NeedCheckEventName!="no")
        {
            //print(NeedCheckEventName + "'s " + NeedCheckEventState + " state is " + istrue);
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
        return istrue;
    }

    /// <summary>
    /// 任务向导
    /// </summary>
    public void CheckTargetText()
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM TargetText";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
           
            string NeedEventStartName = reader.GetString(reader.GetOrdinal("NeedEventStart"));
            string Text = reader.GetString(reader.GetOrdinal("Text"));

            SqliteCommand dbcmd2 = dbconn.CreateCommand();
            string sqlQuery2 = "SELECT EventState" + " FROM EventData" + " WHERE" + " EventName " + " = " + "'" + NeedEventStartName + "'";
            dbcmd2.CommandText = sqlQuery2;
            SqliteDataReader reader2 = dbcmd2.ExecuteReader();

            if (reader2["EventState"].ToString() == "start")
            {
                gm.mm.targetText.text = Text;
                //print("target : " + Text);
            }

            reader2.Close();
            reader2 = null;

            dbcmd2.Cancel();
            dbcmd2.Dispose();
            dbcmd2 = null;
        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    public void CheckAudioList()
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM AudioList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {

            string NeedScene = reader.GetString(reader.GetOrdinal("NeedScene"));
            string NeedEventStartName = reader.GetString(reader.GetOrdinal("NeedEventStart"));
            string AudioName = reader.GetString(reader.GetOrdinal("AudioName"));

            if (SceneManager.GetActiveScene().name == NeedScene)
            {

                SqliteCommand dbcmd2 = dbconn.CreateCommand();
                string sqlQuery2 = "SELECT EventState" + " FROM EventData" + " WHERE" + " EventName " + " = " + "'" + NeedEventStartName + "'";
                dbcmd2.CommandText = sqlQuery2;
                SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                if (reader2["EventState"].ToString() == "start" || reader2["EventState"].ToString() == "")
                {
                    if (gm.am.bgm.clip == null || gm.am.bgm.clip.name != AudioName)
                    {
                        print("play audio " + AudioName);
                        gm.am.audioName = AudioName;
                        gm.am.LoadAudio();
                    }
                }

                reader2.Close();
                reader2 = null;

                dbcmd2.Cancel();
                dbcmd2.Dispose();
                dbcmd2 = null;
            }

        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }


    public void CheckEventCanAutoHappend()
    {
        //print("CheckEventCanAutoHappend");
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM AutoEventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            string StartEvent = reader.GetString(reader.GetOrdinal("EventName"));
            string NeedScene = reader.GetString(reader.GetOrdinal("NeedScene"));
            string NeedEventStart = reader.GetString(reader.GetOrdinal("NeedEventStart"));
            //print(gm.sceneName == NeedScene);
            if (SceneManager.GetActiveScene().name == NeedScene)
            {
                bool isready = IsTrue(StartEvent, "ready");
                bool isstart = IsTrue(NeedEventStart, "start");
                if (isready && (isstart == true || NeedEventStart == "no"))
                {
                    print(StartEvent + " auto start!");
                    gm.dm.curName = StartEvent;
                    gm.dm.StartDialog();
                }
            }
        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    /// <summary>
    /// 不符合条件会跳出对话
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public bool CheckLVCondition(string eventName)
    {
        bool canDo = true;

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("NeedPlayerState")) != "no")
                {
                    string stateName = reader.GetString(reader.GetOrdinal("NeedPlayerState"));
                    int needLV = reader.GetInt32(reader.GetOrdinal("NeedLV"));

                    SqliteCommand dbcmd2 = dbconn.CreateCommand();
                    string sqlQuery2 = "SELECT value" + " FROM PlayerState" + " WHERE" + " name " + "=" + "'" + stateName + "LV" + "'";
                    dbcmd2.CommandText = sqlQuery2;
                    SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                    print(eventName + " 需要的" + stateName +  "等级 = " + needLV + " 现在的等级 = " + reader2[0] );

                    canDo = ((int)reader2[0] >= needLV);
                    //print("canDo = " + canDo);

                    if (!canDo)
                    {
                        gm.mm.canotDo = true;
                        gm.dm.QuitDialog();
                    }

                    reader2.Close();
                    reader2 = null;

                    dbcmd2.Cancel();
                    dbcmd2.Dispose();
                    dbcmd2 = null;
                }
            }
        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
        print(eventName + " canDo = " + canDo);
        return canDo;
    }

    public void CheckPlayerState(string eventName)
    {
        //print("CheckPlayerState");
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("PlayerState")) != "no")
                {
                    string stateName = reader.GetString(reader.GetOrdinal("PlayerState"));

                    SqliteCommand dbcmd2 = dbconn.CreateCommand();
                    string sqlQuery2 = "SELECT EventState" + " FROM EventData" + " WHERE" + " EventName " + "=" + "'" + eventName + "'";
                    dbcmd2.CommandText = sqlQuery2;
                    SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                    

                    if (reader2["EventState"].ToString() == "ready" || reader2["EventState"].ToString() == "")
                    {
                        //print(reader2["EventState"].ToString());
                        SqliteCommand dbcmd1 = dbconn.CreateCommand();
                        string sqlQuery1 = "UPDATE " + "PlayerState" + " SET " + "value" + "=" + "value+1" + " WHERE " + "name" + "=" + "'" + stateName + "EXP" + "'";
                        dbcmd1.CommandText = sqlQuery1;
                        SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                        print(eventName + " *一次性* 经验上升 " + reader2.Read());

                        reader1.Close();
                        reader1 = null;

                        dbcmd1.Cancel();
                        dbcmd1.Dispose();
                        dbcmd1 = null;
                    }

                    reader2.Close();
                    reader2 = null;

                    dbcmd2.Cancel();
                    dbcmd2.Dispose();
                    dbcmd2 = null;

                    CheckLVUP(stateName);
                }
            }
        }

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    /// <summary>
    /// 检验是否可以升级，经验上限待调
    /// </summary>
    /// <param name="stateName"></param>
    private void CheckLVUP(string stateName)
    {
   
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM PlayerState" + " WHERE " + "name" + "=" + "'" + stateName + "LV" + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        SqliteCommand dbcmd2 = dbconn.CreateCommand();
        string sqlQuery2 = "SELECT * " + "FROM PlayerState" + " WHERE " + "name" + "=" + "'" + stateName + "EXP" + "'";
        dbcmd2.CommandText = sqlQuery2;
        SqliteDataReader reader2 = dbcmd2.ExecuteReader();

        string lv = reader["value"].ToString();

        print(reader["name"].ToString() + " = " + reader["value"].ToString() + " " +  reader2["name"].ToString() + " = " + reader2["value"].ToString());
        string exp = reader2["value"].ToString();

        if ((lv=="0"&&exp=="2")||(lv=="1"&&exp=="4") || (lv == "2" && exp == "8"))
        {
            SqliteCommand dbcmd1 = dbconn.CreateCommand();
            string sqlQuery1 = "UPDATE " + "PlayerState" + " SET " + "value" + "=" + "value+1" + " WHERE " + "name" + "=" + "'" + stateName + "LV" + "'";
            dbcmd1.CommandText = sqlQuery1;
            SqliteDataReader reader1 = dbcmd1.ExecuteReader();
            print(stateName +  " LV up!");
            reader1.Close();
            reader1 = null;

            dbcmd1.Cancel();
            dbcmd1.Dispose();
            dbcmd1 = null;

            gm.mm.levelUp = true;
        }


        reader2.Close();
        reader2 = null;

        dbcmd2.Cancel();
        dbcmd2.Dispose();
        dbcmd2 = null;

        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

    }

    /// <summary>
    /// 使用这个后，会改变当前事件名curName
    /// </summary>
    /// <param name="eventName">未改变的curName</param>
    public void CheckNextEvent(string eventName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {

            bool isExist = reader.GetString(reader.GetOrdinal("EventName")) == eventName;
            //print(isExist);
            if (isExist)
            {
                if (reader.GetString(reader.GetOrdinal("NextEvent")) != "no")
                {
                    gm.dm.curName = reader.GetString(reader.GetOrdinal("NextEvent"));
                    gm.dm.StartDialog();
                    print( gm.dm.curName + " next event happen !");
                }
                else
                {
                    
                }
                break;
            }
            else
            {
                gm.dm.curName = null;
                //print(gm.dm.curName);
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    public void CheckSetEventFinsh(string eventName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("SetEventFinish")) != "no")
                {
                    string setEventName = reader.GetString(reader.GetOrdinal("SetEventFinish"));

                    SqliteCommand dbcmd1 = dbconn.CreateCommand();
                    string sqlQuery1 = "UPDATE " + "EventData" + " SET " + "EventState" + "=" + "'finish'" + " WHERE " + "EventName" + "=" + "'" + setEventName + "'";
                    dbcmd1.CommandText = sqlQuery1;
                    SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                    print("set " + setEventName + " finish");

                    reader1.Close();
                    reader1 = null;

                    dbcmd1.Cancel();
                    dbcmd1.Dispose();
                    dbcmd1 = null;

                    if (reader.GetString(reader.GetOrdinal("SetEventFinish2")) != "no")
                    {
                        string setEventName2 = reader.GetString(reader.GetOrdinal("SetEventFinish2"));

                        SqliteCommand dbcmd2 = dbconn.CreateCommand();
                        string sqlQuery2 = "UPDATE " + "EventData" + " SET " + "EventState" + "=" + "'finish'" + " WHERE " + "EventName" + "=" + "'" + setEventName2 + "'";
                        dbcmd2.CommandText = sqlQuery2;
                        SqliteDataReader reader2 = dbcmd2.ExecuteReader();

                        print("set " + setEventName2 + " finish");

                        reader2.Close();
                        reader2 = null;

                        dbcmd2.Cancel();
                        dbcmd2.Dispose();
                        dbcmd2 = null;
                    }

                }
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }

    public void CheckSetEventStart(string eventName)
    {
        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * " + "FROM EventList";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("SetEventStart")) != "no")
                {
                    string setEventName = reader.GetString(reader.GetOrdinal("SetEventStart"));


                    SqliteCommand dbcmd1 = dbconn.CreateCommand();
                    string sqlQuery1 = "UPDATE " + "EventData" + " SET " + "EventState" + "=" + "'start'" + " WHERE " + "EventName" + "=" + "'" + setEventName + "'";
                    dbcmd1.CommandText = sqlQuery1;
                    SqliteDataReader reader1 = dbcmd1.ExecuteReader();

                    print("set " + setEventName + " start");

                    reader1.Close();
                    reader1 = null;

                    dbcmd1.Cancel();
                    dbcmd1.Dispose();
                    dbcmd1 = null;

                }
            }
        }
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;
    }


}
