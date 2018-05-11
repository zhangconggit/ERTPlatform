using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CPlayButtonAudio : MonoBehaviour {

    //音乐文件
    public AudioSource music;
    //音量
    public float musicVolume;

    void Start()
    {
        //设置默认音量
        musicVolume = 0.8F;
        if (gameObject.GetComponent<Button>() != null)
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        else if(gameObject.GetComponent<Toggle>() != null)
            gameObject.GetComponent<Toggle>().onValueChanged .AddListener(OnClick);
    }

    public void OnClick()
    {
        //没有播放中
        if (!music.isPlaying)
        {
            //播放
            music.Play();
        }
    }
    public void OnClick(bool bo)
    {
        //没有播放中
        if (!music.isPlaying)
        {
            //播放
            music.Play();
        }
    }
}
