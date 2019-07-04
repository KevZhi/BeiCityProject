using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

public class SceneObjectController : MonoBehaviour {

    public GameManager gm;

    public GameObject upperPanel;
    public GameObject middlePanel;
    public GameObject lowerPanel;
    public GameObject positionPanel;

    public float panelWith;
    public float panelHeight;

    public float positionWith;
    public float positionHeight;

    public GridLayoutGroup up;
    public GridLayoutGroup mp;
    public GridLayoutGroup lp;
    public GridLayoutGroup pp;

    public bool active;
    // Use this for initialization
    void Start () {
        gm = this.GetComponent<GameManager>();
        up = upperPanel.GetComponent<GridLayoutGroup>();
        mp = middlePanel.GetComponent<GridLayoutGroup>();
        lp = lowerPanel.GetComponent<GridLayoutGroup>();
        pp = positionPanel.GetComponent<GridLayoutGroup>();
	}
	
	// Update is called once per frame
	void Update () {

        if (active)
        {
            active = false;

            DestroyAll();
            LoadSceneObj();
            //print("create!");
        }
	}

    private void OnGUI()
    {
        panelWith = Mathf.Abs(2 * upperPanel.GetComponent<RectTransform>().rect.x);
        panelHeight = Mathf.Abs(2 * upperPanel.GetComponent<RectTransform>().rect.y);

        positionWith = Mathf.Abs(2 * positionPanel.GetComponent<RectTransform>().rect.x);
        positionHeight = Mathf.Abs(2 * positionPanel.GetComponent<RectTransform>().rect.y);

        up.padding.left = (int)(panelWith * 0.05f / 2);
        up.padding.right = (int)(panelWith * 0.05f / 2);
        up.spacing = new Vector2(panelWith * 0.05f / 2, panelWith * 0.05f / 2);
        up.cellSize = new Vector2(panelHeight * 0.8f, panelHeight * 0.8f);

        mp.padding.left = (int)(panelWith * 0.05f / 2);
        mp.padding.right = (int)(panelWith * 0.05f / 2);
        mp.spacing = new Vector2(panelWith * 0.05f / 2, panelWith * 0.05f / 2);
        mp.cellSize = new Vector2(panelHeight * 0.8f, panelHeight * 0.8f);

        lp.padding.left = (int)(panelWith * 0.05f / 2);
        lp.padding.right = (int)(panelWith * 0.05f / 2);
        lp.spacing = new Vector2(panelWith * 0.05f / 2, panelWith * 0.05f / 2);
        lp.cellSize = new Vector2(panelHeight * 0.8f, panelHeight * 0.8f);

        pp.padding.top = (int)(positionHeight * 0.05f / 2);
        pp.padding.bottom = (int)(positionHeight * 0.05f / 2);
        pp.spacing = new Vector2(positionHeight * 0.01f, positionHeight * 0.01f);
        pp.cellSize = new Vector2(positionWith * 0.9f, positionHeight * 0.15f);
    }

