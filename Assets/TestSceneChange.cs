using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TestSceneChange : MonoBehaviour {

    public string curScene;
    public string lastScene;
    public bool hasChange;
    // Use this for initialization
    void Start () {
        //curScene = "1.welcome";
        //lastScene = "1.welcome";
    }
	
	// Update is called once per frame
	void Update () {
        curScene = SceneManager.GetActiveScene().name;
        if (lastScene != curScene)
        {
            lastScene = curScene;
            hasChange = true;
            //print("sceneChange!");
        }
        else
        {
            hasChange = false;
            //print("sceneNotChange");
        }
    }
}
