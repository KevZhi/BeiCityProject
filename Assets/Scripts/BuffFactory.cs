using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class BuffFactory
{
    public GameObject CreateBuff(string buffName,Vector3 pos,Quaternion rot)
    {
        GameObject prefeb = Resources.Load(buffName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb, pos , rot);

        return obj;
    }

    public GameObject CreateBuff(string buffName,Transform parent)
    {
        GameObject prefeb = Resources.Load(buffName) as GameObject;
        GameObject obj = GameObject.Instantiate(prefeb);

        obj.transform.SetParent(parent, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        Buffs_handle(obj,buffName);

        return obj;
    }

    private List<string> buffs_list;
    private int buff_index = 0;
    private int buff_count = 0;

    private string enName;
    private string buffName;
    private string detail;

    public void Buffs_handle(GameObject _obj, string buff_enName)
    {
        XmlDocument xmlDocument = new XmlDocument();//新建一个XML“编辑器” 
        buffs_list = new List<string>();
        string data = Resources.Load("DataBase/buffData").ToString();
        xmlDocument.LoadXml(data);//载入这个xml  
        XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("buffs").ChildNodes;
        foreach (XmlNode xmlNode in xmlNodeList)
        {
            XmlElement xmlElement = (XmlElement)xmlNode;
            buffs_list.Add(xmlElement.ChildNodes.Item(0).InnerText + ","
                + xmlElement.ChildNodes.Item(1).InnerText + ","
                + xmlElement.ChildNodes.Item(2).InnerText);
        }
        buff_count = buffs_list.Count;//获取到底有多少条对话
        for (int i = 0; i < buff_count; i++)
        {
            string[] buff_detail_array = buffs_list[buff_index].Split(',');
            enName = buff_detail_array[0];
            buffName = buff_detail_array[1];
            detail = buff_detail_array[2];
            if (enName == buff_enName)
            {
                _obj.transform.Find("name").GetComponent<Text>().text = buffName;
                _obj.transform.Find("detail").GetComponentInChildren<Text>().text = detail;
            }
            else
            {
                buff_index++;
            }
        }
        buff_index = 0;
        buff_count = 0;
    }
}
