using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	private static SoundManager instance;

	public static SoundManager Instance { get { return instance; } }


	private AudioSource audioSource;

	[SerializeField]
	private AudioClip backgroundMusic;
	[SerializeField]
	private AudioClip pickupItemSound;
	[SerializeField]
	private AudioClip jumpSound;
	[SerializeField]
	private AudioClip playerDiedSound;
	[SerializeField]
	private AudioClip levelUpSound;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}


	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		//PlayBackgroundMusic (true);
	}
	
	public void PlayBackgroundMusic (bool play) {

		audioSource.Stop ();

		if (play) {
			audioSource.clip = backgroundMusic; 
			audioSource.Play ();
		}

	}

	public void PlayPickUpItem () {
		if (audioSource.isPlaying) {
			audioSource.Stop ();
		}
		audioSource.clip = pickupItemSound;
		audioSource.Play ();
	}

	public void PlayJump () {
		if (audioSource.isPlaying) {
			audioSource.Stop ();
		}
		audioSource.clip = jumpSound;
		audioSource.Play ();
	}

	public void PlayPlayerDied () {
		if (audioSource.isPlaying) {
			audioSource.Stop ();
		}
		audioSource.clip = playerDiedSound;
		audioSource.Play ();
	}

	public void PlayLevelUp () {
		if (audioSource.isPlaying) {
			audioSource.Stop ();
		}
		audioSource.clip = levelUpSound;
		audioSource.Play ();
	}


}
