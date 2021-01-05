using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SignUp : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
      player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<Player>().downloading) {
          this.gameObject.GetComponent<Button>().interactable = false;
          this.gameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        else {
          this.gameObject.GetComponent<Button>().interactable = true;
          this.gameObject.GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>().text = "Sign up";
        }
    }
}
