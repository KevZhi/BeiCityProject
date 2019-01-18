using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Mono.Data.Sqlite;

public class CommonMenu : MonoBehaviour {

    public static CommonMenu Instance;

    public Canvas canvas;

    private void Awake()
    {
        canvas = this.GetComponent<Canvas>();
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

    private void OnGUI()
    {
        canvas.worldCamera = Camera.main;
    }

}
