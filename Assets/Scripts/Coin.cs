using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameObject player;
    private Animator animator;

    void Start()
    {
        player = GameObject.Find("player");
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D() {
      if (!animator.GetBool("coin_collected")) {
        animator.SetBool("coin_collected", true);
        player.GetComponent<Player>().coinCount(1);
      }
    }
}
