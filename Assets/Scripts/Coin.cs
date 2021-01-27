using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameObject player;
    private bool hit;

    void Start()
    {
        player = GameObject.Find("player");
    }

    void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject == player && !hit) {
        player.GetComponent<Player>().coinCount(1);
        hit = true;
        Destroy(this.gameObject);
      }
    }
}
