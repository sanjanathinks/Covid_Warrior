using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    void Update()
    {
      horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
      animator.SetFloat("speed", Mathf.Abs(horizontalMove));


      if (Input.GetButtonDown("Jump")){
         jump = true;
         animator.SetBool("isJump", true);
      }

      if (Input.GetButtonDown("Crouch")){
         crouch = true;
      }
      else if (Input.GetButtonUp("Crouch")) {
          crouch = false;
      }
    }

    public void Onlanding () {
      animator.SetBool("isJump", false);
    }

    // Update is called once per frame

    void FixedUpdate()
    {
      //move character
      controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
      jump = false;
    }
}
