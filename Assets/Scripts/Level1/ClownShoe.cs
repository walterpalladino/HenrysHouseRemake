using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ClownShoe : MonoBehaviour {

	private LevelManager levelManager;

	// Use this for initialization
	void Start () {
		levelManager = GameObject.FindObjectOfType(typeof(LevelManager)) as LevelManager;
	}
	
	void OnTriggerEnter2D (Collider2D other) {

		if (other.tag == "Player") {
			levelManager.ClownShoeSmashedThePlayer (gameObject);
		}
	}


}
