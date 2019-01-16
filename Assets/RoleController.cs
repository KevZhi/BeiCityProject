using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleController : MonoBehaviour {

    public GridLayoutGroup gridLayoutGroup;

    public float panelWith;
    public float panelHeight;

	// Use this for initialization
	void Start () {

        gridLayoutGroup = this.GetComponent<GridLayoutGroup>();
        //gridLayoutGroup.cellSize = new Vector2();
	}
	
	// Update is called once per frame
	void Update () {
   
    }

    private void OnGUI()
    {
        panelWith = Mathf.Abs(2*this.GetComponent<RectTransform>().rect.x);
        panelHeight = Mathf.Abs(2*this.GetComponent<RectTransform>().rect.y);
        gridLayoutGroup.padding.left = (int)(panelWith * 0.05f/2);
        gridLayoutGroup.padding.right = (int)(panelWith * 0.05f/2);
        gridLayoutGroup.spacing = new Vector2(panelWith * 0.05f/2, panelWith * 0.05f/2);
        gridLayoutGroup.cellSize = new Vector2(panelHeight * 0.8f, panelHeight * 0.8f);
    }
}
