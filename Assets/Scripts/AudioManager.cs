using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public string audioName;

    public AudioSource bgm;
    public AudioClip clip;

    //private GameManager gm;

    private void Awake()
    {
        //gm = this.GetComponent<GameManager>();
        bgm = this.GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (audioName=="bell")
        {
            if (bgm.time >= 8f)
            {
                audioName = null;
                StopAudio();
            }
        }
    }

    public void LoadAudio()
    {
        clip = Resources.Load("AudioClips/" + audioName) as AudioClip;
        bgm.clip = clip;
        bgm.loop = true;
        bgm.Play();
    }

    public void LoadAudioOnce()
    {
        clip = Resources.Load("AudioClips/" + audioName) as AudioClip;
        bgm.clip = clip;
        bgm.loop = false;
        bgm.Play();
    }

    public void StopAudio()
    {
        bgm.clip = null;
        bgm.loop = false;
        bgm.Stop();
    }
}
