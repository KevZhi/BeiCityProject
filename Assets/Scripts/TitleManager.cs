using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

    public bool check;

    private GameManager gm;


    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //LoadSQL();
        if (File.Exists(Application.persistentDataPath + "/location.db"))
        {
            File.Delete(Application.persistentDataPath + "/location.db");
        }

    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        if (!check)
        {
            if (SceneManager.GetActiveScene().name == "1.welcome")
            {
                LoadSQL();
                check = true;

                gm.mm.ShowOrHideTitleMenu(true);
                gm.mm.ShowOrHideTitleMenuChild(true);
                gm.mm.ShowOrHideGameMenu(false);
                gm.mm.ShowOrHideGameMenuBtn(false);
                gm.mm.maskUI.SetActive(false);

                gm.dm.curName = null;

                gm.objRoot.SetActive(false);
                //gm.sm.checkInDialogFinish = true;
            }

        }
        else
        {
            if (SceneManager.GetActiveScene().name != "1.welcome")
            {
                check = false;
                gm.mm.ShowOrHideTitleMenu(false);
                gm.mm.ShowOrHideTitleMenuChild(false);
                gm.mm.ShowOrHideGameMenuBtn(true);
                gm.mm.ShowOrHideGameMenu(false);

                //gm.sm.checkInDialogFinish = true;
            }
        }
    }

    public void LoadSQL()
    {
        //print("copy");
        string appDBPath = Application.persistentDataPath + "/location.db";

        //WWW loadDB = new WWW("file://" + Application.streamingAssetsPath + "/sqlite4unity.db");
        WWW loadDB = new WWW(Application.streamingAssetsPath + "/original.db");

        File.WriteAllBytes(appDBPath, loadDB.bytes);
    }

    //public IEnumerator LoadSQL()
    //{
    //    //用www先从unity中下载数据库
    //    //Application.streamingAssetsPath

    //    print("copy");

    //    string appDBPath = Application.persistentDataPath + "/location.db";
    //    WWW loadDB = new WWW("file://" + Application.streamingAssetsPath + "/sqlite4unity.db");
    //    yield return loadDB;
    //    //拷贝数据库
    //    File.WriteAllBytes(appDBPath, loadDB.bytes);
    //}
}
