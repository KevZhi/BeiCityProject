using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour {

    public bool active;
    public bool canMove;
    public GameManager gm;
    private string nextStage;

    // Use this for initialization
    void Awake () {
        gm = this.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            //事件触发
            if (Input.GetMouseButtonDown(0) && active == true)
            {
                if (hit.collider.tag == "Normal" && hit.collider.GetComponent<EventCasterManager>().active == true)
                {
                    gm.dm.curName = hit.collider.GetComponent<EventCasterManager>().eventName;
                    gm.dm.StartDialog();
                }
                if (hit.collider.tag == "Once" && hit.collider.GetComponent<EventCasterManager>().active == true)
                {
                    gm.dm.curName = hit.collider.GetComponent<EventCasterManager>().eventName;
                    gm.dm.StartDialog();
                    hit.collider.GetComponent<EventCasterManager>().active = false;
                }
            }
            //移动触发
            if (Input.GetMouseButtonDown(0) && canMove == true)
            {
                if (hit.collider.tag == "MoveBtn")
                {
                    nextStage = hit.collider.gameObject.name;
                    LoadScene();
                }
            }
            //不可移动
            if (Input.GetMouseButtonDown(0) && canMove == false && gm.em.mR02s1==1)
            {
                if (hit.collider.tag == "MoveBtn")
                {
                    gm.dm.curName = "mR02s1canotMove";
                    gm.dm.StartDialog();
                }
            }
        }
    }

    public void LoadScene()
    {
        Globe.nextSceneName = nextStage;
        SceneManager.LoadScene("loading");
    }

}
