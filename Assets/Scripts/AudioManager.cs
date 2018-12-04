using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

    public string audioName;
    private AudioClip clip;

    public GameManager gm;

    private void Awake()
    {
        gm = this.GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (audioName=="bell")
        {
            if (this.GetComponent<AudioSource>().time >= 8f)
            {
                audioName = null;
                StopAudio();
            }
        }
        if ((this.GetComponent<AudioSource>().clip == null || this.GetComponent<AudioSource>().clip.name != "Firelink Shrine") && gm.em.event001==2 && (SceneManager.GetActiveScene().name == "class15" || SceneManager.GetActiveScene().name == "floor2" || SceneManager.GetActiveScene().name == "supportClass15"))
        {
            audioName = "Firelink Shrine";
            LoadAudio();
        }
        if ((this.GetComponent<AudioSource>().clip == null || this.GetComponent<AudioSource>().clip.name != "苦しみの曲") && (SceneManager.GetActiveScene().name == "gate" || SceneManager.GetActiveScene().name == "dongming" || SceneManager.GetActiveScene().name == "street"))
        {
            audioName = "苦しみの曲";
            LoadAudio();
        }
        if ((this.GetComponent<AudioSource>().clip == null || this.GetComponent<AudioSource>().clip.name != "遊園施設") && SceneManager.GetActiveScene().name == "1.welcome")
        {
            audioName = "遊園施設";
            LoadAudio();
        }
    }

    public void LoadAudio()
    {
        clip = Resources.Load("AudioClips\\" + audioName) as AudioClip;
        this.GetComponent<AudioSource>().clip = clip;
        this.GetComponent<AudioSource>().loop = true;
        this.GetComponent<AudioSource>().Play();

    }

    public void LoadAudioOnce()
    {
        clip = Resources.Load("AudioClips\\" + audioName) as AudioClip;
        this.GetComponent<AudioSource>().clip = clip;
        this.GetComponent<AudioSource>().loop = false;
        this.GetComponent<AudioSource>().Play();
    }

    public void StopAudio()
    {
        this.GetComponent<AudioSource>().clip = null;
        this.GetComponent<AudioSource>().loop = false;
        this.GetComponent<AudioSource>().Stop();
    }

}