    public void LoadSceneObj()
    {
        string conn = "data source= " + Application.streamingAssetsPath + "/sqlite4unity.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT * FROM SceneObjList WHERE scene = " + "'" + gm.testScene.curScene + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();
        while (reader.Read())
        {
            
            string ReadyEventName = reader.GetString(reader.GetOrdinal("NeedEventReady"));
            string FinishEventName = reader.GetString(reader.GetOrdinal("NeedEventFinish"));
            string FinishEventName2 = reader.GetString(reader.GetOrdinal("NeedEvent2Finish"));
            string StartEventName = reader.GetString(reader.GetOrdinal("NeedEventStart"));
            string StartEventName2 = reader.GetString(reader.GetOrdinal("NeedEvent2Start"));

            bool isready,isfinish,isfinish2,isstart,isstart2; 

            if (ReadyEventName =="no")
            {
                isready = true;
            }
            else
            {
                isready = IsTrue(ReadyEventName, "ready");
            }

            if (FinishEventName == "no")
            {
                isfinish = true;
            }
            else
            {
                isfinish = IsTrue(FinishEventName, "finish");
            }

            if (FinishEventName2 == "no")
            {
                isfinish2 = true;
            }
            else
            {
                isfinish2 = IsTrue(FinishEventName2, "finish");
            }

            if (StartEventName == "no")
            {
                isstart = true;
            }
            else
            {
                isstart = IsTrue(StartEventName, "start");
            }

            if (StartEventName2 == "no")
            {
                isstart2 = true;
            }
            else
            {
                isstart2 = IsTrue(StartEventName2, "start");
            }

            if (isready && isfinish && isfinish2 && isstart && isstart2)
            {
                CreateObj(reader.GetString(reader.GetOrdinal("icon")), reader.GetString(reader.GetOrdinal("type")), reader.GetString(reader.GetOrdinal("name")));
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

    public bool IsTrue(string NeedCheckEventName, string NeedCheckEventState)
    {
        string conn = "data source= " + Application.persistentDataPath + "/location.db";
        SqliteConnection dbconn = new SqliteConnection(conn);
        dbconn.Open();

        SqliteCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT EventState FROM EventData WHERE EventName = " + "'" + NeedCheckEventName + "'";
        dbcmd.CommandText = sqlQuery;
        SqliteDataReader reader = dbcmd.ExecuteReader();

        string str = reader["EventState"].ToString();
        //print(str);
        bool istrue = (str == NeedCheckEventState);
        //if (NeedCheckEventName != "no")
        //{
        //    //print(NeedCheckEventName + "'s " + NeedCheckEventState + " state is " + istrue);
        //}
        reader.Close();
        reader = null;

        dbcmd.Cancel();
        dbcmd.Dispose();
        dbcmd = null;

        dbconn.Close();
        dbconn.Dispose();
        dbconn = null;

        return istrue;
    }

    public GameObject CreateObj(string iconName,string type,string name)
    {
        //print("create!");
        GameObject prefeb = Resources.Load("Obj") as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);
        obj.name = name;
        Image icon = obj.GetComponent<Image>();
        icon.sprite = Resources.Load("Icon\\" + iconName, typeof(Sprite)) as Sprite;
        Button button = obj.GetComponent<Button>();
        if (type == "1")
        {
            obj.transform.SetParent(upperPanel.transform, false);
            button.onClick.AddListener(delegate () {
                LoadObjDetail(name);
            });
        }
        if (type == "2")
        {
            obj.transform.SetParent(middlePanel.transform, false);
            button.onClick.AddListener(delegate () {
                LoadObjDetail(name);
            });
        }
        if (type == "3")
        {
            obj.transform.SetParent(lowerPanel.transform, false);
            button.onClick.AddListener(delegate () {
                LoadObjDetail(name);
            });
        }
        if (type == "4")
        {
            obj.transform.SetParent(positionPanel.transform, false);

            button.onClick.AddListener(delegate () {
                LoadScene(name);
            });
        }
       

        return obj;
    }

    public void LoadScene(string scene)
    {
        Globe.nextSceneName = scene;
        SceneManager.LoadScene("loading");
    }

    public void LoadObjDetail(string obj)
    {
        gm.dm.curName = obj;
        gm.dc.activeStart = true;

    }

    public void DestroyAll()
    {
        //print("destroy!");
        int upperChildCount = upperPanel.transform.childCount;
        for (int i = 0; i < upperChildCount; i++)
        {
            Destroy(upperPanel.transform.GetChild(i).gameObject);
        }

        int middleChildCount = middlePanel.transform.childCount;
        for (int i = 0; i < middleChildCount; i++)
        {
            Destroy(middlePanel.transform.GetChild(i).gameObject);
        }

        int lowerChildCount = lowerPanel.transform.childCount;
        for (int i = 0; i < lowerChildCount; i++)
        {
            Destroy(lowerPanel.transform.GetChild(i).gameObject);
        }

        int positionChildCount = positionPanel.transform.childCount;
        for (int i = 0; i < positionChildCount; i++)
        {
            Destroy(positionPanel.transform.GetChild(i).gameObject);
        }

    }

}
