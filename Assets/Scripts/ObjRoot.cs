using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRoot : MonoBehaviour {

    public static ObjRoot Instance;

    //初始化
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
}
