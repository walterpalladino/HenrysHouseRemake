using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfFit
{
	ScaleToFit = 0,
	BestFit = 1
}

[ExecuteInEditMode]
public class RenderToTextureCameraFix : MonoBehaviour {

	[SerializeField]
	private TypeOfFit typeOfFit;

	[SerializeField]
	private RenderTexture renderTexture;

	private float width;
	private float height;

	public int pixelsToUnityUnits = 1;

	private int previousWidth = 0;
	private int previousHeight = 0;

	void Start () {
			
		width = 1.0f / renderTexture.texelSize.x;
		height = 1.0f / renderTexture.texelSize.y;

		Debug.Log ("Render Texture Dimension : " + width + " x " + height);

	}

	void Update () {
		if (Screen.width != previousWidth || Screen.height != previousHeight) {
			UpdateCamera ();
		}
	}

	void UpdateCamera () {

		Debug.Log ("Screen Dimension : " + Screen.width + " x " + Screen.height);

		previousWidth	= Screen.width;
		previousHeight	= Screen.height;

		float worldScreenHeight = Camera.main.orthographicSize * 2 / pixelsToUnityUnits;
		float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		Debug.Log ("worldScreenWidth / worldScreenHeight : " + worldScreenWidth + " / " + worldScreenHeight);

		transform.localScale = new Vector3(worldScreenWidth, worldScreenHeight, 1);
	}

}
	
