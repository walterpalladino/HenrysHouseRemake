using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetroPlatformController : RetroPhysicsObject {

	[SerializeField]
	private float walkingSpeed = 7;
	[SerializeField]
	private float jumpTakeOffSpeed = 7;
	[SerializeField]
	private float climbingSpeed = 7;

	private SpriteRenderer spriteRenderer;
	private Animator animator;

	private float prevMovX = 0.0f;

	private bool canClimb	= false;
	private Ladder ladder	= null;
	private bool isClimbing	= false;
	private bool hasStoppedClimbing	= false;

	private float	deltaLadderBoundaries	= 0.3f;


	//
	private Vector2 initialPosition;
	private bool isDead = false;




	// Use this for initialization
	void Start () 
	{
		spriteRenderer = GetComponent<SpriteRenderer> (); 
		animator = GetComponent<Animator> ();

		Ladder.onCanClimbChanged += OnCanClimbChanged;



		//	Store the initial position to respawn at the same place
		initialPosition = rb2d.transform.position;

	}

	protected override void ComputeVelocity()
	{

		if (isDead) {
			return;
		}

		Vector2 move = Vector2.zero;

		if (isGrounded) {
			if (Input.GetKey (KeyCode.A)) {
				move.x = -1.0f;
			} else if (Input.GetKey (KeyCode.D)) {
				move.x = 1.0f;
			}
			prevMovX	= move.x;
		} else {
			move.x	= prevMovX;
		}


		bool flipSprite = (spriteRenderer.flipX ? (move.x < -0.01f) : (move.x > 0.01f)) ;
		if (flipSprite) 
		{
			spriteRenderer.flipX = !spriteRenderer.flipX;
		}


		//	Check Ladder
		if ((canClimb) && (isGrounded)) {

			if (Input.GetKey (KeyCode.W)) {
				
				if (IsPlayerOnLadderBottom()) {

					isClimbing = true;

					//	Force x player position to the ladder center
					Vector2 tmp = rb2d.position;
					tmp.x = ladder.gameObject.transform.position.x;
					rb2d.position = tmp;

				}

			} else if (Input.GetKey (KeyCode.S)) {

				if (IsPlayerOnLadderTop ()) {

					isClimbing = true;

					//	Force x player position to the ladder center
					Vector2 tmp = rb2d.position;
					tmp.x = ladder.gameObject.transform.position.x;
					rb2d.position = tmp;

				}
			}

		}

		if (isClimbing) {
			
			if (Input.GetKey (KeyCode.W)) {
				targetVelocity	= Vector2.up * climbingSpeed;	
			} else if (Input.GetKey (KeyCode.S)) {
				targetVelocity	= Vector2.up * -climbingSpeed;	
			}

		} else {

			if (Input.GetKey (KeyCode.W) && isGrounded) {
				velocity.y = jumpTakeOffSpeed;
			}

			targetVelocity = move * walkingSpeed;

		}

		//	Update the animations
		if (animator != null) {
			
			if (isGrounded && !isClimbing) {
				animator.enabled	= true;
				if (Mathf.Abs (move.x) > 0.01f) {
					animator.Play ("Henry Walk");
				} else {
					animator.Play ("Henry Idle");
				}
			} else if (isClimbing) {

				animator.Play ("Henry Climb");

				if (isGrounded) {
					animator.enabled	= true;
				} else {
					if (targetVelocity.y > 0.01f) {
						animator.enabled	= true;
					} else if (targetVelocity.y < -0.01f) {
						animator.enabled	= true;
					} else {
						animator.enabled	= false;
					}
				}
			} else {
				animator.enabled	= true;
				animator.Play ("Henry Jump");
			}

		}


	}

	public void OnCanClimbChanged (bool canClimb, Ladder ladder) {
		this.canClimb	= canClimb;
		this.ladder = ladder;
	}

	private bool IsPlayerOnLadderBottom () {
		
		if (ladder == null) {
			return false;
		}

		BoxCollider2D ladderBc2d;
		ladderBc2d = ladder.GetComponent<BoxCollider2D> ();
	
		float ladderBottom	= ladderBc2d.bounds.center.y - ladderBc2d.bounds.extents.y;
	
		float playerBottom	= bc2d.bounds.center.y - bc2d.bounds.extents.y;

		if ((playerBottom - ladderBottom) < deltaLadderBoundaries) {
			return true;
		} else {
			return false;
		}
	
	}

	private bool IsPlayerOnLadderTop () {
		
		if (ladder == null) {
			return false;
		}

		BoxCollider2D ladderBc2d;
		ladderBc2d = ladder.GetComponent<BoxCollider2D> ();

		float ladderTop	= ladderBc2d.bounds.center.y + ladderBc2d.bounds.extents.y;

		float playerBottom	= bc2d.bounds.center.y - bc2d.bounds.extents.y;

		if ((ladderTop - playerBottom) < deltaLadderBoundaries) {
			return true;
		} else {
			return false;
		}

	}

	protected override void UpdateMovement () {
		
		if (isClimbing) {
			//	Logic for ladder climbing

			isGrounded	= false;
			//	Check movement along Y (Vertically)
			Vector2 newPosition = rb2d.position + Vector2.up * targetVelocity.y * Time.deltaTime;

			if (targetVelocity.y > 0.01f) {
				//	Up	-	Check Top
				if (IsPlayerOnLadderTop ()) {

					//	Adjust position on ground
					BoxCollider2D ladderBc2d;
					ladderBc2d = ladder.GetComponent<BoxCollider2D> ();

					float ladderTop	= ladderBc2d.bounds.center.y + ladderBc2d.bounds.extents.y;

					newPosition.y = ladderTop + bc2d.bounds.extents.y + 0.01f;

					isClimbing	= false;
					isGrounded	= true;
				}
			} else if (targetVelocity.y < -0.01f) {
				//	Down	- Check Bottom
				if (IsPlayerOnLadderBottom ()) {

					//	Adjust position on ground
					BoxCollider2D ladderBc2d;
					ladderBc2d = ladder.GetComponent<BoxCollider2D> ();

					float ladderBottom	= ladderBc2d.bounds.center.y - ladderBc2d.bounds.extents.y;

					newPosition.y = ladderBottom + bc2d.bounds.extents.y + 0.01f;

					isClimbing	= false;
					isGrounded	= true;
				}
			}

			rb2d.position = newPosition;

		} else {
			//	Logic for walking and jumping
			base.UpdateMovement();
		}
	}

	public void PlayerDied () {
		isDead = true;
		animator.enabled	= true;
		animator.Play ("Henry Died");
	}

	public void PlayerRespawn () {
		isDead = false;
		rb2d.transform.position = initialPosition;
	}
}
