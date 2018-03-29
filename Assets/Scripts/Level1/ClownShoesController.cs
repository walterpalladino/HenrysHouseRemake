using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownShoesController : MonoBehaviour {


	[SerializeField] private Animator clownShoe1Animator;
	[SerializeField] private Animator clownShoe2Animator;
	[SerializeField] private Animator clownShoe3Animator;

	//	0 : All Down - Starts here
	//	1 : First Smashing, Second and Third Idle
	//	2 : Second Smashing, First and Third Idle
	//	3 : Third Smashing, First and Second Idle
	private int clownShoeAnimationState = 0;	//	0 : All Down - Starts here


	// Update is called once per frame
	void Update () {
		CheckActualShoeAnimation ();
	}

	private void CheckActualShoeAnimation () {

		int idleCount = 0;
		if (!AnimatorIsPlaying(clownShoe1Animator, "Smash")) {
			idleCount++;
		}
		if (!AnimatorIsPlaying(clownShoe2Animator, "Smash")) {
			idleCount++;
		}
		if (!AnimatorIsPlaying(clownShoe3Animator, "Smash")) {
			idleCount++;
		}

		if (idleCount == 3) {

			if (clownShoeAnimationState == 0) {

				clownShoeAnimationState = 1;

				clownShoe1Animator.Play ("Smash");
				clownShoe2Animator.Play ("Idle");
				clownShoe3Animator.Play ("Idle");

			} else if (clownShoeAnimationState == 1) {

				clownShoeAnimationState = 2;

				clownShoe1Animator.Play ("Idle");
				clownShoe2Animator.Play ("Smash");
				clownShoe3Animator.Play ("Idle");

			} else if (clownShoeAnimationState == 2) {

				clownShoeAnimationState = 3;

				clownShoe1Animator.Play ("Idle");
				clownShoe2Animator.Play ("Idle");
				clownShoe3Animator.Play ("Smash");

			} else if (clownShoeAnimationState == 3) {

				clownShoeAnimationState = 0;

				clownShoe1Animator.Play ("Idle");
				clownShoe2Animator.Play ("Idle");
				clownShoe3Animator.Play ("Idle");
			}


		}



	}

	bool AnimatorIsPlaying (Animator animator) {
		return animator.GetCurrentAnimatorStateInfo(0).length >
			animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}

	bool AnimatorIsPlaying (Animator animator, string stateName) {
		return AnimatorIsPlaying (animator) && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
	}
}
