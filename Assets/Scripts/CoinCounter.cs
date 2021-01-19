using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    private TextMeshProUGUI coinCounter;
    private GameObject player;

    void Start()
    {
      coinCounter = GetComponent<TextMeshProUGUI>();
      player = GameObject.Find("player");
    }

    void Update()
    {
      int count = player.GetComponent<Player>().coinCount();
      if (!coinCounter.text.Equals("" + count)) {
        if (count < 10) {
          coinCounter.text = "x0" + count;
        } else {
          coinCounter.text = "x" + count;
        }
      }
    }
}
