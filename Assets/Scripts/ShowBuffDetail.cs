using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBuffDetail : MonoBehaviour {

    private float timer = 0;

    private GameObject detail;
    private bool active=false;

    private void Awake()
    {
        detail = this.transform.Find("detail").gameObject;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            detail.SetActive(true);
            timer += Time.deltaTime;
            if (timer>=1f)
            {
                detail.SetActive(false);
                timer = 0;
                active = false;
            }
        }
	}

    public void BuffDetail()
    {
        active = true;
        //if (timer >= 1f)
        //{
        //    detail.SetActive(false);
        //    timer = 0;
        //}
        //else
        //{
        //    detail.SetActive(true);
        //    timer += Time.deltaTime;
        //}
    }
}
