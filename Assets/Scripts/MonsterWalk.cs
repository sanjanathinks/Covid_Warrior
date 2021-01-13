using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWalk : MonoBehaviour
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
      horizontalMove = direction * runSpeed;
      //animator.SetFloat("speed", Mathf.Abs(horizontalMove));
    }

    void FixedUpdate()
    {
      //move character
      controller.Move(horizontalMove * Time.fixedDeltaTime, false, false);
      traveled = transform.position.x - originalX;
      Debug.Log(traveled);

      //if moved as far as want and going in direction that would increase that movement change, turn around
      if ((traveled > xRight || traveled < -xLeft) && traveled*direction > 0) {
        direction*=-1;
      }
    }
}
