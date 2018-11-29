using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;
public class GlobalControl : MonoBehaviour {

    public static GlobalControl Instance;
    public BuffFactory buffFact;

    private GameManager gm;

    //private BuffDataStructure buffData;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
        CheckGameObject();
        CheckSingle();
    }

    private void Start()
    {
        InitBuffFactory();

        //buffFact.CreateBuff("alone", gm.buffPanel.transform);
        buffFact.CreateBuff("unfortunate", gm.buffPanel.transform);
    }

    private void InitBuffFactory()
    {
        buffFact = new BuffFactory();
    }

    private void CheckSingle()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void CheckGameObject()
    {
        if (tag=="Player")
        {
            return;
        }
        Destroy(gameObject);
    }
}
