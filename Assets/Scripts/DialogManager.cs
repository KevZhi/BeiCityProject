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

    public GameObject portraitImage;
    public Image leftPos1;
    public Image leftPos2;
    //public Image leftPos3;
    public Image centerPos;
    public Image rightPos1;
    public Image rightPos2;
    //public Image rightPos3;

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
    }

    void Update()
    {
        if (startTimer)
        {
            if (timer>=1f)
            {
                timer = 0;
                startTimer = false;
            }
            else
            {
                timer += Time.deltaTime;
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
        gm.dc.next = true;
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
        gm.objRoot.SetActive(false);
        gm.mm.ShowOrHideGameMenuBtn(false);

        gm.im.active = false;

        gm.sm.destroyed = true;
        gm.sm.checkInDialogStart = true;

        SetDialogUI(true);
        //gm.dc.next = true;
    }

    public void QuitDialog()
    {
       /* gm.dc.next = false;*///姑且放在这吧，我要被搞死了
        gm.mm.ShowOrHideGameMenuBtn(true);
        gm.sm.destroyed = true;
        gm.sm.checkInDialogFinish = true;
        ResetRolePortrait();
        SetDialogUI(false);
        gm.objRoot.SetActive(true);
        gm.dc.next = false;
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
        //if (rolePosition == leftPos3.name)
        //{
        //    leftPos3.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        //}
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
        //if (rolePosition == rightPos3.name)
        //{
        //    rightPos3.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        //}

    }
    /// <summary>
    /// 使角色图片全部关闭
    /// </summary>
    public void ResetRolePortrait()
    {
  
        int ChildCount = allPortrait.childCount;
        for (int i = 0; i < ChildCount; i++)
        {
            allPortrait.GetChild(i).gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// 对话/非对话状态切换时的UI控制
    /// </summary>
    /// <param name="isopen">对话状态</param>
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
            StartDialog();
        }
        else
        {
            QuitDialog();
        }
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
        //if (stateDetail == "+left3")
        //{
        //    leftPos3.gameObject.SetActive(true);
        //}
        //if (stateDetail == "-left3")
        //{
        //    leftPos3.gameObject.SetActive(false);
        //}
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
        //if (stateDetail == "+right3")
        //{
        //    rightPos3.gameObject.SetActive(true);
        //}
        //if (stateDetail == "-right3")
        //{
        //    rightPos3.gameObject.SetActive(false);
        //}
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
        //if (stateDetail == "-center+right3")
        //{
        //    rightPos3.gameObject.SetActive(true);
        //    centerPos.gameObject.SetActive(false);
        //}
        //if (stateDetail == "+center-right3")
        //{
        //    rightPos3.gameObject.SetActive(false);
        //    centerPos.gameObject.SetActive(true);
        //}
        //if (stateDetail == "-left1-right3")
        //{
        //    rightPos3.gameObject.SetActive(false);
        //    leftPos1.gameObject.SetActive(false);
        //}
        if (stateDetail == "+black")
        {
            //gm.black.SetActive(true);
        }
        if (stateDetail == "-black")
        {
            //gm.black.SetActive(false);
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
            gm.mm.AnswerOrNot.SetActive(true);
        }
        if (stateDetail == "HelpOrNot")
        {
            gm.dc.next = false;
            gm.mm.HelpOrNot.SetActive(true);
        }
        if (stateDetail == "ObserveOrNot")
        {
            gm.dc.next = false;
            gm.mm.ObserveOrNot.SetActive(true);
        }
        if (stateDetail == "ChatOrNot")
        {
            gm.dc.next = false;
            gm.mm.ChatOrNot.SetActive(true);
        }
        if (stateDetail == "SitOrNot")
        {
            gm.dc.next = false;
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
