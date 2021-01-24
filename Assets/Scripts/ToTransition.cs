using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTransition : MonoBehaviour
{
    public int level;
    private GameObject player;

    void Start() {
      player = GameObject.Find("player");
    }

    void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.name.Equals("player")) {
        player.GetComponent<Player>().updateUser((level+1) + "");
        SceneManager.LoadScene("transition" + level);
      }
    }
}
