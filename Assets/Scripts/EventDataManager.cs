using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventDataManager : MonoBehaviour {

    public int event001;
    public int event002;
    public int event002canSubmit;
    public int event003;

    public int desk01observed;
    public int desk02observed;

    public int mR02s1actived;
    public int wR03s1actived;
    public int sR02s1actived;
    public int wR03s2actived;
    public int sR05s1actived;
    public int wR02s1actived;

    [Header("=====非储存=====")]
    public int picture01;
    public int notice01;
    public int notice02;
    public int notice03;
    public int notice04;
    public int myDesk;
    public int desk01;
    public int desk02;
    public int desk03;//荀的
    public int desk04;//谢的

    public int Floor2ToClass15;
    public int Floor215ToSupportClass15;
    public int Floor2ToGate;
    public int SupportClass15ToFloor2;
    public int Class15ToFloor2;
    public int GateToFloor2;
    public int GateToDM;
    public int DMToGate;
    public int DMToStreet;
    public int StreetToDM;
    public int StreetToHome;

    public int mR02s1;
    public int wR03s1;
    public int sR02s1;
    public int sR05s1;
    public int wR03s2;
    public int wR02s1;

    private GameManager gm;
    private Transform eventParent;
    private Transform posParent;

    private Vector3 role1Pos = new Vector3(-6f, 4f, 0);
    private Vector3 role2Pos = new Vector3(-4f, 4f, 0);

    private Vector3 notice1Pos = new Vector3(-6f, 0, 0);
    private Vector3 notice2Pos = new Vector3(-4f, 0, 0);

    private Vector3 thing1Pos = new Vector3(-6f, -2f, 0);
    private Vector3 thing2Pos = new Vector3(-4f, -2f, 0);
    private Vector3 thing3Pos= new Vector3(-2f, -2f, 0);

    private Vector3 position1Pos = new Vector3(9.5f, 5.3f, 0);
    private Vector3 position2Pos = new Vector3(9.5f, 4f, 0);
    private Vector3 position3Pos = new Vector3(9.5f, 2.7f, 0);
    //private bool canCreate = true;
    private void Awake()
    {
        gm = this.GetComponent<GameManager>();

    }


    // Use this for initialization
    void Start () {
        eventParent = gm.roleRoot.transform;
        posParent = gm.posRoot.transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //常规物体
        //二层
        if (picture01==0 && SceneManager.GetActiveScene().name == "floor2")
        {
            CreateEvent("picture01", notice1Pos);
            picture01 = 1;
        }
        if (picture01 == 1 && SceneManager.GetActiveScene().name != "floor2")
        {
            Destroy(gm.roleRoot.transform.Find("picture01").gameObject);
            picture01 = 0;
        }
        //教辅室
        if (notice01 == 0 && SceneManager.GetActiveScene().name == "supportClass15")
        {
            CreateEvent("notice01", notice1Pos);
            notice01 = 1;
        }
        if (notice01 == 1 && SceneManager.GetActiveScene().name != "supportClass15")
        {
            Destroy(gm.roleRoot.transform.Find("notice01").gameObject);
            notice01 = 0;
        }
        if (desk01 == 0 && SceneManager.GetActiveScene().name == "supportClass15")
        {
            CreateEvent("desk01", thing1Pos);
            desk01 = 1;
        }
        if (desk01 == 1 && SceneManager.GetActiveScene().name != "supportClass15")
        {
            Destroy(gm.roleRoot.transform.Find("desk01").gameObject);
            desk01 = 0;
        }
        if (desk02 == 0 && SceneManager.GetActiveScene().name == "supportClass15")
        {
            CreateEvent("desk02", thing2Pos);
            desk02 = 1;
        }
        if (desk02 == 1 && SceneManager.GetActiveScene().name != "supportClass15")
        {
            Destroy(gm.roleRoot.transform.Find("desk02").gameObject);
            desk02 = 0;
        }
        //十五班
        if (notice02 == 0 && SceneManager.GetActiveScene().name == "class15")
        {
            CreateEvent("notice02", notice1Pos);
            notice02 = 1;
        }
        if (notice02 == 1 && SceneManager.GetActiveScene().name != "class15")
        {
            Destroy(gm.roleRoot.transform.Find("notice02").gameObject);
            notice02 = 0;
        }
        if (myDesk == 0 && SceneManager.GetActiveScene().name == "class15")
        {
            CreateEvent("myDesk", thing1Pos);
            myDesk = 1;
        }
        if (myDesk == 1 && SceneManager.GetActiveScene().name != "class15")
        {
            Destroy(gm.roleRoot.transform.Find("myDesk").gameObject);
            myDesk = 0;
        }
        //校门
        if (notice03 == 0 && SceneManager.GetActiveScene().name == "gate")
        {
            CreateEvent("notice03", notice1Pos);
            notice03 = 1;
        }
        if (notice03 == 1 && SceneManager.GetActiveScene().name != "gate")
        {
            Destroy(gm.roleRoot.transform.Find("notice03").gameObject);
            notice03 = 0;
        }
        if (notice04 == 0 && SceneManager.GetActiveScene().name == "gate")
        {
            CreateEvent("notice04", notice2Pos);
            notice04 = 1;
        }
        if (notice04 == 1 && SceneManager.GetActiveScene().name != "gate")
        {
            Destroy(gm.roleRoot.transform.Find("notice04").gameObject);
            notice04 = 0;
        }


        //常规可移动地点
        //二层
        if (Floor2ToClass15==0 && SceneManager.GetActiveScene().name == "floor2")
        {
            CreatePosition("class15", position1Pos);
            Floor2ToClass15 = 1;
        }
        if (Floor2ToClass15 == 1 && SceneManager.GetActiveScene().name != "floor2")
        {
            Destroy(gm.posRoot.transform.Find("class15").gameObject);
            Floor2ToClass15 = 0;
        }
        if (Floor215ToSupportClass15 == 0 && SceneManager.GetActiveScene().name == "floor2")
        {
            CreatePosition("supportClass15", position2Pos);
            Floor215ToSupportClass15 = 1;
        }
        if (Floor215ToSupportClass15 == 1 && SceneManager.GetActiveScene().name != "floor2")
        {
            Destroy(gm.posRoot.transform.Find("supportClass15").gameObject);
            Floor215ToSupportClass15 = 0;
        }
        //教辅室
        if (SupportClass15ToFloor2 == 0 && SceneManager.GetActiveScene().name == "supportClass15")
        {
            CreatePosition("floor2", position1Pos);
            SupportClass15ToFloor2 = 1;
        }
        if (SupportClass15ToFloor2 == 1 && SceneManager.GetActiveScene().name != "supportClass15")
        {
            Destroy(gm.posRoot.transform.Find("floor2").gameObject);
            SupportClass15ToFloor2 = 0;
        }
        //十五班
        if (Class15ToFloor2==0 && SceneManager.GetActiveScene().name=="class15")
        {
            CreatePosition("floor2", position1Pos);
            Class15ToFloor2 = 1;
        }
        if (Class15ToFloor2 == 1 && SceneManager.GetActiveScene().name != "class15")
        {
            Destroy(gm.posRoot.transform.Find("floor2").gameObject);
            Class15ToFloor2 = 0;
        }
        //校门
        if (GateToFloor2==0&& SceneManager.GetActiveScene().name == "gate")
        {
            CreatePosition("floor2", position1Pos);
            GateToFloor2 = 1;
        }
        if (GateToFloor2 == 1 && SceneManager.GetActiveScene().name != "gate")
        {
            Destroy(gm.posRoot.transform.Find("floor2").gameObject);
            GateToFloor2 = 0;
        }
        if (GateToDM == 0 && SceneManager.GetActiveScene().name == "gate")
        {
            CreatePosition("dongming", position2Pos);
            GateToDM = 1;
        }
        if (GateToDM == 1 && SceneManager.GetActiveScene().name != "gate")
        {
            Destroy(gm.posRoot.transform.Find("dongming").gameObject);
            GateToDM = 0;
        }
        //冬明
        if (DMToGate == 0&& SceneManager.GetActiveScene().name == "dongming")
        {
            CreatePosition("gate", position1Pos);
            DMToGate = 1;
        }
        if (DMToGate == 1 && SceneManager.GetActiveScene().name != "dongming")
        {
            Destroy(gm.posRoot.transform.Find("gate").gameObject);
            DMToGate = 0;
        }
        if (DMToStreet == 0 && SceneManager.GetActiveScene().name == "dongming")
        {
            CreatePosition("street", position2Pos);
            DMToStreet = 1;
        }
        if (DMToStreet == 1 && SceneManager.GetActiveScene().name != "dongming")
        {
            Destroy(gm.posRoot.transform.Find("street").gameObject);
            DMToStreet = 0;
        }
        //街道
        if (StreetToDM==0 && SceneManager.GetActiveScene().name == "street")
        {
            CreatePosition("dongming", position1Pos);
            StreetToDM = 1;
        }
        if (StreetToDM == 1 && SceneManager.GetActiveScene().name != "street")
        {
            Destroy(gm.posRoot.transform.Find("dongming").gameObject);
            StreetToDM = 0;
        }
        if (StreetToHome == 0 && SceneManager.GetActiveScene().name == "street")
        {
            CreatePosition("home", position2Pos);
            StreetToHome = 1;
        }
        if (StreetToHome == 1 && SceneManager.GetActiveScene().name != "street")
        {
            Destroy(gm.posRoot.transform.Find("home").gameObject);
            StreetToHome = 0;
        }
       
        //特定事件发生
        if (event001== 0)
        {
            this.GetComponent<DialogManager>().curName = "event001";
            this.GetComponent<DialogManager>().StartDialog();
            event001 = 1;
        }
        if (event002==1)
        {
            gm.targetPanel.transform.Find("Text").gameObject.GetComponent<Text>().text = "回到自己的座位";

            if (mR02s1 == 0 && SceneManager.GetActiveScene().name == "floor2")
            {
                if (mR02s1actived==0)
                {
                    CreateEvent("mR02s1", role1Pos);
                    gm.dm.curName = "mR02s0";
                    gm.dm.StartDialog();
                }
                mR02s1 = 1;
            }
            if (mR02s1 == 1 && SceneManager.GetActiveScene().name != "floor2")
            {
                if (mR02s1actived == 0)
                {
                    Destroy(gm.roleRoot.transform.Find("mR02s1").gameObject);
                }
                mR02s1 = 0;
            }

            if (wR03s1 == 0 && SceneManager.GetActiveScene().name == "class15")
            {
                if (wR03s1actived==0)
                {
                    CreateEvent("wR03s1", role1Pos);
                }
                if (wR03s1actived == 1)
                {
                    CreateEvent("wR03", role1Pos);
                }
                wR03s1 = 1;
            }
            if (wR03s1 == 1 && SceneManager.GetActiveScene().name != "class15")
            {
                if (wR03s1actived==0)
                {
                    Destroy(gm.roleRoot.transform.Find("wR03s1").gameObject);
                }
                if (wR03s1actived == 1)
                {
                    Destroy(gm.roleRoot.transform.Find("wR03").gameObject);
                }
                wR03s1 = 0;
            }

            if (sR02s1 == 0 && SceneManager.GetActiveScene().name == "class15")
            {
                if (sR02s1actived==0)
                {
                    CreateEvent("sR02s1", role2Pos);
                }
                if (sR02s1actived == 1)
                {
                    CreateEvent("sR02", role2Pos);
                }
                sR02s1 = 1;
            }
            if (sR02s1 == 1 && SceneManager.GetActiveScene().name != "class15")
            {
                if (sR02s1actived==0)
                {
                    Destroy(gm.roleRoot.transform.Find("sR02s1").gameObject);
                }
                if (sR02s1actived == 1)
                {
                    Destroy(gm.roleRoot.transform.Find("sR02").gameObject);
                }
                sR02s1 = 0;
            }

            if (desk03 == 0 && SceneManager.GetActiveScene().name == "class15")
            {
                CreateEvent("desk03", thing2Pos);
                desk03 = 1;
            }
            if (desk03 == 1 && SceneManager.GetActiveScene().name != "class15")
            {
                Destroy(gm.roleRoot.transform.Find("desk03").gameObject);
                desk03 = 0;
            }

        }
        //重载场景
        if (event002 == 2 && SceneManager.GetActiveScene().name == "class15")
        {
            int childCount = eventParent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(eventParent.GetChild(i).gameObject);
            }

            gm.targetPanel.transform.Find("Text").gameObject.GetComponent<Text>().text = "回到自己的家";
            notice02 = 0;
            myDesk = 0;
            desk03 = 0;
            event002 = 3;
        }

        if (event002 == 3 && event003==1)
        {
            gm.targetPanel.transform.Find("Text").gameObject.GetComponent<Text>().text = "回到自己的家";

            if (wR03s2 == 0 && SceneManager.GetActiveScene().name == "class15")
            {
                if (event002canSubmit==0)
                {
                    CreateEvent("wR03", role1Pos);
                }
                if (event002canSubmit == 1)
                {
                    if (wR03s2actived==0)
                    {
                        CreateEvent("wR03s2", role1Pos);
                    }
                    if (wR03s2actived == 1)
                    {
                        CreateEvent("wR03", role1Pos);
                    }
                }

                wR03s2 = 1;
            }
            if (wR03s2 == 1 && SceneManager.GetActiveScene().name != "class15")
            {
                if (event002canSubmit == 0)
                {
                    Destroy(gm.roleRoot.transform.Find("wR03").gameObject);
                }
                if (event002canSubmit == 1)
                {
                    if (wR03s2actived == 0)
                    {
                        Destroy(gm.roleRoot.transform.Find("wR03s2").gameObject);
                    }
                    if (wR03s2actived == 1)
                    {
                        Destroy(gm.roleRoot.transform.Find("wR03").gameObject);
                    }
                }
                wR03s2 = 0;
            }

            if (desk03 == 0 && SceneManager.GetActiveScene().name == "class15")
            {
                CreateEvent("desk03", thing2Pos);
                desk03 = 1;
            }
            if (desk03 == 1 && SceneManager.GetActiveScene().name != "class15")
            {
                Destroy(gm.roleRoot.transform.Find("desk03").gameObject);
                desk03 = 0;
            }

            if (desk04 == 0 && SceneManager.GetActiveScene().name == "class15")
            {
                CreateEvent("desk04", thing3Pos);
                desk04 = 1;
            }
            if (desk04 == 1 && SceneManager.GetActiveScene().name != "class15")
            {
                Destroy(gm.roleRoot.transform.Find("desk04").gameObject);
                desk04 = 0;
            }

            if (sR05s1 == 0 && SceneManager.GetActiveScene().name == "floor2")
            {
                if (sR05s1actived==0)
                {
                    CreateEvent("sR05s1", role1Pos);
                }
                sR05s1 = 1;
            }
            if (sR05s1 == 1 && SceneManager.GetActiveScene().name != "floor2")
            {
                if (sR05s1actived==0)
                {
                    Destroy(gm.roleRoot.transform.Find("sR05s1").gameObject);
                }
                sR05s1 = 0;
            }

            if (Floor2ToGate == 0 && SceneManager.GetActiveScene().name == "floor2")
            {
                CreatePosition("gate", position3Pos);
                Floor2ToGate = 1;
            }
            if (Floor2ToGate == 1 && SceneManager.GetActiveScene().name != "floor2")
            {
                Destroy(gm.posRoot.transform.Find("gate").gameObject);
                Floor2ToGate = 0;
            }

            if (wR02s1 == 0 && SceneManager.GetActiveScene().name == "gate")
            {
                if (wR02s1actived == 0)
                {
                    CreateEvent("wR02s1", role1Pos);
                }
                wR02s1 = 1;
            }
            if (wR02s1 == 1 && SceneManager.GetActiveScene().name != "gate")
            {
                if (wR03s2actived == 0)
                {
                    Destroy(gm.roleRoot.transform.Find("wR02s1").gameObject);
                }
                wR02s1 = 0;
            }


        }
    }

    public GameObject CreateEvent(string eventName, Vector3 pos)
    {
        GameObject prefeb = Resources.Load(eventName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);

        obj.name = eventName;

        obj.transform.SetParent(eventParent, false);
        obj.transform.localPosition = pos; 
        //obj.transform.localRotation = Quaternion.identity;

        return obj;
    }

    public GameObject CreatePosition(string positionName, Vector3 pos)
    {
        GameObject prefeb = Resources.Load(positionName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);

        obj.name = positionName;

        obj.transform.SetParent(posParent, false);
        obj.transform.localPosition = pos;
        //obj.transform.localRotation = Quaternion.identity;

        return obj;
    }
}
