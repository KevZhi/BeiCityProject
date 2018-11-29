using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerState : MonoBehaviour {

    //private PlayerDataStructure playerData;

    //public int HP;
    //public int MP;
    //public int Level;
    //public int EXP;

    public int SkillPoint;

    public int ShamLV;
    public int ShamEXP;
    public int PassiveLV;
    public int PassiveEXP;
    public int RebelLV;
    public int RebelEXP;
    public int SelfishLV;
    public int SelfishEXP;
    public int EvilLV;
    public int EvilEXP;

    public Text ShamText;
    public Text PassiveText;
    public Text RebelText;
    public Text SelfishText;
    public Text EvilText;

    public GameObject warning;
    private float timer;


    private void Awake()
    {
        warning = GameObject.Find("commonMenu").transform.Find("warning").gameObject;
        //playerData = JsonUtility.FromJson<PlayerDataStructure>(File.ReadAllText(Application.dataPath + "\\playerData.json"));
    }

    void Start()
    {

        //HP = GlobalControl.Instance.HP;
        //MP = GlobalControl.Instance.MP;
        //EXP = playerData.Exp;

        //print(SceneManager.GetActiveScene().name);
    }
	
	// Update is called once per frame
	void Update () {

        ShamText.text = ShamLV.ToString();
        PassiveText.text = PassiveLV.ToString();
        RebelText.text = RebelLV.ToString();
        SelfishText.text = SelfishLV.ToString();
        EvilText.text = EvilLV.ToString();

        if (PassiveEXP == 2 && PassiveLV == 0)
        {
            
            if (timer>=1f)
            {
                PassiveLV++;
                warning.SetActive(false);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
                warning.SetActive(true);
                warning.GetComponentInChildren<Text>().text = "冷漠的等级提升了";
            }
        }
        if (EvilEXP == 2 && EvilLV == 0)
        {

            if (timer >= 1f)
            {
                EvilLV++;
                warning.SetActive(false);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
                warning.SetActive(true);
                warning.GetComponentInChildren<Text>().text = "邪恶的等级提升了";
            }
        }
    }

 

}
