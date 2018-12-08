using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Xml;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour {

    private GameManager gm;

    public string curName;

    private LyricsStructure lyrics;
    private NoticeStructure notice;

    public GameObject portraitImage;
    public Image leftPos1;
    public Image leftPos2;
    public Image leftPos3;
    public Image centerPos;
    public Image rightPos1;
    public Image rightPos2;
    public Image rightPos3;

    public GameObject dialogImage;
    public GameObject roleName;
    public GameObject detail;

    private List<string> dialogues_list;//存放dialogues的list
    public int dialogue_index;//对话索引
    public int dialogue_count;//对话数量

    private bool startTimer = false;
    private float timer;

    private Transform allPortrait;

    //private string xmlPath;

    private string role;
    private string role_detail;
    private string portrait;
    private string rolePos;
    private string state;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }
    // Use this for initialization
    void Start()
    {
        allPortrait = portraitImage.transform;
        lyrics = JsonUtility.FromJson<LyricsStructure>(Resources.Load("DataBase/lyrics").ToString());
        notice = JsonUtility.FromJson<NoticeStructure>(Resources.Load("DataBase/notice").ToString());
    }

    void Update()
    {
        if (startTimer)
        {
            if (timer>=1f)
            {
                timer = 0;
                gm.ps.warning.SetActive(false);
                startTimer = false;
            }
            else
            {
                timer += Time.deltaTime;
                gm.ps.warning.SetActive(true);
                if (curName == "desk03")
                {
                    gm.ps.warning.GetComponentInChildren<Text>().text = "邪恶的等级不足";
                }
                if (curName == "desk04")
                {
                    gm.ps.warning.GetComponentInChildren<Text>().text = "邪恶的等级不足";
                }
            }
        }
    }

    public void Dialogues_handle(int dialogue_index)
    {
        string[] role_detail_array = dialogues_list[dialogue_index].Split(',');

        role = role_detail_array[0];
        role_detail = role_detail_array[1];
        rolePos = role_detail_array[2];
        portrait = role_detail_array[3];
        state = role_detail_array[4];

        roleName.GetComponent<Text>().text = role;
        detail.GetComponent<Text>().text = role_detail;

        LoadRolePortrait(rolePos);
        CheckState(state);

    }

    public void StartDialog()
    {
        dialogue_index = 0;
        dialogue_count = 0;

        gm.menuUI.SetActive(false);
        gm.targetPanel.SetActive(false);
        dialogImage.SetActive(true);
        portraitImage.SetActive(true);
        
        XmlDocument xmlDocument = new XmlDocument();
        dialogues_list = new List<string>();
        string data = Resources.Load("Text/" + curName).ToString();
        xmlDocument.LoadXml(data); 
        XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("dialogues").ChildNodes;
        foreach (XmlNode xmlNode in xmlNodeList)
        {
            XmlElement xmlElement = (XmlElement)xmlNode;
            dialogues_list.Add(xmlElement.ChildNodes.Item(0).InnerText + ","
                + xmlElement.ChildNodes.Item(1).InnerText + ","
                + xmlElement.ChildNodes.Item(2).InnerText + ","
                + xmlElement.ChildNodes.Item(3).InnerText + ","
                + xmlElement.ChildNodes.Item(4).InnerText);
        }
        dialogue_count = dialogues_list.Count;

        Dialogues_handle(0);

        gm.dc.next = true;

        gm.objRoot.SetActive(false);
        gm.sceneMask.SetActive(true);
       
        gm.im.active = false;
        //print("start");
        //print(curName);
        gm.sm.destroyed = true;
        gm.sm.checkInDialogStart = true;
     
    }

    public void QuitDialog()
    {
       
        dialogImage.SetActive(false);
        gm.menuUI.SetActive(true);
        gm.sceneMask.SetActive(false);
        gm.targetPanel.SetActive(true);
        gm.noticePanel.SetActive(false);

        gm.objRoot.SetActive(true);
        ResetRolePortrait();

        gm.dc.next = false;//姑且放在这吧，我要被搞死了

        gm.sm.destroyed = true;
        gm.sm.checkInDialogFinish = true;
        //print("finish");
        //print(curName);

    }

    public void LoadRolePortrait(string rolePosition)
    {
        if (rolePosition == leftPos1.name)
        {
            leftPos1.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePosition == leftPos2.name)
        {
            leftPos2.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePosition == leftPos3.name)
        {
            leftPos3.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePosition == centerPos.name)
        {
            centerPos.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePosition == rightPos1.name)
        {
            rightPos1.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePosition == rightPos2.name)
        {
            rightPos2.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePosition == rightPos3.name)
        {
            rightPos3.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }

    }

    public void ResetRolePortrait()
    {
        portraitImage.SetActive(false);
        int ChildCount = allPortrait.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            allPortrait.GetChild(i).gameObject.SetActive(false);
        }
    }
    //选项
    public void Answer()
    {
        curName = curName + "Answer";
        print(Resources.Load("Text/" + curName));
        if (Resources.Load("Text/" + curName))
        {
            StartDialog();
        }
        gm.AnswerOrNot.SetActive(false);
    }

    public void NotAnswer()
    {
     
        curName = curName + "NotAnswer";
        if (Resources.Load("Text/" + curName))
        {
            StartDialog();
        }
        else
        {
            QuitDialog();
        }
        gm.AnswerOrNot.SetActive(false);
    }

    public void Help()
    {
      
        curName = curName + "Help";
      
        if (Resources.Load("Text/" + curName))
        {
            StartDialog();
        }
        gm.HelpOrNot.SetActive(false);
    }

    public void NotHelp()
    {
      
        curName = curName + "NotHelp";
        //print(Resources.Load("Text/" + curName));
        if (Resources.Load("Text/" + curName))
        {
            StartDialog();
        }
        else
        {
            QuitDialog();
        }
        gm.HelpOrNot.SetActive(false);
    }

    public void Observe()
    {
  
        if (curName == "desk01")
        {

            dialogue_index = 0;
            dialogue_count = 0;

            curName = "desk01nothing";
            StartDialog();
            //if (gm.em.desk01observed==0)
            //{
            //    gm.em.desk01observed = 1;
            //    gm.ps.EvilEXP++;
            //}

        }
        if (curName == "desk02")
        {

            dialogue_index = 0;
            dialogue_count = 0;

            curName = "desk02lyrics1";
            StartDialog();
            //if (gm.em.desk02observed == 0)
            //{
            //    gm.em.desk02observed = 1;
            //    gm.ps.EvilEXP++;
            //}
        }
        if (curName == "desk03")
        {
            if (gm.ps.EvilLV < 2)
            {
                startTimer = true;
                dialogue_index = 0;
                dialogue_count = 0;
                QuitDialog();
            }
            else
            {
                //调查
            }
        }
        if (curName == "desk04")
        {
            if (gm.ps.EvilLV < 2)
            {
                startTimer = true;
                dialogue_index = 0;
                dialogue_count = 0;
                QuitDialog();
            }
            else
            {
                //调查
            }
        }

        gm.ObserveOrNot.SetActive(false);
    }

    public void NotObserve()
    {
   
        curName = curName + "NotObserve";
        
        if (Resources.Load("Text/" + curName))
        {
            StartDialog();
        }
        else
        {
            QuitDialog();
        }
        gm.ObserveOrNot.SetActive(false);
    }

    public void Chat()
    {

        dialogue_index = 0;
        dialogue_count = 0;
        curName = curName + "Chat";
        StartDialog();
        gm.ChatOrNot.SetActive(false);
    }

    public void NotChat()
    {

        curName = curName + "NotChat";
        //print(Resources.Load("Text/" + curName));
        if (Resources.Load("Text/" + curName))
        {
            StartDialog();
        }
        else
        {
            QuitDialog();
        }
        gm.ChatOrNot.SetActive(false);
    }

    public void Sit()
    {
  
        //if (gm.em.event002==1)
        //{
        //    dialogue_index = 0;
        //    dialogue_count = 0;
        //    curName = "event002";
        //    StartDialog();
        //}
        gm.SitOrNot.SetActive(false);
    }

    public void NotSit()
    {
        QuitDialog();
        gm.SitOrNot.SetActive(false);
    }

    public void CheckState(string stateDetail)
    {
        //图像
        if (stateDetail == "+left1")
        {
            leftPos1.gameObject.SetActive(true);
        }
        if (stateDetail == "-left1")
        {
            leftPos1.gameObject.SetActive(false);
        }
        if (stateDetail == "+left2")
        {
            leftPos2.gameObject.SetActive(true);
        }
        if (stateDetail == "-left2")
        {
            leftPos2.gameObject.SetActive(false);
        }
        if (stateDetail == "+left3")
        {
            leftPos3.gameObject.SetActive(true);
        }
        if (stateDetail == "-left3")
        {
            leftPos3.gameObject.SetActive(false);
        }
        if (stateDetail == "+right1")
        {
            rightPos1.gameObject.SetActive(true);
        }
        if (stateDetail == "-right1")
        {
            rightPos1.gameObject.SetActive(false);
        }
        if (stateDetail == "+right2")
        {
            rightPos2.gameObject.SetActive(true);
        }
        if (stateDetail == "-right2")
        {
            rightPos2.gameObject.SetActive(false);
        }
        if (stateDetail == "+right3")
        {
            rightPos3.gameObject.SetActive(true);
        }
        if (stateDetail == "-right3")
        {
            rightPos3.gameObject.SetActive(false);
        }
        if (stateDetail == "+center")
        {
            centerPos.gameObject.SetActive(true);
        }
        if (stateDetail == "-center")
        {
            centerPos.gameObject.SetActive(false);
        }

        if (stateDetail == "+left2+right1")
        {
            rightPos1.gameObject.SetActive(true);
            leftPos2.gameObject.SetActive(true);
        }
        if (stateDetail == "+left2-right1")
        {
            rightPos1.gameObject.SetActive(false);
            leftPos2.gameObject.SetActive(true);
        }
        if (stateDetail == "+center-right1")
        {
            rightPos1.gameObject.SetActive(false);
            centerPos.gameObject.SetActive(true);
        }
        if (stateDetail == "-center+right3")
        {
            rightPos3.gameObject.SetActive(true);
            centerPos.gameObject.SetActive(false);
        }
        if (stateDetail == "+center-right3")
        {
            rightPos3.gameObject.SetActive(false);
            centerPos.gameObject.SetActive(true);
        }
        if (stateDetail == "-left1-right3")
        {
            rightPos3.gameObject.SetActive(false);
            leftPos1.gameObject.SetActive(false);
        }
        if (stateDetail == "+black")
        {
            gm.black.SetActive(true);
        }
        if (stateDetail == "-black")
        {
            gm.black.SetActive(false);
        }

        //文章
        if (stateDetail == "+lyrics1")
        {
            gm.noticePanel.SetActive(true);
            gm.noticePanel.GetComponentInChildren<Text>().text = lyrics.Lyrics1;
        }
        if (stateDetail == "+notice1")
        {
            gm.noticePanel.SetActive(true);
            gm.noticePanel.GetComponentInChildren<Text>().text = notice.Notice1;
        }

        //音频
        if (stateDetail == "+Firelink Shrine")
        {
            this.GetComponent<AudioManager>().audioName = "Firelink Shrine";
            this.GetComponent<AudioManager>().LoadAudio();
        }
        if (stateDetail == "+bell")
        {
            this.GetComponent<AudioManager>().audioName = "bell";
            this.GetComponent<AudioManager>().LoadAudioOnce();
        }

        //选项
        if (stateDetail == "AnswerOrNot")
        {
            gm.dc.next = false;
            gm.AnswerOrNot.SetActive(true);
        }
        if (stateDetail == "HelpOrNot")
        {
            gm.dc.next = false;
            gm.HelpOrNot.SetActive(true);
        }
        if (stateDetail == "ObserveOrNot")
        {
            gm.dc.next = false;
            gm.ObserveOrNot.SetActive(true);
        }
        if (stateDetail == "ChatOrNot")
        {
            gm.dc.next = false;
            gm.ChatOrNot.SetActive(true);
        }
        if (stateDetail == "SitOrNot")
        {
            gm.dc.next = false;
            //if (gm.em.event002 == 1)
            //{
            //    startDialog = false;
            //    gm.SitOrNot.SetActive(true);
            //}

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

        //权限
        if (stateDetail == "canotMove")
        {
            gm.im.canMove = false;
        }
        if (stateDetail == "canMove")
        {
            gm.im.canMove = true;
        }
    }
}
