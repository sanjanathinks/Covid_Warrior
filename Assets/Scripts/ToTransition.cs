using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTransition : MonoBehaviour
{
    public int level;

    void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.name.Equals("player")) SceneManager.LoadScene("transition" + level);
    }
}
