using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class SQLiteEventManager : MonoBehaviour {

    private GameManager gm;
    private SQLiteHelper sql;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    public void CheckPlayerState(string eventName)
    {
        sql = new SQLiteHelper("data source=" + Application.streamingAssetsPath + "/sqlite4unity.db");
        SqliteDataReader reader = sql.ReadFullTable("event");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (reader.GetString(reader.GetOrdinal("PlayerState")) == "Passive")
                {
                    gm.ps.PassiveEXP++;
                    //Debug.Log(reader.GetString(reader.GetOrdinal("PlayerState")));
                }
                if (reader.GetString(reader.GetOrdinal("PlayerState")) == "Sham")
                {
                    gm.ps.ShamEXP++;
                }
                if (reader.GetString(reader.GetOrdinal("PlayerState")) == "Rebel")
                {
                    gm.ps.RebelEXP++;
                }
                if (reader.GetString(reader.GetOrdinal("PlayerState")) == "Selfish")
                {
                    gm.ps.SelfishEXP++;
                }
                if (reader.GetString(reader.GetOrdinal("PlayerState")) == "Evil")
                {
                    gm.ps.EvilEXP++;
                }
            }
        }
        sql.CloseConnection();
    }

    /// <summary>
    /// 会改变当前事件名curName
    /// </summary>
    /// <param name="eventName">为改变的curName</param>
    public void CheckNextEvent(string eventName)
    {
        sql = new SQLiteHelper("data source=" + Application.streamingAssetsPath + "/sqlite4unity.db");
        SqliteDataReader reader = sql.ReadFullTable("event");
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
        sql.CloseConnection();
    }

    public void CheckSetEventFinsh(string eventName)
    {
        sql = new SQLiteHelper("data source=" + Application.streamingAssetsPath + "/sqlite4unity.db");
        SqliteDataReader reader = sql.ReadFullTable("event");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (gm.em.HasEvent(reader.GetString(reader.GetOrdinal("SetEventFinish"))))
                {
                    gm.em.SetEventState(reader.GetString(reader.GetOrdinal("SetEventFinish")), 2);
                }
            }
        }
        sql.CloseConnection();
    }

    public void CheckSetEventStart(string eventName)
    {
        sql = new SQLiteHelper("data source=" + Application.streamingAssetsPath + "/sqlite4unity.db");
        SqliteDataReader reader = sql.ReadFullTable("event");
        while (reader.Read())
        {
            if (reader.GetString(reader.GetOrdinal("EventName")) == eventName)
            {
                if (gm.em.HasEvent(reader.GetString(reader.GetOrdinal("SetEventStart"))))
                {
                    gm.em.SetEventState(reader.GetString(reader.GetOrdinal("SetEventStart")), 1);
                }
            }
        }
        sql.CloseConnection();
    }
}
