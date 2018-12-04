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

    public bool startDialog = false;
    public string curName;

    //private LyricsStructure lyrics;
    //private NoticeStructure notice;

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
    private int dialogue_index = 0;//对话索引
    private int dialogue_count = 0;//对话数量

    private bool startTimer = false;
    private float timer;

    private Transform allPortrait;

    private string xmlPath;

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
        //lyrics = JsonUtility.FromJson<LyricsStructure>(File.ReadAllText(Application.dataPath + "/Resources/DataBase/lyrics.json"));
        //notice = JsonUtility.FromJson<NoticeStructure>(File.ReadAllText(Application.dataPath + "/Resources/DataBase/notice.json"));
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

        if (startDialog)
        {
            if (Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Space))
            {
                dialogue_index++;
                if (dialogue_index < dialogue_count)//如果对话还没有完
                {
                    Dialogues_handle(dialogue_index);//那就载入下一条对话
                }
                else
                { 
                    QuitDialog();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                gm.im.active = true;
            }
        }
    }

    private void Dialogues_handle(int dialogue_index)
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
        string data = Resources.Load("Text\\" + curName).ToString();
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

        startDialog = true;

        Dialogues_handle(0);

        gm.roleRoot.SetActive(false);
        gm.posRoot.SetActive(false);
        gm.sceneMask.SetActive(true);
       
        gm.im.active = false;
    }

    public void QuitDialog()
    {
       
        dialogImage.SetActive(false);
        gm.menuUI.SetActive(true);
        gm.sceneMask.SetActive(false);
        gm.targetPanel.SetActive(true);
        gm.noticePanel.SetActive(false);
        gm.roleRoot.SetActive(true);
        gm.posRoot.SetActive(true);
        ResetRolePortrait();
        startDialog = false;
   
        CheckEventName(curName);

        gm.em.destroyed = true;
        //gm.im.active = true;
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
        startDialog = true;
        curName = curName + "Answer";
        xmlPath = Application.dataPath + "/Resources/Text/" + curName + ".xml";
        if (File.Exists(xmlPath))
        {
            StartDialog();
        }
        gm.AnswerOrNot.SetActive(false);
    }

    public void NotAnswer()
    {
        startDialog = true;
        curName = curName + "NotAnswer";
        xmlPath = Application.dataPath + "/Resources/Text/" + curName + ".xml";
        if (File.Exists(xmlPath))
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
        startDialog = true;
        curName = curName + "Help";
        xmlPath = Application.dataPath + "/Resources/Text/"+ curName + ".xml";
        if (File.Exists(xmlPath))
        {
            StartDialog();
        }
        gm.HelpOrNot.SetActive(false);
    }

    public void NotHelp()
    {
        startDialog = true;
        curName = curName + "NotHelp";
        xmlPath = Application.dataPath + "/Resources/Text/" + curName + ".xml";
        if (File.Exists(xmlPath))
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
        startDialog = true;
        if (curName == "desk01")
        {

            dialogue_index = 0;
            dialogue_count = 0;

            curName = "desk01nothing";
            StartDialog();
            if (gm.em.desk01observed==0)
            {
                gm.em.desk01observed = 1;
                gm.ps.EvilEXP++;
            }

        }
        if (curName == "desk02")
        {

            dialogue_index = 0;
            dialogue_count = 0;

            curName = "desk02lyrics1";
            StartDialog();
            if (gm.em.desk02observed == 0)
            {
                gm.em.desk02observed = 1;
                gm.ps.EvilEXP++;
            }
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
        startDialog = true;
        curName = curName + "NotObserve";
        xmlPath = Application.dataPath + "/Resources/Text/" + curName + ".xml";
        if (File.Exists(xmlPath))
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
        startDialog = true;
        dialogue_index = 0;
        dialogue_count = 0;
        curName = curName + "Chat";
        StartDialog();
        gm.ChatOrNot.SetActive(false);
    }

    public void NotChat()
    {
        startDialog = true;
        curName = curName + "NotChat";
        xmlPath = Application.dataPath + "/Resources/Text/" + curName + ".xml";
        if (File.Exists(xmlPath))
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
        startDialog = true;
        if (gm.em.event002==1)
        {
            dialogue_index = 0;
            dialogue_count = 0;
            curName = "event002";
            StartDialog();
        }
        gm.SitOrNot.SetActive(false);
    }

    public void NotSit()
    {
        startDialog = true;
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
        //if (stateDetail == "+lyrics1")
        //{
        //    gm.noticePanel.SetActive(true);
        //    gm.noticePanel.GetComponentInChildren<Text>().text = lyrics.Lyrics1;
        //}
        //if (stateDetail == "+notice1")
        //{
        //    gm.noticePanel.SetActive(true);
        //    gm.noticePanel.GetComponentInChildren<Text>().text = notice.Notice1;
        //}

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
            startDialog = false;
            gm.AnswerOrNot.SetActive(true);
        }
        if (stateDetail == "HelpOrNot")
        {
            startDialog = false;
            gm.HelpOrNot.SetActive(true);
        }
        if (stateDetail == "ObserveOrNot")
        {
            startDialog = false;
            gm.ObserveOrNot.SetActive(true);
        }
        if (stateDetail == "ChatOrNot")
        {
            startDialog = false;
            gm.ChatOrNot.SetActive(true);
        }
        if (stateDetail == "SitOrNot")
        {
            if (gm.em.event002 == 1)
            {
                startDialog = false;
                gm.SitOrNot.SetActive(true);
            }

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

    public void CheckEventName(string eventName)
    {
        /* event001 */
        
        //不回答荀
        if (eventName == "event001NotAnswer")
        {
            gm.ps.PassiveEXP++;
        }
        if (eventName == "event001Answer" || eventName == "event001NotAnswer")
        {
            curName = "event001X";
            StartDialog();
        }
        //不回答谢
        if (eventName == "event001XNotHelp")
        {
            gm.ps.PassiveEXP++;
        }
        if (eventName == "event001XHelp" || eventName == "event001XNotHelp")
        {
            curName = "event001XX";
            StartDialog();
        }
        if (eventName == "event001XX")
        {
            gm.em.event001 = 2;
            gm.em.event002 = 1;
        }
        //返回途中与钟对话
        if (eventName == "mR02s1")
        {
            gm.im.canMove = true;
            gm.em.mR02s1actived = 1;
        }
        //教室内与高对话
        if (eventName == "wR03s1Chat")
        {
            gm.em.wR03s1actived = 1;
        }
        //教室内与谢对话
        if (eventName == "sR02s1Chat")
        {
            gm.em.sR02s1actived = 1;
        }
       
        /* event002 */
        
        //答应高
        if (eventName == "event002Answer")
        {
            gm.em.event002canSubmit = 1;
            gm.ps.ShamEXP++;
        }
        //拒绝高
        if (eventName == "event002NotAnswer")
        {
            gm.ps.RebelEXP++;
        }
        if (eventName == "event002Answer" || eventName == "event002NotAnswer")
        {

            curName = "event002X";
            StartDialog();
        }
        if (eventName == "event002X")
        {
            gm.em.event002 = 2;
            gm.em.event003 = 1;
        }
        //对高提交
        if (eventName == "wR03s2Chat")
        {
            gm.em.wR03s2actived = 1;
        }
        //杜、程对话
        if (eventName == "sR05s1")
        {
            gm.em.sR05s1actived = 1;
        }
        //与欧阳对话
        if (eventName == "wR02s1Chat")
        {
            gm.em.wR02s1actived = 1;
        }
        //帮助乞丐
        if (eventName == "sR07s1Help")
        {
            gm.em.sR07s1actived = 1;
            gm.em.sR07Helped = 1;
        }
        //不帮助乞丐
        if (eventName == "sR07s1NotHelp")
        {
            gm.em.sR07s1actived = 1;
            gm.ps.PassiveEXP++;
        }
        
        /* event003 */

    }
}
