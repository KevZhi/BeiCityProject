using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour {

    //单例
    private static GameManager1 _instance;
    public static GameManager1 Instance
    {
        get
        {
            return _instance;
        }
    }

    //玩家属性
    public int gold;
    public int favor;
    public int leftDay;

    public Text goldText;
    public Text favorText;
    public Text dateText;

    public GameObject actionBtn;

    public GameObject talkLine;
    public Text talkLineText;

    //天黑天亮属性
    public Image mask;
    public bool toAntherDay;
    public bool toBeDay;
    private float timeVal;

    //工作
    public GameObject workBtn;
    public Sprite[] workSprites;
    public Image workImage;
    public GameObject workUI;

    private void Awake()
    {
        _instance = this;
        gold = favor = 0;
        leftDay = 20;
        UpdateUI();
    }
    // Update is called once per frame
    void Update () {
        //是否过度到另外一天
        if (toAntherDay)
        {
            if (toBeDay)
            {
                //天亮
                if (timeVal >= 2)
                {
                    timeVal = 0;
                    ToDay();
                }
                else
                {
                    timeVal += Time.deltaTime;
                }

            }
            else
            {
                //天黑
                ToDark();
            }
        }
	}

    //即将天黑
    public void ToBeDark()
    {
        toAntherDay = true;
    }

    //天黑
    public void ToDark()
    {
        mask.color += new Color(0, 0, 0, Mathf.Lerp(0, 1, 0.1f));
        if (mask.color.a>=0.8f)
        {
            mask.color = new Color(0, 0, 0, 1);
            toBeDay = true;
            ResetUI();
            UpdateUI();
        }
    }

    //天亮
    public void ToDay()
    {
        mask.color -= new Color(0, 0, 0, Mathf.Lerp(1, 0, 0.1f));
        if (mask.color.a<=0.2f)
        {
            mask.color = new Color(0, 0, 0, 0);
            toAntherDay = false;
            toBeDay = false;
        }
    }



    //打工
    public void ClickWorkBtn()
    {
        actionBtn.SetActive(false);
        workBtn.SetActive(true);

    }

    public void GetMoney(int workIndex)
    {
        workBtn.SetActive(false);
        ChangeGold((4 - workIndex) * 20);
        workImage.sprite = workSprites[workIndex];
        workUI.SetActive(true);
        talkLine.SetActive(true);
        talkLineText.text = "+"+ ((4 - workIndex) * 20).ToString()+"gold";
    }

    public void ClickChatBtn()
    {
        actionBtn.SetActive(false);
    }


    //更新玩家属性UI显示
    private void UpdateUI()
    {
        goldText.text = gold.ToString();
        favorText.text = favor.ToString();
        dateText.text = leftDay.ToString();
    }
    //金币数额变换方法
    public void ChangeGold(int goldValue)
    {
        gold += goldValue;
        if (gold <= 0)
        {
            gold = 0;
        }
        UpdateUI();
    }
    //好感度数额变换方法
    public void ChangeFavor(int favorValue)
    {
        favor += favorValue;
        if (favor<=0)
        {
            favor = 0;
        }
        UpdateUI();
    }

    //重置所有UI
    private void ResetUI()
    {
        workUI.SetActive(false);
        talkLine.SetActive(false);
        actionBtn.SetActive(true);
        leftDay--;
    }

}
