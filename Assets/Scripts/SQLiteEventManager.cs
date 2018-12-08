using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class SQLiteEventManager : MonoBehaviour {

    private GameManager gm;
    public SQLiteHelper eventSQL;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    public void CheckPlayerState(string eventName)
    {
        eventSQL = new SQLiteHelper("data source=" + Application.persistentDataPath + "/location.db");
        SqliteDataReader reader = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("PlayerState")) != "no")
                {
                    string stateName = reader.GetString(reader.GetOrdinal("PlayerState"));
                    eventSQL.UpdateEventState("PlayerState", "value", "value+1", "name", "=", "'" + stateName + "EXP" + "'");
                    CheckLVUP(stateName);
                }
            }
        }
        eventSQL.CloseConnection();
    }

    /// <summary>
    /// 使用这个后，会改变当前事件名curName
    /// </summary>
    /// <param name="eventName">未改变的curName</param>
    public void CheckNextEvent(string eventName)
    {
        eventSQL = new SQLiteHelper("data source=" + Application.persistentDataPath + "/location.db");
        SqliteDataReader reader = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("NextEvent")) != "no")
                {
                    gm.dm.curName = reader.GetString(reader.GetOrdinal("NextEvent"));
                    gm.dm.StartDialog();
                }
            }
        }
        eventSQL.CloseConnection();
    }

    public void CheckSetEventFinsh(string eventName)
    {
        eventSQL = new SQLiteHelper("data source=" + Application.persistentDataPath + "/location.db");
        SqliteDataReader readList = eventSQL.ReadFullTable("EventList");     
        while (readList.Read())
        {
            if (readList.GetString(readList.GetOrdinal("EventName")) == eventName)
            {
                if (readList.GetString(readList.GetOrdinal("SetEventFinish")) != "no"  )
                {
                    string setEventName = readList.GetString(readList.GetOrdinal("SetEventFinish"));
                    eventSQL.UpdateEventState("EventData", "EventState", "'finish'", "EventName", "=", "'" + setEventName + "'");
                    //print(setEventName);
                }
            }
        }
        eventSQL.CloseConnection();
    }

    public void CheckSetEventStart(string eventName)
    {
        eventSQL = new SQLiteHelper("data source=" + Application.persistentDataPath + "/location.db");
        SqliteDataReader reader = eventSQL.ReadFullTable("EventList");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("SetEventStart")) != "no")
                {
                    string setEventName = reader.GetString(reader.GetOrdinal("SetEventStart"));
                    eventSQL.UpdateEventState("EventData", "EventState", "'start'", "EventName", "=", "'" + setEventName + "'");
                    //print(setEventName);
                }
            }
        }
        eventSQL.CloseConnection();
    }
    /// <summary>
    /// 检验是否可以升级
    /// </summary>
    /// <param name="stateName"></param>
    private void CheckLVUP( string stateName)
    {
        SqliteDataReader reader = eventSQL.ReadFullTable("PlayerState");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("name")) == (stateName + "EXP") )
            {
                if (reader.GetInt32(reader.GetOrdinal("value")) == 2)
                {
                    eventSQL.UpdateEventState("PlayerState", "value", "value+1", "name", "=", "'" + stateName + "LV" + "'");
                    gm.ps.LVup = true;
                }
            }
        }
    }

    public void ShowPlayerState()
    {
        eventSQL = new SQLiteHelper("data source=" + Application.persistentDataPath + "/location.db");
        SqliteDataReader reader = eventSQL.ReadFullTable("PlayerState");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("name")) == "ShamLV")
            {
                gm.ps.ShamText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
                //print("ShamLV");
            }
            if (reader.GetString(reader.GetOrdinal("name")) == "PassiveLV")
            {
                gm.ps.PassiveText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
                //print("PassiveLV");
            }
            if (reader.GetString(reader.GetOrdinal("name")) == "RebelLV")
            {
                gm.ps.RebelText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
                //print("RebelLV");
            }
            if (reader.GetString(reader.GetOrdinal("name")) == "SelfishLV")
            {
                gm.ps.SelfishText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
            }
            if (reader.GetString(reader.GetOrdinal("name")) == "EvilLV")
            {
                gm.ps.EvilText.text = reader.GetInt32(reader.GetOrdinal("value")).ToString();
            }
        }
        eventSQL.CloseConnection();
    }

    public void SetAllDefalut()
    {
        eventSQL = new SQLiteHelper("data source=" + Application.persistentDataPath + "/location.db");
        eventSQL.UpdateEventState("PlayerState", "value", "0", "value", "!=", "0");
        eventSQL.UpdateEventState("EventData", "EventState", "'ready'", "EventState", "!=", "'ready'");
        eventSQL.CloseConnection();
    }

}
