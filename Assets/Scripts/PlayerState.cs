using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerState : MonoBehaviour {

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

    public bool LVup;
    private float timer;

    //private GameManager gm;

    private void Awake()
    {
        warning = GameObject.Find("commonMenu").transform.Find("warning").gameObject;
        //gm = this.GetComponent<GameManager>();
    }

    void Start()
    {

    }

	void Update () {

        if (LVup)
        {

            if (timer >= 1f)
            {
                LVup = false;
                timer = 0;
                warning.SetActive(false);

            }
            else
            {
                timer += Time.deltaTime;
                warning.SetActive(true);
                warning.GetComponentInChildren<Text>().text = "等级提升了";
            }
        }
    }

  
 

}
