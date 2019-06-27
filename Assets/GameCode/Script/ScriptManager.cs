using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScriptManager : MonoBehaviour {
    public Text role;
    public Text talk;
    public Image background;
    public Image left;
    public Image right;

    void Start()
    {
        ScriptLoad.LoadScripts("test.txt");
        HandleData(ScriptLoad.LoadNext());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleData(ScriptLoad.LoadNext());
        }
    }

    public void SetText(Text text, string content)
    {
        //print(content);
        text.text = content;
    }

    private string picturePath = "Assets/Resources/Pictures/";
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
        if (data.type == "0")
        {
            SetImage(background, data.picName);
            print(data.picName);
            HandleData(ScriptLoad.LoadNext());
        }
        else
        {

            if (data.pos.CompareTo("left") == 0)
            {
                left.gameObject.SetActive(true);
                SetImage(left, data.picName);
                //right.gameObject.SetActive(false);
            }
            else
            {
                //right.gameObject.SetActive(true);
                SetImage(right, data.picName);
                left.gameObject.SetActive(false);
            }
            SetText(role, data.name);
            SetText(talk, data.talk);
        }
    }
}
