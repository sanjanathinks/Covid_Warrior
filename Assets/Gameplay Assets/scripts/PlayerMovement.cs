using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static bool gameIsPaused;

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool battle = false;
    Vector3 screenBounds;
    Vector3 screenPosition;
    private float width;
    private float height;

    void Start() {
      width = GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
      height = GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

    void Update()
    {
      if (!gameIsPaused) {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
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
      if (gameIsPaused) {
        animator.SetFloat("speed", 0);
        animator.SetBool("isJump", false);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
      }
    }

    public void Onlanding () {
      animator.SetBool("isJump", false);
    }

    // Update is called once per frame

    void FixedUpdate()
    {
      if (!gameIsPaused) {
        //move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
      }
    }

    public void setBattle(bool value, Vector3 bounds, Vector3 position) {
      battle = value;
      screenBounds = bounds;
      screenPosition = position;
    }

    public void setBattle(bool value) {
      battle = value;
    }

    void LateUpdate() {
      if (battle) {
        Vector3 pos = transform.position;
        float x = pos.x;
        pos.x = Mathf.Clamp(pos.x, screenPosition.x*2 - screenBounds.x + width, screenBounds.x - width);
        if (x != pos.x) {
          controller.Move(pos.x - x, false, false);
        }
      }
    }
}
