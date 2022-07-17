using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject music;

    public void Awake()
    {
        music = GameObject.Find("menuMusic");
        audioSource = music.GetComponent<AudioSource>();
        audioSource.Play();
        DontDestroyOnLoad(music);
    }
    public void loadGameScene(){
        SceneManager.LoadScene("GameScene");
    }

}
