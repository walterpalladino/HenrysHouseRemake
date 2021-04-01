using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {

	private GameObject key;

	[SerializeField] private Text scoreText;
	[SerializeField] private Text roomText;
	[SerializeField] private Text livesText;

	[SerializeField] private RetroPlatformController playerController;

	public int playerScore = 0;
	public int playerLives = 3;
	public int actualRoom = 1;

	public bool keyCollected = false;

	private SoundManager soundManager = null;

	void Awake () {
	}


	// Use this for initialization
	void Start () {

		soundManager = SoundManager.Instance;

		key = GameObject.FindGameObjectWithTag("Key");
		key.SetActive (false);

		DisplayPlayerScore ();
		DisplayActualRoom ();
		DisplayPlayerLives ();

	}
	

	public void ItemColleted (GameObject item) {

		switch (item.tag) {
		case "Hat":
			//	Remove item
			item.SetActive (false);
			//	Increase Score
			playerScore += 50;
			if (soundManager != null) {
				soundManager.PlayPickUpItem ();
			}
			break;
		case "Question":
			//	Remove item
			item.SetActive(false);
			//	Increase Score
			playerScore += 200;
			if (soundManager != null) {
				soundManager.PlayPickUpItem ();
			}
			//	Activate the Key
			key.SetActive (true);
			break;
		case "Hand":
			//	Remove item
			item.SetActive(false);
			//	Increase Score
			playerScore += 150;
			if (soundManager != null) {
				soundManager.PlayPickUpItem ();
			}
			break;
		case "Bag":
			//	Remove item
			item.SetActive(false);
			//	Increase Score
			playerScore += 100;
			if (soundManager != null) {
				soundManager.PlayPickUpItem ();
			}
			break;
		case "Key":
			//	Remove item
			item.SetActive(false);
			//	Increase Score
			playerScore += 250;
			if (soundManager != null) {
				soundManager.PlayPickUpItem ();
			}
			//	Open Door
			keyCollected = true;
			break;
		case "Tie":
			//	Remove item
			item.SetActive(false);
			//	Increase Score
			playerScore += 125;
			if (soundManager != null) {
				soundManager.PlayPickUpItem ();
			}
			break;
		case "Exit":
			if (keyCollected) {
				if (soundManager != null) {
					soundManager.PlayLevelUp ();
				}
				LevelCompleted ();
			}
			break;
		default:
			break;
		} 

		DisplayPlayerScore ();

	}

	private void DisplayPlayerScore () {
		string text = "";

		if (playerScore < 10) {
			text += "0";
		}
		if (playerScore < 100) {
			text += "0";
		}
		if (playerScore < 1000) {
			text += "0";
		}
		if (playerScore < 1000) {
			text += "0";
		}
		if (playerScore < 10000) {
			text += "0";
		}

		scoreText.text = text + playerScore;
	}

	private void DisplayActualRoom () {
		roomText.text = "0" + actualRoom;
	}

	private void DisplayPlayerLives () {
		livesText.text = "0" + playerLives;
	}

	public void ClownShoeSmashedThePlayer (GameObject clownShoe) {

		//	Animate the player
		playerController.PlayerDied ();
		if (soundManager != null) {
			soundManager.PlayPlayerDied ();
		}

		//	Discount live
		playerLives --;
		DisplayPlayerLives ();
		if (playerLives > 0) {
			//	If there are lives left, continue	
			RestartPlayer ();
		} else {
			//	If not, move to the main screen
			GameOver ();
		}
	}

	public void RotatingFacesHitThePlayer (GameObject rotatingFaces) {

		//	Animate the player
		playerController.PlayerDied ();
		if (soundManager != null) {
			soundManager.PlayPlayerDied ();
		}

		//	Discount live
		playerLives --;
		DisplayPlayerLives ();
		if (playerLives > 0) {
			//	If there are lives left, continue
			RestartPlayer ();
		} else {
			//	If not, move to the main screen
			GameOver ();
		}
	}

	private void RestartPlayer () {
		StartCoroutine(RespawnPlayer ());
	}

	IEnumerator RespawnPlayer() {
		
		yield return new WaitForSeconds(2);
		playerController.PlayerRespawn ();

	}


	private void GameOver () {
		StartCoroutine(BackToMainMenu ());
	}

	IEnumerator BackToMainMenu () {
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene ("MainTitle");
	}

	private void LevelCompleted () {
		StartCoroutine(BackToMainMenu ());
	}

}
