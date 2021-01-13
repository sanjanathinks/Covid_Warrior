using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
  public GameObject main;
  public GameObject camera;

  public int health = 10;

    // Update is called once per frame
    void Update()
    {
      if (health <= 0) {
        camera.SetActive(false);
        Destroy(this.gameObject);
      }
    }

    void FixedUpdate() {
      Vector3 euler = transform.eulerAngles;
      if (euler.z > 180) euler.z = euler.z - 360;
      euler.z = Mathf.Clamp(euler.z, -10, 10);
      transform.eulerAngles = euler;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Equals("player")) {
          main.GetComponent<ChoiceScript>().newQuestion();
        }
        //TODO: also will want to stop movement and otherwise pause the game
        //have question show now but not before this
    }

    void OnBecameVisible() {
      //note that this also triggers if you have editor window open and you can see the monster
      //but it should be fine for actual gameplay
      camera.SetActive(true);
    }
}
