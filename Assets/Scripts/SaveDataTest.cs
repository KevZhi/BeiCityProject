using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveDataTest : MonoBehaviour {

    private PlayerDataStructure playerData;
    private bool isExist;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        isExist = File.Exists(Application.persistentDataPath + "/" + this.name + ".json");
        if (isExist == true)
        {
            playerData = JsonUtility.FromJson<PlayerDataStructure>(File.ReadAllText(Application.persistentDataPath + "/" + this.name + ".json"));
            this.GetComponentInChildren<Text>().text = playerData.SavedText;
        }
        else
        {
            this.GetComponentInChildren<Text>().text = "新的存档";
        }
        
    }
}
