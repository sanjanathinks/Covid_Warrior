using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWalk : MonsterMove
{
    public CharacterController2D controller;
    //public Animator animator;

    public float runSpeed;
    public float xRight;
    public float xLeft;

    private int direction = -1;
    private float traveled;
    private float horizontalMove;
    private float originalX;

    void Start()
    {
      originalX = transform.position.x;
    }

    void Update()
    {
      if (!stop) horizontalMove = direction * runSpeed;
      //animator.SetFloat("speed", Mathf.Abs(horizontalMove));
    }

    void FixedUpdate()
    {
      if (stop) {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        foreach (Collider c in GetComponents<Collider>()) {
          c.enabled = false;
        }
      }
      if (!stop) {
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
      }
    }
}
