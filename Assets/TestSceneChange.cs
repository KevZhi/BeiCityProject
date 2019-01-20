using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class TestSceneChange : MonoBehaviour {

    public GameManager gm;

    public string curScene;
    public string lastScene;
    //public bool hasChange;

    void Start () {
        gm = this.GetComponent<GameManager>();
        //curScene = "1.welcome";
        //lastScene = "1.welcome";
    }

	void Update () {
        curScene = SceneManager.GetActiveScene().name;
        if (File.Exists(Application.persistentDataPath + "/location.db"))
        {
            if (lastScene != curScene)
            {
                lastScene = curScene;

                gm.bgc.active = true;
                gm.bgmc.active = true;
                gm.soc.active = true;
                gm.dc.activeAtuo = true;
                gm.tc.active = true;
                //hasChange = true;
                //print("sceneChange!");
            }
            else
            {
                
                //hasChange = false;
                //print("sceneNotChange");
            }
        }
    }
}
