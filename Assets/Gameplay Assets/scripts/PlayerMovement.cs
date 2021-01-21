using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static bool gameIsPaused;

    public CharacterController2D controller;
    public Animator animator;
    public bool isAttacking;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool battle = false;
    Vector3 screenBounds;
    Vector3 screenPosition;

    private float width;
    private float height;
    private GameObject main;
    private Button attackButton;
    private GameObject currentMonster;

    //need this and OnSceneLoaded because object doesn't destroy
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      main = GameObject.Find("GameObject");
      if (GameObject.Find("Attack")!=null) {
        attackButton = GameObject.Find("Attack").GetComponent<Button>();
        Debug.Log(attackButton);
        attackButton.onClick.AddListener(attack);
        attackButton.gameObject.SetActive(false);
      }
    }

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
           animator.SetBool("isCrouching", true);
        }

        else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
            animator.SetBool("isCrouching", false);
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

    public void OnCrouching (bool isCrouching)
    {
      animator.SetBool("isCrouching", isCrouching);
    }


    void FixedUpdate()
    {
      if (!gameIsPaused) {
        //move character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
      }
      if (isAttacking && currentMonster!=null) {
        float xDiff = currentMonster.transform.position.x - transform.position.x;
        controller.Flip(xDiff > 0);
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

    void attack() {
      foreach (GameObject monster in GameObject.FindGameObjectsWithTag("monster")) {
        if (monster.GetComponent<Renderer>().isVisible && !monster.GetComponent<Monster>().isAttacking) {
          currentMonster = monster;
          isAttacking = true;
          main.GetComponent<ChoiceScript>().newQuestion();
          animator.SetBool("isAttacking", true);
          attackButton.gameObject.SetActive(false);
        }
      }
    }

    public void attackFinished() {
      gameIsPaused = true;
      animator.SetBool("isAttacking", false);
      ChoiceScript.animationIsFinished();
      isAttacking = false;
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
