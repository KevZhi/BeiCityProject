using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectController : MonoBehaviour {

    public GameManager gm;

    [Header("====表示时间流逝=====")]
    public RectTransform goswitch;
    public float gospeed = 1;
    public float screenwidth;
    public bool goactive;
    public bool goreset;

    [Header("=====淡出=====")]
    public Image black;
    public bool fadeoutreset;
    public bool fadeout;
    public float timerout;
    public float alpha;

    [Header("=====淡入=====")]
    public bool fadeinreset;
    public bool fadein;
    public float timerin;

    private void OnGUI()
    {
        screenwidth = Screen.width;
    }

    // Use this for initialization
    void Start () {
        gm = this.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (goreset)
        {
            goswitch.gameObject.SetActive(true);
            goreset = false;
            goswitch.anchoredPosition = new Vector2(-2 * screenwidth, 0);
            goswitch.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenwidth * 2);
        }
        if (goactive)
        {
            gm.dc.istalking = false;
            TimeGone();

            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.LeftControl) || goswitch.anchoredPosition.x >= 1.8 * screenwidth)
            {
                goswitch.anchoredPosition = new Vector2(2 * screenwidth, 0);
                goswitch.gameObject.SetActive(true);
                goactive = false;
                gm.dc.istalking = true;
            }

        }

        if (fadeoutreset)
        {
            black.gameObject.SetActive(true);
            fadeoutreset = false;
            UpdateColorAlpha(0);
        }

        if (fadeout)
        {
            if (timerout >= 1.5f)
            {
                fadeout = false;
                timerout = 0;
                alpha = 1;
                fadeinreset = true;
                fadein = true;
            }
            else
            {
                timerout += Time.deltaTime;
                alpha += Time.deltaTime;
                UpdateColorAlpha(alpha);
            }
        }

        if (fadeinreset)
        {
            black.gameObject.SetActive(true);
            fadeinreset = false;
            UpdateColorAlpha(1);
        }
        if (fadein)
        {
            if (timerin >= 1f)
            {
                fadein = false;
                timerin = 0;
                alpha = 0;
                black.gameObject.SetActive(false);
            }
            else
            {
                timerin += Time.deltaTime;
                alpha -= Time.deltaTime;
                UpdateColorAlpha(alpha);
            }
        }
    }

    void UpdateColorAlpha(float alpha)
    {
        Color ss = black.color;
        ss.a = alpha;
        black.color = ss;
        if (alpha > 1f)
        {
            alpha = 1f;
        }
        else if (alpha < 0)
        {
            alpha = 0;
        }
    }

    public void TimeGone()
    {
        goswitch.anchoredPosition = Vector2.Lerp(goswitch.anchoredPosition, new Vector2(2 * screenwidth, 0),gospeed * Time.deltaTime);
    }

}
