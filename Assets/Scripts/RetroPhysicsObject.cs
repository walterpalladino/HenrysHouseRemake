using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetroPhysicsObject : MonoBehaviour {

	//	Boundary control
	[System.Serializable]
	public class BoundaryControl
	{
		public bool checkBoundaries = false;

		public float boundaryLeft = 0;
		public float boundaryRight = 1;
		public float boundaryTop = 0;
		public float boundaryBottom = 1;
	}
	public BoundaryControl boundaryControl;	
	private float movMinX;
	private float movMaxX;
	private float movMinY;
	private float movMaxY;


	[SerializeField]
	private bool forceIntegerPositions	= false;

	public float customGravity = 9.8f;

	protected Vector2 targetVelocity;
	protected bool isGrounded;
	protected Vector2 groundNormal;
	protected Rigidbody2D rb2d;
	protected Vector2 velocity;
	protected int layerMask	;

	protected const float minMoveDistance = 0.001f;
	protected const float shellRadius = 0.01f;

	protected BoxCollider2D bc2d;

	void Awake () {

		rb2d = GetComponent<Rigidbody2D> ();
		bc2d = GetComponent<BoxCollider2D> ();

		//	Precaulculate the Maxs and Mins to check boundaries
		Vector3 objectExtents ;
		SpriteRenderer spriteRenderer;

		spriteRenderer = GetComponent<SpriteRenderer> ();
		objectExtents = spriteRenderer.bounds.extents;

		//	Works for Scale == 1
		movMinX = boundaryControl.boundaryLeft + objectExtents.x ;
		movMaxX = boundaryControl.boundaryRight - objectExtents.x ;

		movMinY = boundaryControl.boundaryBottom + objectExtents.y ;
		movMaxY = boundaryControl.boundaryTop - objectExtents.y ;

		layerMask	= LayerMask.GetMask ("Platform") | LayerMask.GetMask ("Wall");
	}

	void Update () 
	{
		targetVelocity = Vector2.zero;
		ComputeVelocity (); 
	}

	protected virtual void ComputeVelocity() {}

	void FixedUpdate()
	{
		UpdateMovement ();
	}

	protected virtual void UpdateMovement () {
		
		velocity -= customGravity * Vector2.up * Time.deltaTime;
		velocity.x = targetVelocity.x;

		isGrounded = false;

		Vector2 deltaPosition = velocity * Time.deltaTime;

		//	Check movement along X (Ground)
		Vector2 move = Vector2.right * deltaPosition.x;
		Movement (move, false);

		//	Check movement along Y (Vertically)
		move = Vector2.up * deltaPosition.y;
		Movement (move, true);
	}


	protected void Movement (Vector2 move, bool yMovement)
	{
		float distance = move.magnitude;

		RaycastHit2D	hit	;

		if (distance > minMoveDistance) {

			//	If the player is jumping will not test for collisions
			if (!yMovement || (move.y < 0.01f)) {

				Vector2 start	= rb2d.position;

				if (yMovement) {
					start.y	-= bc2d.bounds.extents.y;
				} else {
					start.x	+= bc2d.bounds.extents.x * Mathf.Sign (move.x);
				}

				Vector2 end = start + move;

				hit	= Physics2D.Linecast (start, end, layerMask, -Mathf.Infinity, distance + shellRadius);

				if (hit.collider != null) {

					if (yMovement) {
						isGrounded = true;
						velocity.y = 0.0f;
					}
					float modifiedDistance = hit.distance - shellRadius;
					distance = modifiedDistance < distance ? modifiedDistance : distance;
				}

			}

			UpdatePosition (rb2d.position + move.normalized * distance);

		}

	}

	//	This method controls the player movement inside boundaries
	protected void UpdatePosition (Vector2 position) {
		
		Vector2 newPosition = position;

		if (boundaryControl.checkBoundaries) {
			newPosition.x = Mathf.Clamp (newPosition.x, movMinX, movMaxX);
			newPosition.y = Mathf.Clamp (newPosition.y, movMinY, movMaxY);
		}

		if (forceIntegerPositions) {
			//	Force the position x/y to integer values only
			newPosition.x	= Mathf.Floor (newPosition.x + 0.5f);
			newPosition.y	= Mathf.Floor (newPosition.y + 0.5f);
		}

		rb2d.position = newPosition ;
	}
}
