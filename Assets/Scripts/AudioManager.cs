using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	static AudioSource bgmInstance;
	static AudioSource sfxInstance;
	[SerializeField] AudioSource bgm;
	[SerializeField] AudioSource sfx;
	
	public bool IsMute
    {
        get => bgm.mute;
        set => bgm.mute = value;
    }

    public float BgmVolume { get => bgm.volume; }
    public float SfxVolume { get => sfx.volume; }
    private void Start()
    {
        if (bgmInstance != null)
        {
            Destroy(this.bgm.gameObject);
            bgm = bgmInstance;
        }
        else
        {
            bgmInstance = bgm;
            bgm.transform.SetParent(null);
            DontDestroyOnLoad(bgm.gameObject);
        }
        if (sfxInstance != null)
        {
            Destroy(this.sfx.gameObject);
            sfx = sfxInstance;
        }
        else
        {
            sfxInstance = sfx;
            sfx.transform.SetParent(null);
            DontDestroyOnLoad(sfx.gameObject);
        }
        if (IsMuted())
        {
            IsMute = true;
        }
        SetMute(IsMuted());
        if (PlayerPrefs.HasKey("BGMVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadBGM();
            LoadSFX();
        }
    }

	
	public void PlaySFX(AudioClip clip)
	{
		if(sfx.isPlaying)
			sfx.Stop();
		sfx.clip = clip;
		sfx.Play();
	}
	
	public void ToggleMute()
	{
		if (IsMuted())
		{
			PlayerPrefs.SetInt("IsMuted", 1);
			bgm.volume = 1;
			sfx.volume = 1;
			bgm.mute = false;
			sfx.mute = false;
			PlayerPrefs.SetFloat("BGMVolume", 1);
			PlayerPrefs.SetFloat("SFXVolume", 1);
			if (IsMute)
			{ 
				bgm.Play();
				sfx.Play();
			}
		}
		else
		{
			PlayerPrefs.SetInt("IsMuted", 0);
			bgm.volume = 0;
			sfx.volume = 0;
			PlayerPrefs.SetFloat("BGMVolume", 0);
			PlayerPrefs.SetFloat("SFXVolume", 0);
		}
		Debug.Log(PlayerPrefs.GetInt("IsMute"));
	}
	
	public void SetMute(bool value)
	{
		bgm.mute = value;
		sfx.mute = value;
	}
	
	public void SetBgmVolume(float value)
	{
		bgm.volume = value;
		PlayerPrefs.SetFloat("BGMVolume", value);
	}
	
	public void SetSfxVolume(float value)
	{
		sfx.volume = value;
		PlayerPrefs.SetFloat("SFXVolume", value);
	}
	
	public void LoadBGM()
	{
		bgm.volume = PlayerPrefs.GetFloat("BGMVolume");
	}
	public void LoadSFX()
	{
		sfx.volume = PlayerPrefs.GetFloat("SFXVolume");
	}

	public bool IsMuted()
	{
		bool i = (PlayerPrefs.GetInt("IsMuted", 1) == 0);
		return i;
	}
}
