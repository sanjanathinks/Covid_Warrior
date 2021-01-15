using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Monster : MonoBehaviour
{
    public static float lastQuestionTime;

    public GameObject main;
    public GameObject virtualCam;
    public Button attack;

    public int health = 10;
    public float attackTime; //time between last question finish and monster's potential for next attack

    private GameObject player;

    void Awake() {
      player = GameObject.Find("player");
      attack.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
      if (health <= 0) {
        virtualCam.SetActive(false);
        player.GetComponent<Player>().updateUser();
        player.GetComponent<PlayerMovement>().setBattle(false);
        attack.interactable = false;
        attack.gameObject.SetActive(false);
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
          attack.interactable = true;
          attack.GetComponentInChildren<TextMeshProUGUI>().text = "Attack";

          //if it's been enough time and it's close, monster should attack
          if (Time.time - lastQuestionTime > attackTime && !PlayerMovement.gameIsPaused) {
            Debug.Log(Time.time);
            Debug.Log(lastQuestionTime);
            attack.gameObject.SetActive(false);
            //monster should attack
            //play monster attack animation, when finished, call monsterAttack()
            monsterAttack();
          }
        }
        else {
          attack.interactable = false;
          attack.GetComponentInChildren<TextMeshProUGUI>().text = "Move closer to attack";
        }
      }
    }

    public void monsterAttack()
    {
        main.GetComponent<ChoiceScript>().newQuestion();
    }

    //check distance to player
    //check time since last question close

    void OnBecameVisible() {
      //note that this also triggers if you have editor window open and you can see the monster
      //but it should be fine for actual gameplay
      virtualCam.SetActive(true);
      attack.gameObject.SetActive(true);
      lastQuestionTime = Time.time;
      GetComponent<MonsterMove>().enabled = true;
    }
}
