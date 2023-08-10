using UnityEngine;
using System.Collections;

public class SJSoundObj : SJGoPoolObj 
{
	public	AudioSource			audio_src;

	[HideInInspector]
	public	SJSoundMng			sndMng;

	[HideInInspector]
	public	bool 				bOrder = false;

	bool 	bStartFrame = false;

	[HideInInspector]
	public	AudioClip 			clip_next;
	[HideInInspector]
	public	string 				bgmName_next;

	public	bool				bBGM = false;

	//public	bool				toggled_mute = false;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () 
	{
		if( bBGM == false && audio_src.isPlaying == false )
		{
			if( bStartFrame )
			{
				bStartFrame = false;
				return;
			}
			SJSoundMng.OnEnd_SoundObj_S(this);
		}
	}

	override	public	void 	AllocInstSJ( GameObject prf )
	{
		audio_src = GetComponent<AudioSource>();
	}

	override	public void 	StartInstSJ()
	{
		bStartFrame = true;
		clip_next = null;
		bgmName_next = "";
	}


	public	void NextInput_Sound( AudioClip clip , string bgm_name )
	{
		clip_next = clip;
		bgmName_next = bgm_name;
	}

	override	public void 	EndInstSJ()
	{
		audio_src.Stop();
	}

}
