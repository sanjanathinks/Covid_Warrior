using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFloat : MonoBehaviour
{
    public CharacterController2D controller;
    //public Animator animator;

    public float runSpeed;
    public float bounceForce;
    public float totalChangeX;
    public float totalChangeY;

    private int direction = 1;
    private float traveled;
    private float horizontalMove;
    private float originalX;
    private float originalY;

    void Start()
    {
      originalX = this.gameObject.transform.position.x;
      originalY = this.gameObject.transform.position.y;
      controller.setAirControl(true);
    }

    void Update()
    {
      horizontalMove = direction * runSpeed;
      //animator.SetFloat("speed", Mathf.Abs(horizontalMove));
    }

    void FixedUpdate()
    {
      //move character
      controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
      traveled = this.gameObject.transform.position.x - originalX;

      //if moved as far as want and going in direction that would increase that movement change, turn around
      if (Mathf.Abs(traveled) > totalChangeX && traveled*direction > 0) {
        direction*=-1;
      }

      //if gravity pulled down as far as want
      if (originalY - this.gameObject.transform.position.y > totalChangeY) {
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, bounceForce));
      }
    }
}
