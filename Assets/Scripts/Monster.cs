using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Monster : MonoBehaviour
{
    public static float timeInRange;

    public GameObject main;
    public GameObject virtualCam;
    public Button attackButton;
    public GameObject healthBar;
    public bool isAttacking;
    public Animator animator;
    public Image questionBackground;
    public Sprite attackSprite;

    public int health = 10;
    public int maxHealth = 10;
    public float attackTime; //time player has been in range of monster attack
    public string progress;
    public bool cameraToMainAfter;

    private GameObject player;

    void Awake() {
      player = GameObject.Find("player");
      attackButton.interactable = false;
      healthBar.SetActive(false);
    }

    void Start() {
      if (player.GetComponent<Player>().getProgress()!=null && player.GetComponent<Player>().getProgress().CompareTo(progress) >= 0) {
        Destroy(this.gameObject);
      }
    }

    // Update is called once per frame
    void Update()
    {
      if (health <= 0) {
        if (cameraToMainAfter) {
          virtualCam.SetActive(false);
        }
        else {
          virtualCam.GetComponent<CinemachineVirtualCamera>().Follow = player.transform;
        }
        player.GetComponent<Player>().updateUser(progress);
        player.GetComponent<PlayerMovement>().setBattle(false);
        PlayerMovement.gameIsPaused = false;
        attackButton.interactable = false;
        attackButton.gameObject.SetActive(false);
        healthBar.SetActive(false);
        Destroy(this.gameObject);
      }
    }

    void FixedUpdate() {
      Vector3 euler = transform.eulerAngles;
      if (euler.z > 180) euler.z = euler.z - 360;
      euler.z = Mathf.Clamp(euler.z, -10, 10);
      transform.eulerAngles = euler;

      if (GetComponent<Renderer>().isVisible) {
        float dist = Vector3.Distance(player.transform.position, transform.position);
        if (dist <= 10.0f) {
          attackButton.interactable = true;
          attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Attack";
          timeInRange+=Time.fixedDeltaTime;

          //if it's been enough time and it's close to player, monster should attack
          if (timeInRange > attackTime && !PlayerMovement.gameIsPaused && !player.GetComponent<PlayerMovement>().isAttacking) {
            Attack();
          }
        }
        else {
          attackButton.interactable = false;
          attackButton.GetComponentInChildren<TextMeshProUGUI>().text = "Move closer to attack";
        }
      }
    }

    public void attackFinished() {
      animator.SetBool("isAttacking", false);
      ChoiceScript.animationIsFinished();
      isAttacking = false;
    }

    private void Attack() {
      isAttacking = true;

      //UI elements
      attackButton.gameObject.SetActive(false);
      questionBackground.sprite = attackSprite;

      animator.SetBool("isAttacking", true);

      main.GetComponent<ChoiceScript>().newQuestion();
      PlayerMovement.gameIsPaused = true; //stop movement
    }
}
