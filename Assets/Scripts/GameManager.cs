using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Xml;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private PlayerDataStructure playerData;
    private EventDataStructure eventData;

    public GameObject wantToSave;
    public GameObject wantToLoad;
    public GameObject saveComplete;
    public GameObject wantToLeave;

    //public GameObject panelMask;

    private string nowSceneName;
    private string tempText;

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

    public GameObject objRoot;
    public GameObject roleRoot;
    public GameObject posRoot;
    public GameObject sceneMask;
    public PlayerState ps;
    public EventDataManager em;
    public DialogManager dm;
    public InteractionManager im;

    //private float timer = 0;

    private void Awake()
    {

        objRoot = GameObject.Find("objRoot");
        sceneMask = objRoot.transform.Find("sceneMask").gameObject;
        roleRoot = objRoot.transform.Find("roleRoot").gameObject;
        posRoot = objRoot.transform.Find("posRoot").gameObject;

        ps = this.GetComponent<PlayerState>();
        em = this.GetComponent<EventDataManager>();
        dm = this.GetComponent<DialogManager>();
        im = this.GetComponent<InteractionManager>();
    }

    private void Update()
    {
        NowScene();
        tempText = "地点：" + nowSceneName
            + "\n存档时间：" + System.DateTime.Now;
    }


    public void SavePlayerData()
    {
        playerData = new PlayerDataStructure(
            SceneManager.GetActiveScene().name, 
            ps.SkillPoint,
            tempText,
            ps.ShamLV,
            ps.ShamEXP,
            ps.PassiveLV,
            ps.PassiveEXP,
            ps.RebelLV,
            ps.RebelEXP,
            ps.SelfishLV,
            ps.SelfishEXP,
            ps.EvilLV,
            ps.EvilEXP
            );

        eventData = new EventDataStructure(
        em.event001,
        em.event002,
        em.event003,

        em.desk01observed,
        em.desk02observed,
        em.desk03observed,

        em.mR02s0actived,
        em.mR02s1actived,
        em.wR03s1actived,
        em.sR02s1actived,
        em.wR03s2actived,
        em.sR05s1actived,
        em.wR02s1actived,
        em.sR07s1actived,

        em.event002canSubmit,

        em.sR07Helped);

        File.WriteAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json", JsonUtility.ToJson(playerData));
        File.WriteAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + "event.json", JsonUtility.ToJson(eventData));

        saveComplete.SetActive(true);
        saveComplete.GetComponentInChildren<Text>().text = "存档完成";
    }

    public void LoadPlayerData()
    {
        isExist = File.Exists(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json");

        if (isExist)
        {
            playerData = JsonUtility.FromJson<PlayerDataStructure>(File.ReadAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + ".json"));
            eventData = JsonUtility.FromJson<EventDataStructure>(File.ReadAllText(Application.persistentDataPath + "/" + EventSystem.current.currentSelectedGameObject.name + "event.json"));

            ps.SkillPoint = playerData.SkillPoint;
            ps.ShamLV = playerData.ShamLV;
            ps.ShamEXP = playerData.ShamEXP;
            ps.PassiveLV = playerData.PassiveLV;
            ps.PassiveEXP = playerData.PassiveEXP;
            ps.RebelLV = playerData.RebelLV;
            ps.RebelEXP = playerData.RebelEXP;
            ps.SelfishLV = playerData.SelfishLV;
            ps.SelfishEXP = playerData.SelfishEXP;
            ps.EvilLV = playerData.EvilLV;
            ps.EvilEXP = playerData.EvilEXP;

            em.event001 = eventData.event001;
            em.event002 = eventData.event002;
            em.event003 = eventData.event003;

            em.desk01observed = eventData.desk01observed;
            em.desk02observed = eventData.desk02observed;
            em.desk03observed = eventData.desk03observed;

            em.mR02s0actived = eventData.mR02s0actived;
            em.mR02s1actived = eventData.mR02s1actived;
            em.wR03s1actived = eventData.wR03s1actived;
            em.sR02s1actived = eventData.sR02s1actived;
            em.wR03s2actived = eventData.wR03s2actived;
            em.sR05s1actived = eventData.sR05s1actived;
            em.wR02s1actived = eventData.wR02s1actived;
            em.sR07s1actived = eventData.sR07s1actived;

            em.event002canSubmit = eventData.event002canSubmit;

            em.sR07Helped = eventData.sR07Helped;

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

        roleRoot.SetActive(false);
        posRoot.SetActive(false);

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

        roleRoot.SetActive(true);
        posRoot.SetActive(true);

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

    public void NowScene()
    {
        if (SceneManager.GetActiveScene().name =="class15")
        {
            nowSceneName = "十五班教室";
        }
        if (SceneManager.GetActiveScene().name == "floor2")
        {
            nowSceneName = "二楼走廊";
        }
        if (SceneManager.GetActiveScene().name == "supportClass15")
        {
            nowSceneName = "教辅室";
        }
        if (SceneManager.GetActiveScene().name == "gate")
        {
            nowSceneName = "校门";
        }
        if (SceneManager.GetActiveScene().name == "dongming")
        {
            nowSceneName = "冬明";
        }
        if (SceneManager.GetActiveScene().name == "street")
        {
            nowSceneName = "街道";
        }
    }
}
