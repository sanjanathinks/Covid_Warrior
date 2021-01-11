using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWalk : MonoBehaviour
{
    public CharacterController2D controller;
    //public Animator animator;

    public float runSpeed;
    public float totalChange;

    private int direction = 1;
    private float traveled;
    private float horizontalMove;
    private float originalX;

    void Start()
    {
      originalX = this.gameObject.transform.position.x;
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
      if (Mathf.Abs(traveled) > totalChange && traveled*direction > 0) {
        direction*=-1;
        Debug.Log(traveled);
        Debug.Log(this.gameObject.transform.position);
      }
    }
}
