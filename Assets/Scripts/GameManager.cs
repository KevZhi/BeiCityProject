using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public string sceneName;

    private PlayerDataStructure playerData;
    private EventDataStructure eventData;

    public GameObject wantToSave;
    public GameObject wantToLoad;
    public GameObject saveComplete;
    public GameObject wantToLeave;

    //private string nowSceneName;
    //private string tempText;

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

    public GameObject objRoot;

    public PlayerState ps;
    public DialogManager dm;
    public DialogController dc;
    public InteractionManager im;
    public SceneObjManager sm;
    public SQLiteEventManager SQLem;

    private void Awake()
    {

        objRoot = GameObject.Find("objRoot");

        ps = this.GetComponent<PlayerState>();

        dm = this.GetComponent<DialogManager>();
        im = this.GetComponent<InteractionManager>();

        SQLem = this.GetComponent<SQLiteEventManager>();
        dc = this.GetComponent<DialogController>();
        sm = this.GetComponent<SceneObjManager>();
    }

    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
        //NowScene();
        //tempText = "地点：" + nowSceneName
        //    + "\n存档时间：" + System.DateTime.Now;
    }


    public void SavePlayerData()
    {
        File.WriteAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json", JsonUtility.ToJson(playerData));
        File.WriteAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + "event.json", JsonUtility.ToJson(eventData));

        saveComplete.SetActive(true);
        saveComplete.GetComponentInChildren<Text>().text = "存档完成";
    }

    public void LoadPlayerData()
    {
        SQLem.SetAllDefalut();
        isExist = File.Exists(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json");

        if (isExist)
        {
            playerData = JsonUtility.FromJson<PlayerDataStructure>(File.ReadAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json"));
            eventData = JsonUtility.FromJson<EventDataStructure>(File.ReadAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + "event.json"));

            Globe.nextSceneName = playerData.CurScene;
            SceneManager.LoadScene("loading");

            CancelLoadPanel();
            titleMenu.SetActive(false);
            loadInTitlePanel.SetActive(false);
            CloseMenu();
        }
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
        SQLem.ShowPlayerState();
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
        SQLem.SetAllDefalut();
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

    //public void NowScene()
    //{
    //    if (SceneManager.GetActiveScene().name =="class15")
    //    {
    //        nowSceneName = "十五班教室";
    //    }
    //    if (SceneManager.GetActiveScene().name == "floor2")
    //    {
    //        nowSceneName = "二楼走廊";
    //    }
    //    if (SceneManager.GetActiveScene().name == "supportClass15")
    //    {
    //        nowSceneName = "教辅室";
    //    }
    //    if (SceneManager.GetActiveScene().name == "gate")
    //    {
    //        nowSceneName = "校门";
    //    }
    //    if (SceneManager.GetActiveScene().name == "dongming")
    //    {
    //        nowSceneName = "冬明";
    //    }
    //    if (SceneManager.GetActiveScene().name == "street")
    //    {
    //        nowSceneName = "街道";
    //    }
    //}
}
