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
    bool battle = false;
    Vector3 screenBounds;
    Vector3 screenPosition;
    private float width;
    private float height;

    void Start() {
      width = transform.GetComponent<SpriteRenderer>().bounds.extents.x; //extents = size of width / 2
      height = transform.GetComponent<SpriteRenderer>().bounds.extents.y; //extents = size of height / 2
    }

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

    public void setBattle(bool value, Vector3 bounds, Vector3 position) {
      battle = value;
      screenBounds = bounds;
      screenPosition = position;
      Debug.Log(screenBounds);
      Debug.Log(position);
      Debug.Log(screenPosition.x - screenBounds.x/2 + width);
      Debug.Log(screenPosition.x + screenBounds.x/2 - width);
      Debug.Log(screenPosition.y - screenBounds.y/2 + height);
      Debug.Log(screenPosition.y + screenBounds.y/2 - height);
      Debug.Log(transform.position);
    }

    void LateUpdate() {
      if (battle) {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, screenPosition.x - screenBounds.x/2, screenPosition.x + screenBounds.x/2);
        pos.y = Mathf.Clamp(pos.y, screenPosition.y - screenBounds.y/2, screenPosition.y + screenBounds.y/2);
        this.gameObject.GetComponent<Rigidbody2D>().MovePosition(pos);
      }
    }
}
