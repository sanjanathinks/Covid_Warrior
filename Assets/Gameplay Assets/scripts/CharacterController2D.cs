using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[SerializeField] private float slopeCheckDistance;
	[SerializeField] PhysicsMaterial2D slippery;
	[SerializeField] PhysicsMaterial2D friction;
	[SerializeField] private float maxAngle;

	private CapsuleCollider2D[] allColliders;
	private Vector2 colliderSize;
	private float slopeDownAngle;
	private Vector2 slopeNormalPerpendicular;
	private bool isOnSlope;
	private float slopeSideAngle;
	private float xInput;
	private bool canWalk;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		allColliders = GetComponents<CapsuleCollider2D>();
		colliderSize = allColliders[0].size;

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void Update() {
		if (this.gameObject.name.Equals("player")) xInput = Input.GetAxisRaw("Horizontal");
	}

	private void FixedUpdate()
	{
		SlopeCheck();
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}

	private void SlopeCheck() {
		Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, colliderSize.y / 2));
		SlopeCheckHorizontal(checkPos);
		SlopeCheckVertical(checkPos);
	}

	private void SlopeCheckHorizontal(Vector2 checkPos) {
		RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, m_WhatIsGround);
		RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, - transform.right, 0.5f, m_WhatIsGround);

		if (slopeHitFront) {
			isOnSlope = true;
			slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
			Debug.DrawRay(slopeHitFront.point, slopeHitFront.normal, Color.green);
		} else if (slopeHitBack) {
			isOnSlope = true;
			slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
			Debug.DrawRay(slopeHitBack.point, slopeHitBack.normal, Color.blue);
		} else {
			isOnSlope = false;
			slopeSideAngle = 0.0f;
		}
	}

	private void SlopeCheckVertical(Vector2 checkPos) {
		RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, m_WhatIsGround);

		if (hit) {
			slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized; //points to left of ground
			slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up); //angle between y-axis and normal (same as angle between x-axis and slope)

			if (slopeDownAngle != 0.0f) {
				isOnSlope = true;
			}

			Debug.DrawRay(hit.point, hit.normal, Color.white);
			Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);

		}

		if (slopeDownAngle > maxAngle || slopeSideAngle > maxAngle) {
			canWalk = false;
			Debug.Log("down: " + slopeDownAngle + ", side: " + slopeSideAngle);
		} else {
			canWalk = true;
		}

		if (this.gameObject.tag.Equals("Player")) {
			if (xInput == 0.0f && canWalk && isOnSlope) {
				m_Rigidbody2D.sharedMaterial = friction;
				m_Rigidbody2D.gravityScale = 0;
			} else {
				m_Rigidbody2D.sharedMaterial = slippery;
				m_Rigidbody2D.gravityScale = 3;
			}
		}
	}

	public void setAirControl(bool floats) {
		m_AirControl = floats;
	}


	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		 //if (m_wasCrouching)

			// If the character has a ceiling preventing them from standing up, keep them crouching
			/*if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				m_wasCrouching = true;
			}*/

				if (!crouch)
					{
 // If the character has a ceiling preventing them from standing up, keep them crouching
 				if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
 				{
	 				crouch = true;
 				}
				}


		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity, 3 different cases:
			Vector3 targetVelocity = Vector3.zero;
			if (m_Grounded && !isOnSlope) {
				targetVelocity = new Vector2(move * 10f, 0.0f);
			} else if (m_Grounded && isOnSlope && canWalk) {
				targetVelocity = new Vector2(move * slopeNormalPerpendicular.x * -10f, move * slopeNormalPerpendicular.y * -10f);
			} else if (!m_Grounded) {
				targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			}
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump && slopeDownAngle <= maxAngle)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		//Vector3 theScale = transform.localScale;
		//theScale.x *= -1;
		//transform.localScale = theScale;
		transform.Rotate(0.0f, 180.0f, 0.0f);
	}
}
