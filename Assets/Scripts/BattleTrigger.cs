using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTrigger : MonoBehaviour
{
  public GameObject virtualCam;
  public Button attack;
  public GameObject monster;
  public GameObject healthBar;

  void OnTriggerExit2D(Collider2D col) {
    if (col.gameObject.name.Equals("player") && !virtualCam.activeInHierarchy) {
      virtualCam.SetActive(true);
      attack.gameObject.SetActive(true);
      monster.GetComponent<MonsterMove>().enabled = true;
      healthBar.SetActive(true);
    }
  }
}
