using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using Mono.Data.Sqlite;

public class CommonMenu : MonoBehaviour {

    public static CommonMenu Instance;

    private GameManager gm;

    public bool check;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gm = GameObject.Find("Player").GetComponent<GameManager>();
        gm.black.SetActive(true);
        gm.begin.SetActive(true);

       
    }

    private void Update()
    {

        if (!check)
        {
            if (SceneManager.GetActiveScene().name == "1.welcome")
            {
                LoadSQL();
                check = true;
                this.transform.Find("titleMenu").gameObject.SetActive(true);
                gm.menuUI.SetActive(false);

                gm.showMenuBtn.SetActive(true);
                gm.saveBtn.SetActive(false);
                gm.loadBtn.SetActive(false);
                gm.stateBtn.SetActive(false);
                gm.optionBtn.SetActive(false);
                gm.titleBtn.SetActive(false);
                gm.closeMenuBtn.SetActive(false);

                gm.dm.curName = null;
            }
         
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "1.welcome")
            {
                check = false;
                this.transform.Find("titleMenu").gameObject.SetActive(false);
                gm.menuUI.SetActive(true);
            }
        }
    }
    public void LoadSQL()
    {

        string appDBPath = Application.persistentDataPath + "/location.db";

        WWW loadDB = new WWW(Application.streamingAssetsPath + "/sqlite4unity.db");

        File.WriteAllBytes(appDBPath, loadDB.bytes);
    }
}
