using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mono.Data.Sqlite;

public class SceneObjectController : MonoBehaviour {

    public GameObject upperPanel;
    public GameObject middlePanel;
    public GameObject lowerPanel;

    public float panelWith;
    public float panelHeight;

    public GridLayoutGroup up;
    public GridLayoutGroup mp;
    public GridLayoutGroup lp;

    public bool creat;
    // Use this for initialization
    void Start () {
        up = upperPanel.GetComponent<GridLayoutGroup>();
        mp = middlePanel.GetComponent<GridLayoutGroup>();
        lp = lowerPanel.GetComponent<GridLayoutGroup>();
	}
	
	// Update is called once per frame
	void Update () {
        if (creat)
        {
            creat = false;
            CreateObj("obj");
        }
	}

    private void OnGUI()
    {
        panelWith = Mathf.Abs(2 * upperPanel.GetComponent<RectTransform>().rect.x);
        panelHeight = Mathf.Abs(2 * upperPanel.GetComponent<RectTransform>().rect.y);

        up.padding.left = (int)(panelWith * 0.05f / 2);
        up.padding.right = (int)(panelWith * 0.05f / 2);
        up.spacing = new Vector2(panelWith * 0.05f / 2, panelWith * 0.05f / 2);
        up.cellSize = new Vector2(panelHeight * 0.8f, panelHeight * 0.8f);

        mp.padding.left = (int)(panelWith * 0.05f / 2);
        mp.padding.right = (int)(panelWith * 0.05f / 2);
        mp.spacing = new Vector2(panelWith * 0.05f / 2, panelWith * 0.05f / 2);
        mp.cellSize = new Vector2(panelHeight * 0.8f, panelHeight * 0.8f);

        lp.padding.left = (int)(panelWith * 0.05f / 2);
        lp.padding.right = (int)(panelWith * 0.05f / 2);
        lp.spacing = new Vector2(panelWith * 0.05f / 2, panelWith * 0.05f / 2);
        lp.cellSize = new Vector2(panelHeight * 0.8f, panelHeight * 0.8f);
    }

    public GameObject CreateObj(string objName)
    {
        GameObject prefeb = Resources.Load(objName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);

        obj.name = objName;

        obj.transform.SetParent(upperPanel.transform, false);

        return obj;
    }
}
