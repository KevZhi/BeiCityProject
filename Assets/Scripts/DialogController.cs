using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {

    private GameManager gm;
    public bool next;

    public bool isloading;

    //public bool quitDialog;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (gm.sceneName == "loading")
        {
            isloading = true;
        }
        else
        {
            isloading = false;
        }

        if (!isloading)
        {
            if (next)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.LeftControl))
                {
                    gm.dm.dialogue_index++;
                    if (gm.dm.dialogue_index < gm.dm.dialogue_count)
                    {
                        gm.dm.Dialogues_handle(gm.dm.dialogue_index);//那就载入下一条对话
                    }
                    else
                    {
                        gm.edc.active = true;
                        //quitDialog = true;
                        gm.dm.QuitDialog();
                        gm.dm.TryToLoadNextEvent(gm.dm.curName);
                    }
                }
            }
        }

        if (gm.testScene.hasChange)
        {
            gm.dm.TryToAutoHappendEvent();
        }
    }
}
