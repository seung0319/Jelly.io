using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum BGMType
{
    None,
    InGame
}

public enum SEType
{
    None,
    Button,
    Buy,
    Grow,
    Pause,
    Resume,
    Sell,
    Touch,
    Drop,
    Unlock
}

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> BGMs = new List<AudioClip>();
    public List<AudioClip> SEs = new List<AudioClip>();

    public static BGMType playing_sound = BGMType.None;
    public static SEType se_sound = SEType.None;
    public static SoundManager instance;

    public AudioSource BGMSource;
    public AudioSource SESource;

    public AudioMixer masterMixer;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    private void Awake()
    {
        //if (instance == null)
        //{
            instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else
        //{
        //    Destroy(gameObject);
        //}
    }
    private void Update()
    {
        AudioControl();
    }

    // 사운드 on
    public void BGM_Play(BGMType type)
    {
        if (type != playing_sound)
        {

            switch (type)
            {
                case BGMType.InGame:
                    BGMSource.clip = BGMs[0];
                    break;
            }
            BGMSource.Play();
        }
    }
    // 사운드 off
    public void BGM_Stop()
    {
        GetComponent<AudioSource>().Stop();
        playing_sound = BGMType.None;
    }


    public void SEPlay(SEType type)
    {
        if (type != se_sound)
        {

            switch (type)
            {
                case SEType.Button:
                    SESource.clip = SEs[0];
                    break;
                case SEType.Buy:
                    SESource.clip = SEs[1];
                    break;
                case SEType.Grow:
                    SESource.clip = SEs[2];
                    break;
                case SEType.Pause:
                    SESource.clip = SEs[3];
                    break;
                case SEType.Resume:
                    SESource.clip = SEs[4];
                    break;
                case SEType.Sell:
                    SESource.clip = SEs[5];
                    break;
                case SEType.Touch:
                    SESource.clip = SEs[6];
                    break;
                case SEType.Drop:
                    SESource.clip = SEs[7];
                    break;
                case SEType.Unlock:
                    SESource.clip = SEs[8];
                    break;

            }
            SESource.PlayOneShot(SESource.clip);
        }
    }

    

    public void MasterControl()
    {
        float sound = masterSlider.value;
        if (sound == -40f)
        {
            masterMixer.SetFloat("Master", -80);
        }
        else
        {
            masterMixer.SetFloat("Master", sound);
        }
    }
    public void BgmControl()
    {
        float sound = bgmSlider.value;
        if (sound == -40f)
        {
            masterMixer.SetFloat("BGM", -80);
        }
        else
        {
            masterMixer.SetFloat("BGM", sound);
        }
    }
    public void SeControl()
    {
        float sound = seSlider.value;
        if (sound == -40f)
        {
            masterMixer.SetFloat("SFX", -80);
        }
        else
        {
            masterMixer.SetFloat("SFX", sound);
        }
    }

    public void SFXtest()
    {
        instance.SEPlay(SEType.Button);
    }

    public void AudioControl()
    {
        MasterControl();
        BgmControl();
        SeControl();
    }
}
