using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class TweenEffect{
    
    /// <summary>
    /// 图片淡入
    /// </summary>
    /// <param name="image"></param>
    public static void SetImageFadeIn(Image image)
    {
        image.gameObject.SetActive(true);
        image.color = new Color(1, 1, 1, 0);
        image.DOColor(new Color(1, 1, 1, 1), 1f).SetEase(Ease.Linear);
    }
    public static void SetImageFadeIn(Image image,float time)
    {
        image.gameObject.SetActive(true);
        image.color = new Color(1, 1, 1, 0);
        image.DOColor(new Color(1, 1, 1, 1), time).SetEase(Ease.Linear);
    }
    /// <summary>
    /// 图片淡出
    /// </summary>
    /// <param name="image"></param>
    public static void SetImageFadeOut(Image image)
    {
        image.color = new Color(1, 1, 1, 1);
        image.DOColor(new Color(1, 1, 1, 0), 1f).SetEase(Ease.Linear);
    }
    public static void SetImageFadeOut(Image image,float time)
    {
        image.color = new Color(1, 1, 1, 1);
        image.DOColor(new Color(1, 1, 1, 0), time).SetEase(Ease.Linear);
    }
    /// <summary>
    /// 文字淡出
    /// </summary>
    /// <param name="image"></param>
    public static void SetTextFadeOut(Text text)
    {
        text.color = new Color(0, 0, 0, 1);
        text.DOColor(new Color(0, 0, 0, 0), 1f).SetEase(Ease.Linear);
    }
    public static void SetTextFadeOut(Text text, float time)
    {
        text.color = new Color(0, 0, 0, 1);
        text.DOColor(new Color(0, 0, 0, 0), time).SetEase(Ease.Linear);
    }
}
