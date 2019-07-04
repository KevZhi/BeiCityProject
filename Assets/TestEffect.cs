using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestEffect : MonoBehaviour {

    public RectTransform goswitch;

    public float speed;

    public float width;
    public bool active;
    public bool reset;
    public bool go;

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {

        if (reset)
        {
            reset = false;
            goswitch.anchoredPosition = new Vector2(-2*width, 0);
            goswitch.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width*2);
        }

        if (active)
        {
            FadeToClear();
            if (Input.GetMouseButtonDown(0))
            {
                goswitch.anchoredPosition = new Vector2(2*width, 0);
            }

            if (goswitch.anchoredPosition == new Vector2(2 * width, 0))
            {
                active = false;
            }

        }

    }

    public void FadeToClear()
    {
        goswitch.anchoredPosition = Vector2.Lerp(goswitch.anchoredPosition, new Vector2(2*width, 0), speed * Time.deltaTime);
    }

    private void OnGUI()
    {
        width = Screen.width;
    }
}
