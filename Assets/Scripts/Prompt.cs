using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    public GameObject canvas;
    public GameObject factBckgd;
    public GameObject factText;

    void Update()
    {
        if (Input.GetKeyDown("e") && factBckgd.activeSelf) {
          factBckgd.SetActive(false);
          factText.SetActive(false);
          canvas.SetActive(false);
        } else if (Input.GetKeyDown("e")) {
          factBckgd.SetActive(true);
          factText.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col) {
      if (col.gameObject.name.Equals("player")) {
        canvas.SetActive(false);
      }
    }
}
