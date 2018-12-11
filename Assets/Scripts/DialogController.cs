using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour {

    private GameManager gm;
    public bool next;
    public bool isloading;

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
                if (Input.GetMouseButtonDown(0))
                {
                    gm.dm.dialogue_index++;
                    if (gm.dm.dialogue_index < gm.dm.dialogue_count)
                    {
                        gm.dm.Dialogues_handle(gm.dm.dialogue_index);//那就载入下一条对话
                    }
                    else
                    {
                        gm.dm.QuitDialog();

                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    gm.im.active = true;
                }
            }
        }


        //if (gm.dm.curName == null )
        //{
        //    gm.dm.ResetRolePortrait();
        //    gm.dm.SetDialogUI(false);
        //}
    }
}
