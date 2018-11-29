using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCasterManager : MonoBehaviour {

    public string eventName;
    public bool active = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active==false)
        {
            Destroy(gameObject);
        }
	}
}
