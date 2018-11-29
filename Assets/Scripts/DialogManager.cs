using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Xml;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour {

    public bool startDialog = false;
    public GameObject dialogImage;
    public string curName;
    private GameManager gm;

    private LyricsStructure lyrics;
    private NoticeStructure notice;

    //这是场景中的各个物体
    public Image leftPos1;
    public Image leftPos2;
    public Image leftPos3;
    public Image centerPos;
    public Image rightPos1;
    public Image rightPos2;
    public Image rightPos3;
    public GameObject roleName;
    public GameObject detail;
    private List<string> dialogues_list;//存放dialogues的list
    private int dialogue_index = 0;//对话索引
    private int dialogue_count = 0;//对话数量

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }
    // Use this for initialization
    void Start()
    {
        lyrics = JsonUtility.FromJson<LyricsStructure>(File.ReadAllText(Application.dataPath + "\\Resources\\DataBase\\lyrics.json"));
        notice = JsonUtility.FromJson<NoticeStructure>(File.ReadAllText(Application.dataPath + "\\Resources\\DataBase\\notice.json"));
    }


    private float timer;
    private bool startTimer = false;

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

     
        //print(dialogue_index);
        if (startDialog)
        {
            gm.roleRoot.SetActive(false);
           gm.posRoot.SetActive(false);
           gm.sceneMask.SetActive(true);

            if (Input.GetMouseButtonDown(0)||Input.GetKeyDown(KeyCode.Space))
            //if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogue_index++;//对话跳到一下个
                if (dialogue_index < dialogue_count)//如果对话还没有完
                {
                    dialogues_handle(dialogue_index);//那就载入下一条对话
                }
                else
                { 
                    QuitDialog();
                }
            }
        }
    }


    private string role;//当前在说话的角色
    private string role_detail;//当前在说话的内容。
    private string portrait;
    private string rolePos;
    private string state;

    /*处理每一条对话的函数，就是将dialogues_list每一条对话弄到场景*/
    private void dialogues_handle(int dialogue_index)
    {
        //切割数组
        string[] role_detail_array = dialogues_list[dialogue_index].Split(',');
        //list中每一个对话格式就是“角色名,对话”
        role = role_detail_array[0];
        role_detail = role_detail_array[1];
        rolePos = role_detail_array[2];
        portrait = role_detail_array[3];
        state = role_detail_array[4];

        if (rolePos==leftPos1.name)
        {
            leftPos1.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePos == leftPos2.name)
        {
            leftPos2.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePos == leftPos3.name)
        {
            leftPos3.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePos == centerPos.name)
        {
            centerPos.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePos==rightPos1.name)
        {
            rightPos1.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePos == rightPos2.name)
        {
            rightPos2.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }
        if (rolePos == rightPos3.name)
        {
            rightPos3.sprite = Resources.Load("Pictures\\" + portrait, typeof(Sprite)) as Sprite;
        }


        roleName.GetComponent<Text>().text = role;
        detail.GetComponent<Text>().text = role_detail;//并加载当前的对话

        //图像
        if (state=="+left1")
        {
            leftPos1.gameObject.SetActive(true);
        }
        if (state == "-left1")
        {
            leftPos1.gameObject.SetActive(false);
        }
        if (state == "+left2")
        {
            leftPos2.gameObject.SetActive(true);
        }
        if (state == "-left2")
        {
            leftPos2.gameObject.SetActive(false);
        }
        if (state=="+right1")
        {
            rightPos1.gameObject.SetActive(true);
        }
        if (state == "+right2")
        {
            rightPos2.gameObject.SetActive(true);
        }
        if (state == "+right3")
        {
            rightPos3.gameObject.SetActive(true);
        }
        if (state == "-right3")
        {
            rightPos3.gameObject.SetActive(false);
        }
        if (state == "+center")
        {
            centerPos.gameObject.SetActive(true);
        }
        if (state == "-center")
        {
            centerPos.gameObject.SetActive(false);
        }
        if (state== "+right1+left2")
        {
            rightPos1.gameObject.SetActive(true);
            leftPos2.gameObject.SetActive(true);
        }
        if (state == "+left2-right1")
        {
            rightPos1.gameObject.SetActive(false);
            leftPos2.gameObject.SetActive(true);
        }
        if (state == "+center-right1")
        {
            rightPos1.gameObject.SetActive(false);
            centerPos.gameObject.SetActive(true);
        }
        if (state == "-center+right3")
        {
            rightPos3.gameObject.SetActive(true);
            centerPos.gameObject.SetActive(false);
        }
        if (state == "+center-right3")
        {
            rightPos3.gameObject.SetActive(false);
            centerPos.gameObject.SetActive(true);
        }
        if (state == "-left1-right3")
        {
            rightPos3.gameObject.SetActive(false);
            leftPos1.gameObject.SetActive(false);
        }
        if (state=="+black")
        {
            gm.black.SetActive(true);
        }
        if (state == "-black")
        {
            gm.black.SetActive(false);
        }

        //文章
        if (state== "+lyrics1")
        {
            gm.noticePanel.SetActive(true);
            gm.noticePanel.GetComponentInChildren<Text>().text = lyrics.Lyrics1; 
        }
        if (state == "+notice1")
        {
            gm.noticePanel.SetActive(true);
            gm.noticePanel.GetComponentInChildren<Text>().text = notice.Notice1;
        }

        //音频
        if (state== "+Firelink Shrine")
        {
            this.GetComponent<AudioManager>().audioName = "Firelink Shrine";
            this.GetComponent<AudioManager>().LoadAudio();
        }
        if (state == "+bell")
        {
            this.GetComponent<AudioManager>().audioName = "bell";
            this.GetComponent<AudioManager>().LoadAudioOnce();
        }

        //选项
        if (state== "AnswerOrNot")
        {
            startDialog = false;
            gm.AnswerOrNot.SetActive(true);
        }
        if (state == "HelpOrNot")
        {
            startDialog = false;
            gm.HelpOrNot.SetActive(true);
        }
        if (state== "ObserveOrNot")
        {
            startDialog = false;
            gm.ObserveOrNot.SetActive(true);
        }
        if (state == "ChatOrNot")
        {
            startDialog = false;
            gm.ChatOrNot.SetActive(true);
        }
        if (state == "SitOrNot")
        {
            if (gm.em.event002==1)
            {
                startDialog = false;
                gm.SitOrNot.SetActive(true);
            }
    
        }

        //移动
        if (state=="GoToClass15")
        {
            Globe.nextSceneName = "class15";
            SceneManager.LoadScene("loading");

        }
        if (state == "GoToSupportClass15")
        {
            Globe.nextSceneName = "supportClass15";
            SceneManager.LoadScene("loading");

        }

        //事件状态
        if (state=="finish"&& curName== "event001X1X1")
        {
            gm.em.event001 = 2;
            gm.em.event002 = 1;
        }
        if (state == "finish" && curName == "mR02s1")
        {
            gm.em.mR02s1actived = 1;
        }
        if (state == "finish" && curName == "wR03s1X1")
        {
            gm.em.wR03s1 = 0;
            gm.em.wR03s1actived = 1;
            Destroy(gm.roleRoot.transform.Find("wR03s1").gameObject);
        }
        if (state == "finish" && curName == "sR02s1X1")
        {
            gm.em.sR02s1 = 0;
            gm.em.sR02s1actived = 1;
            Destroy(gm.roleRoot.transform.Find("sR02s1").gameObject);
        }
        if (state == "finish" && curName == "event002A")
        {
            gm.em.event002 = 2;
            gm.em.event002canSubmit = 1;
        }
        if (state == "finish" && curName == "event002B")
        {
            gm.em.event002 = 2;
        }
        if (state == "finish" && curName == "event002X1")
        {
            gm.em.event003 = 1;
        }
        if (state == "finish" && curName == "wR03s2X1")
        {
            gm.em.wR03s2 = 0;
            gm.em.wR03s2actived = 1;
            Destroy(gm.roleRoot.transform.Find("wR03s2").gameObject);
        }
        if (state == "finish" && curName == "sR05s1")
        {
            gm.em.sR05s1actived = 1;
        }
        
        //权限
        if (state == "canotMove")
        {
            gm.im.canMove = false;
        }
        if (state=="canMove")
        {
            gm.im.canMove = true;
        }
    }

    public void StartDialog()
    {
        
        this.GetComponent<GameManager>().menuUI.SetActive(false);
       
        gm.targetPanel.SetActive(false);
        dialogImage.SetActive(true);
        this.GetComponent<InteractionManager>().active = false;

        XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器” 
        //xmlDocument = new XmlDocument();
        dialogues_list = new List<string>();//初始化存放dialogues的list
        //载入资源文件
        string data = Resources.Load("Text\\" + curName).ToString();
        //string data = Resources.Load("dialogues").ToString();
        //注意这里没有后缀名xml。你可以看看编辑器中，也是不带后缀的。因此不要有个同名的其它格式文件注意！
        //如果Resources下又有目录那就：Resources.Load("xx\\xx\\dialogues").ToString()

        xmlDocument.LoadXml(data);//载入这个xml  
        XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("dialogues").ChildNodes;//选择<dialogues>为根结点并得到旗下所有子节点  
        //xmlNodeList = xmlDocument.SelectSingleNode("dialogues").ChildNodes;
        foreach (XmlNode xmlNode in xmlNodeList)//遍历<dialogues>下的所有节点<dialogue>压入List
        {
            XmlElement xmlElement = (XmlElement)xmlNode;//对于任何一个元素，其实就是每一个<dialogue>  
            dialogues_list.Add(xmlElement.ChildNodes.Item(0).InnerText + ","
                + xmlElement.ChildNodes.Item(1).InnerText + ","
                + xmlElement.ChildNodes.Item(2).InnerText + ","
                + xmlElement.ChildNodes.Item(3).InnerText + ","
                + xmlElement.ChildNodes.Item(4).InnerText);
            //将角色名和对话内容存入这个list，中间存个逗号一会儿容易分割
        }
        dialogue_count = dialogues_list.Count;//获取到底有多少条对话
        dialogues_handle(0);//载入第一条对话的场景

       gm.roleRoot.SetActive(false);
       gm.posRoot.SetActive(false);
       gm.sceneMask.SetActive(true);

        startDialog = true;

    }

    public void QuitDialog()
    {

        this.GetComponent<GameManager>().menuUI.SetActive(true);
        gm.targetPanel.SetActive(true);
        gm.noticePanel.SetActive(false);

        dialogue_index = 0;
        dialogue_count = 0;

        leftPos1.gameObject.SetActive(false);
        leftPos2.gameObject.SetActive(false);
        leftPos3.gameObject.SetActive(false);
        centerPos.gameObject.SetActive(false); 
        rightPos1.gameObject.SetActive(false);
        rightPos2.gameObject.SetActive(false);
        rightPos3.gameObject.SetActive(false);

        dialogImage.SetActive(false);

        this.GetComponent<GameManager>().sceneMask.SetActive(false);

        startDialog = false;

        this.GetComponent<InteractionManager>().active = true;

        this.GetComponent<GameManager>().roleRoot.SetActive(true);
        this.GetComponent<GameManager>().posRoot.SetActive(true);

        if (curName=="event001A"||curName=="event001B")
        {
            curName = "event001X1";
            StartDialog();
        }

        if (curName == "event001X1A" || curName == "event001X1B")
        {
            curName = "event001X1X1";
            StartDialog();
        }

        if (curName == "event002A" || curName == "event002B")
        {
            curName = "event002X1";
            StartDialog();
        }

    }

    //选项
    public void Answer()
    {
        startDialog = true;
        if (curName=="event001")
        {
            
            dialogue_index = 0;
            dialogue_count = 0;
            curName = "event001A";
            StartDialog();
        }
        if (curName == "event002")
        {

            dialogue_index = 0;
            dialogue_count = 0;
            curName = "event002A";
            gm.ps.ShamEXP++;
            StartDialog();
        }

        gm.AnswerOrNot.SetActive(false);
    }

    public void NotAnswer()
    {
        startDialog = true;
        if (curName == "event001")
        {
            dialogue_index = 0;
            dialogue_count = 0;

            this.GetComponent<PlayerState>().PassiveEXP++;
            curName = "event001B";
            StartDialog();
        }
        if (curName == "event002")
        {
            dialogue_index = 0;
            dialogue_count = 0;

            gm.ps.RebelEXP++;
            curName = "event002B";
            StartDialog();
        }
        gm.AnswerOrNot.SetActive(false);
    }

    public void Help()
    {
        startDialog = true;
        if (curName == "event001X1")
        {

            dialogue_index = 0;
            dialogue_count = 0;
            curName = "event001X1A";
            StartDialog();
        }

        gm.HelpOrNot.SetActive(false);
    }

    public void NotHelp()
    {
        startDialog = true;
        if (curName == "event001X1")
        {
            dialogue_index = 0;
            dialogue_count = 0;

            this.GetComponent<PlayerState>().PassiveEXP++;
            curName = "event001X1B";
            StartDialog();
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
        QuitDialog();
        gm.ObserveOrNot.SetActive(false);
    }

    public void Chat()
    {
        startDialog = true;
        if (curName == "wR03s1")
        {

            dialogue_index = 0;
            dialogue_count = 0;
            curName = "wR03s1X1";
            StartDialog();
        }
        if (curName == "sR02s1")
        {

            dialogue_index = 0;
            dialogue_count = 0;
            curName = "sR02s1X1";
            StartDialog();
        }
        if (curName == "wR03s2")
        {

            dialogue_index = 0;
            dialogue_count = 0;
            curName = "wR03s2X1";
            StartDialog();
        }
        gm.ChatOrNot.SetActive(false);
    }

    public void NotChat()
    {
        startDialog = true;
        QuitDialog();
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

}
