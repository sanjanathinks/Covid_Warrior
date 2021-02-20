using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure_Chest : MonoBehaviour
{
    public GameObject canvas;
    public GameObject promptBckgd;
    public GameObject promptText;
    public GameObject factBckgd;
    public GameObject factText;

    void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.name.Equals("player")) {
        promptBckgd.SetActive(true);
        promptText.SetActive(true);
        canvas.SetActive(true);
      }
    }
}
