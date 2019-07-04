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

    [Header("=====沉睡=====")]
    public bool fallsleepreset;
    public bool fallsleep;
    public float fallalpha;
    public Image BGblack;
    public float timersleep;

    [Header("=====醒来=====")]
    public bool wakeupreset;
    public bool wakeup;
    public float timerwake;

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
            goreset = false;

            gm.dc.istalking = false;
            goswitch.gameObject.SetActive(true);
            goswitch.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, screenwidth * 2);
            goswitch.anchoredPosition = new Vector2(-2 * screenwidth, 0);
            goactive = true;
            fadeoutreset = true;
        }
        if (goactive)
        {
            goswitch.anchoredPosition = Vector2.Lerp(goswitch.anchoredPosition, new Vector2(2 * screenwidth, 0), gospeed * Time.deltaTime);

            if (goswitch.anchoredPosition.x >= 1.5 * screenwidth)
            {
                goactive = false;
                goswitch.anchoredPosition = new Vector2(2 * screenwidth, 0);
        
                //gm.dc.istalking = true;
            }

        }

        if (fadeoutreset)
        {
            fadeoutreset = false;

            black.gameObject.SetActive(true);          
            UpdateColorAlpha(black, 0);
            fadeout = true;
        }
        if (fadeout)
        {
            if (timerout >= 1f)
            {
                fadeout = false;
                timerout = 0;
                alpha = 1;
                //fadeinreset = true;
                //fadein = true;
            }
            else
            {
                timerout += Time.deltaTime;
                alpha += Time.deltaTime;
                UpdateColorAlpha(black, alpha);
            }
        }

        if (alpha==1)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.LeftControl))
            {
                fadeinreset = true;
            }
        }

        if (fadeinreset)
        {
            fadeinreset = false;

            black.gameObject.SetActive(true);  
            UpdateColorAlpha(black, 1);
            fadein = true;
        }
        if (fadein)
        {
            if (timerin >= 1f)
            {
                fadein = false;
                timerin = 0;
                alpha = 0;
                black.gameObject.SetActive(false);

                gm.dc.istalking = true;
            }
            else
            {
                timerin += Time.deltaTime;
                alpha -= Time.deltaTime;
                UpdateColorAlpha(black, alpha);
            }
        }

        if (fallsleepreset)
        {
            fallsleepreset = false;

            BGblack.gameObject.SetActive(true);
            UpdateColorAlpha(BGblack, 0);
            fallsleep = true;

            gm.dc.istalking = false;
        }
        if (fallsleep)
        {

            if (timersleep >= 1f)
            {
                fallsleep = false;
                timersleep = 0;
                fallalpha = 1;

                gm.dc.istalking = true;
            }
            else
            {          
                timersleep += Time.deltaTime;
                fallalpha += Time.deltaTime;
                UpdateColorAlpha(BGblack,fallalpha);
            }
        }

        if (wakeupreset)
        {
            wakeupreset = false;

            BGblack.gameObject.SetActive(true);
            UpdateColorAlpha(BGblack, 1);
            wakeup = true;

            gm.dc.istalking = false;
        }
        if (wakeup)
        {

            if (timerwake >= 1f)
            {
                wakeup = false;
                timerwake = 0;
                fallalpha = 0;
                BGblack.gameObject.SetActive(false);

                gm.dc.istalking = true;
            }
            else
            {
                timerwake += Time.deltaTime;
                fallalpha -= Time.deltaTime;
                UpdateColorAlpha(BGblack, fallalpha);
            }
        }

    }

    void UpdateColorAlpha(Image img, float alpha)
    {
        Color ss = img.color;
        ss.a = alpha;
        img.color = ss;
        if (alpha > 1f)
        {
            alpha = 1f;
        }
        else if (alpha < 0)
        {
            alpha = 0;
        }
    }
}
