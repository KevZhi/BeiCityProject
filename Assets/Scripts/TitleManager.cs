using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour {

    public GameObject startUI;
    public GameObject loadUI;
    public GameObject optionUI;
    public GameObject savePanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CallSavePanel()
    {
        startUI.SetActive(false);
        loadUI.SetActive(false);
        optionUI.SetActive(false);
        savePanel.SetActive(true);
    }

    public void CancelSavePanel()
    {
        startUI.SetActive(true);
        loadUI.SetActive(true);
        optionUI.SetActive(true);
        savePanel.SetActive(false);
    }
}
