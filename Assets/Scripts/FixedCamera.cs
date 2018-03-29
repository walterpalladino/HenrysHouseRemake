using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCamera : MonoBehaviour {

	public enum GameMode { Landscape, Portrait };

	[SerializeField]
	private float cameraWidth = 800;
	[SerializeField]
	private float cameraHeight = 480;
	[SerializeField]
	private GameMode gameMode = GameMode.Landscape;
	[SerializeField]
	private bool stretchCamera = false;

	[SerializeField]
	private float referenceHeight = 200;

	// Use this for initialization
	void Start () {
	
		switch (gameMode) {

		case GameMode.Landscape:
		
			if (stretchCamera) {
				// Scale the game object to fit the screen
				float newHeightScale = (float)(cameraWidth * Screen.height) / (float)(cameraHeight * Screen.width);
				gameObject.transform.localScale = new Vector3(1, newHeightScale, 1);
			}

			// Adjust the camera to the screen size
			Camera.main.orthographicSize = referenceHeight / 2.0f;
			break;
		
		default:
		
			if (stretchCamera) {
				// Scale the game object to fit the screen
				float newHeightScale = (float)(cameraWidth * Screen.width) / (float)(cameraHeight * Screen.height);
				gameObject.transform.localScale = new Vector3(newHeightScale, 1, 1);
			}

			// Adjust the camera to the screen size
			Camera.main.orthographicSize = (cameraWidth / 200f);
			break;
		}
	}

}
