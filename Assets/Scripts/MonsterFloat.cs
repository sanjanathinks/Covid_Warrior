using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFloat : MonsterMove
{
    public CharacterController2D controller;
    //public Animator animator;

    public float runSpeed;
    public float bounceForce;
    public float xRight;
    public float xLeft;
    public float yDown;
    public float yUp;

    private int direction = -1;
    private float traveled;
    private float horizontalMove;
    private float originalX;
    private float originalY;

    void Start()
    {
      originalX = transform.position.x;
      originalY = transform.position.y;
      controller.setAirControl(true);
      GetComponent<Rigidbody2D>().gravityScale = 1.0f;
    }

    void Update()
    {
      if (!PlayerMovement.gameIsPaused) horizontalMove = direction * runSpeed;
      //animator.SetFloat("speed", Mathf.Abs(horizontalMove));
    }

    void FixedUpdate()
    {
      if (PlayerMovement.gameIsPaused) {
        GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        foreach (Collider c in GetComponents<Collider>()) {
          c.enabled = false;
        }
      }
      if (!PlayerMovement.gameIsPaused) {
        GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        foreach (Collider c in GetComponents<Collider>()) {
          c.enabled = true;
        }
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        //move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
        traveled = transform.position.x - originalX;

        //if moved as far as want and going in direction that would increase that movement change, turn around
        if ((traveled > xRight || traveled < -xLeft) && traveled*direction > 0) {
          direction*=-1;
        }

        //if gravity pulled down as far as want
        if (originalY - transform.position.y > yDown) {
          this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, bounceForce));
        }
      }
    }

    void LateUpdate() {
      if (!PlayerMovement.gameIsPaused) {
        Vector3 pos = transform.position;
        float y = pos.y;
        pos.y = Mathf.Clamp(pos.y, originalY - yDown*2, originalY + yUp);
        transform.position = pos;
      }
    }
}
