using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EventDataManager : MonoBehaviour {

    [Header("=====默认0，已开始1，完成2=====")]
    public int event001 = 0;
    public int event002;
    public int event003;

    [Header("=====未完成0，已完成1=====")]
    public int desk01observed;
    public int desk02observed;
    public int desk03observed;

    public int mR02s0actived;
    public int mR02s1actived;
    public int wR03s1actived;
    public int sR02s1actived;
    public int wR03s2actived;
    public int sR05s1actived;
    public int wR02s1actived;
    public int sR07s1actived;

    [Header("=====不可以完成0，可以完成1=====")]
    public int event002canSubmit;
    [Header("=====未选择0，选择了1=====")]
    public int sR07Helped;

    [Header("=====物体生成旗标=====")]
    public bool created;
    public bool destroyed;

    private GameManager gm;
    private Transform roleParent;
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

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();

    }

    void Start () {
        roleParent = gm.roleRoot.transform;
        posParent = gm.posRoot.transform;
    }

	void Update ()
    {
        if (!destroyed)
        {
            if (SceneManager.GetActiveScene().name == "loading")
            {
                DestroyAll();
                destroyed = true;
                created = false;
            }
          
        }

        if (destroyed)
        {
            if (SceneManager.GetActiveScene().name != "loading")
            {
                DestroyAll();
                destroyed = false;
                created = false;
            }
        }


        if (!created && SceneManager.GetActiveScene().name != "loading")
        {
            created = true;
            destroyed = false;
            //场景物体
            //二层
            if (SceneManager.GetActiveScene().name == "floor2")
            {
                CreateRole("picture01", notice1Pos);

                CreatePosition("class15", position1Pos);
                CreatePosition("supportClass15", position2Pos);
                if (event002 == 2)
                {
                    CreatePosition("gate", position3Pos);
                }

                //事件
                if (event001 == 0)
                {
                    event001 = 1;
                    gm.dm.curName = "event001";
                    gm.dm.StartDialog();
                }
                if (event002 == 1)
                {
                    if (mR02s0actived == 0)
                    {
                        mR02s0actived = 1;
                        gm.dm.curName = "mR02s0";
                        gm.dm.StartDialog();
                    }
                    if (mR02s1actived == 0)
                    {
                        gm.im.canMove = false;
                        CreateRole("mR02s1", role1Pos);
                    }
                }          
                if (event003 == 1)
                {
                    if (sR05s1actived == 0)
                    {
                        CreateRole("sR05s1", role1Pos);
                    }
                }

            }
            //教辅室
            if (SceneManager.GetActiveScene().name == "supportClass15")
            {
                CreateRole("notice01", notice1Pos);
                CreateRole("desk01", thing1Pos);
                CreateRole("desk02", thing2Pos);

                CreatePosition("floor2", position1Pos);
            }
            //十五班
            if (SceneManager.GetActiveScene().name == "class15")
            {
                CreateRole("notice02", notice1Pos);
                CreateRole("myDesk", thing1Pos);

                CreatePosition("floor2", position1Pos);

                //事件
                if (event002==1)
                {
                    CreateRole("desk03", thing2Pos);
                    if (wR03s1actived == 0)
                    {
                        CreateRole("wR03s1", role1Pos);
                    }
                    if (wR03s1actived == 1)
                    {
                        CreateRole("wR03", role1Pos);
                    }
                    if (sR02s1actived == 0)
                    {
                        CreateRole("sR02s1", role2Pos);
                    }
                    if (sR02s1actived == 1)
                    {
                        CreateRole("sR02", role2Pos);
                    }

                }
                if (event003==1)
                {
                    if (event002canSubmit == 0)
                    {
                        CreateRole("wR03", role1Pos);
                    }
                    if (event002canSubmit == 1)
                    {
                        if (wR03s2actived == 0)
                        {
                            CreateRole("wR03s2", role1Pos);
                        }
                        if (wR03s2actived == 1)
                        {
                            CreateRole("wR03", role1Pos);
                        }
                    }
                    CreateRole("desk03", thing2Pos);
                    CreateRole("desk04", thing3Pos);
                }

            }
            //校门
            if (SceneManager.GetActiveScene().name == "gate")
            {
                CreateRole("notice03", notice1Pos);
                CreateRole("notice04", notice2Pos);

                CreatePosition("floor2", position1Pos);
                CreatePosition("dongming", position2Pos);

                if (event003==1)
                {
                    if (wR02s1actived == 0)
                    {
                        CreateRole("wR02s1", role1Pos);
                    }
                }
            }
            //冬明
            if (SceneManager.GetActiveScene().name == "dongming")
            {
                CreatePosition("gate", position1Pos);
                CreatePosition("street", position2Pos);

                CreateRole("shop01", notice1Pos);
                if (event003 == 1)
                {
                    if (sR07s1actived==0)
                    {
                        gm.dm.curName = "sR07s1";
                        gm.dm.StartDialog();
                    }
                }
            }
            //街道
            if (SceneManager.GetActiveScene().name == "street")
            {
                CreatePosition("dongming", position1Pos);
                CreatePosition("home", position2Pos);

                if (event003 == 1)
                {
                    CreateRole("sR08s1", role1Pos);
                }
            }

            if (event002 == 1)
            {
                gm.targetPanel.transform.Find("Text").gameObject.GetComponent<Text>().text = "回到自己的座位";
            }
            if (event003 == 1)
            {
                gm.targetPanel.transform.Find("Text").gameObject.GetComponent<Text>().text = "回到自己的家";
            }
        }
       


    }

    public GameObject CreateRole(string roleName,Vector3 pos)
    {
        GameObject prefeb = Resources.Load(roleName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);

        obj.name = roleName;

        obj.transform.SetParent(roleParent, false);
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

    public void DestroyAll()
    {
        int roleChildCount = roleParent.childCount;
        for (int i = 0; i < roleChildCount; i++)
        {
            Destroy(roleParent.GetChild(i).gameObject);
        }
        int posChildCount = posParent.childCount;
        for (int i = 0; i < posChildCount; i++)
        {
            Destroy(posParent.GetChild(i).gameObject);
        }
    }
}
