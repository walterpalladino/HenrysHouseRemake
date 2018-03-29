using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Ladder : MonoBehaviour {

	[SerializeField]
	private float maxDistanceToClimb	= 4.0f;	//	Max distance to the ladder center to allow climbing it

	private BoxCollider2D bc2d;

	public delegate void OnCanClimbChanged(bool canClimb, Ladder ladder);
	public static OnCanClimbChanged onCanClimbChanged;

	void Awake () {
		bc2d = GetComponent<BoxCollider2D> ();
	}

	void OnTriggerStay2D (Collider2D other) {

		if ((other.transform.position.x - bc2d.bounds.center.x) < maxDistanceToClimb) {
			//	Inform can climb
			onCanClimbChanged (true, this);
		} else {
			//	Inform can not climb
			onCanClimbChanged (false, this);
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		onCanClimbChanged (false, this);
	}

}
