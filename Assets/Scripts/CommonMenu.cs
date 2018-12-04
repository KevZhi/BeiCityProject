using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommonMenu : MonoBehaviour {

    public static CommonMenu Instance;

    private GameManager gm;

    public bool check;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gm = GameObject.Find("Player").GetComponent<GameManager>();
        gm.black.SetActive(true);
        gm.begin.SetActive(true);
    }

    private void Update()
    {

        if (!check)
        {
            if (SceneManager.GetActiveScene().name == "1.welcome")
            {
                check = true;
                this.transform.Find("titleMenu").gameObject.SetActive(true);
                gm.menuUI.SetActive(false);

                gm.ps.SkillPoint = 0;
                gm.ps.ShamLV = 0;
                gm.ps.ShamEXP = 0;
                gm.ps.PassiveLV = 0;
                gm.ps.PassiveEXP = 0;
                gm.ps.RebelLV = 0;
                gm.ps.RebelEXP = 0;
                gm.ps.SelfishLV = 0;
                gm.ps.SelfishEXP = 0;
                gm.ps.EvilLV = 0;
                gm.ps.EvilEXP = 0;

                gm.em.event001 = 0;
                gm.em.event002 = 0;
                gm.em.event003 = 0;

                gm.em.desk01observed = 0;
                gm.em.desk02observed = 0;
                gm.em.desk03observed = 0;

                gm.em.mR02s0actived = 0;
                gm.em.mR02s1actived = 0;
                gm.em.wR03s1actived = 0;
                gm.em.sR02s1actived = 0;
                gm.em.wR03s2actived = 0;
                gm.em.sR05s1actived = 0;
                gm.em.wR02s1actived = 0;
                gm.em.sR07s1actived = 0;

                gm.em.event002canSubmit = 0;

                gm.em.sR07Helped = 0;
            }
         
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "1.welcome")
            {
                check = false;
                this.transform.Find("titleMenu").gameObject.SetActive(false);
                gm.menuUI.SetActive(true);
            }
        }
    }
}
