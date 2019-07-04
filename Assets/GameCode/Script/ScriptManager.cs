using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScriptManager : MonoBehaviour {
    //public GameManager gm;
    public Image dialogImage;
    public Text roleText;
    public Text talkText;
    public Image background;
    public Image left;
    public Image right;
    private string picturePath = "Assets/Resources/Pictures/";

    private void Awake()
    {
        EventCenter.AddListener(EventType.NextSentence, SetNextSentence);
        EventCenter.AddListener(EventType.DialogFinish, SetDialogFinish);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.NextSentence, SetNextSentence);
        EventCenter.RemoveListener(EventType.DialogFinish, SetDialogFinish);
    }

    void Start()
    {
        //gm = GetComponent<GameManager>();
        ScriptLoad.LoadScripts("test.txt");
        HandleData(ScriptLoad.LoadNext());
    }

    /// <summary>
    /// 载入下一条对话
    /// </summary>
    private void SetNextSentence()
    {
        HandleData(ScriptLoad.LoadNext());
    }
    /// <summary>
    /// 对话结束
    /// </summary>
    private void SetDialogFinish()
    {
        TweenEffect.SetImageFadeOut(dialogImage);
        TweenEffect.SetTextFadeOut(roleText);
        TweenEffect.SetTextFadeOut(talkText);
    }

    public void SetText(Text text, string content)
    {
        text.text = content;
    }

    public void SetImage(Image image, string picName)
    {
        image.sprite = LoadPicture(picturePath + picName);
    }

    public Sprite LoadPicture(string picPath)
    {

        //创建文件读取流
        FileStream fileStream = new FileStream(picPath, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = Screen.width;
        int height = Screen.height;
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
    }

    public void HandleData(ScriptData data)
    {
        if (data == null)
            return;

        switch (data.type)
        {
            case ScriptType.Dialogue:
                SetText(roleText, data.role);
                SetText(talkText, data.talk);
                break;
            case ScriptType.FadeInRole:
                //gm.keyboardInput.action = true;
                switch (data.pos)
                {
                    case RolePos.Left:
                        SetImage(left, data.picName);
                        EventCenter.Broadcast(EventType.NextSentence);
                        TweenEffect.SetImageFadeIn(left);
                        break;
                    case RolePos.Right:
                        SetImage(right, data.picName);
                        EventCenter.Broadcast(EventType.NextSentence);
                        TweenEffect.SetImageFadeIn(right);
                        break;
                    default:
                        Debug.Log("error RolePos");
                        break;
                }
                break;
            case ScriptType.FadeOutRole:
                switch (data.pos)
                {
                    case RolePos.Left:
                        TweenEffect.SetImageFadeOut(left);
                        EventCenter.Broadcast(EventType.NextSentence);
                        break;
                    case RolePos.Right:
                        TweenEffect.SetImageFadeOut(right);
                        EventCenter.Broadcast(EventType.NextSentence);
                        break;
                    default:
                        Debug.Log("error RolePos");
                        break;
                }
                break;
            default:
                Debug.Log("HandleData data.type");
                break;
        }
    }
}
