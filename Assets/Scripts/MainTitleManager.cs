using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainTitleManager : MonoBehaviour {

	//private static MainTitleManager instance;

	//public static MainTitleManager Instance { get { return instance; } }

	private string levelName	= "Level1";

	void Awake() {
		//if (instance == null)
		//	instance = this;
		//else if (instance != this)
		//	Destroy(gameObject);
		//DontDestroyOnLoad(gameObject);
	}

	void Start () {
		Debug.Log ("Title Manager Start");
		Init ();
	}

	private void Init () {
		SoundManager.Instance.PlayBackgroundMusic (true);
	}

	public void PlayGameLevel () {
		SoundManager.Instance.PlayBackgroundMusic (false);
		SceneManager.LoadScene (levelName);
		Debug.Log ("Loading Level : " + levelName);
	}
}
