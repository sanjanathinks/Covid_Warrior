using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneMgmt : MonoBehaviour
{
    private GameObject player;
    public TextMeshProUGUI error;

    void Start()
    {
      player = GameObject.Find("player");
    }

    public void signup() {
      player.GetComponent<Player>().signup();
    }

    public void classCode(string code) {
      player.GetComponent<Player>().enterClassCode(code);
    }

    public void username(string name) {
      player.GetComponent<Player>().chooseUsername(name);
      error.text = "";
    }

    public void loginUsername(string name) {
      player.GetComponent<Player>().loginUsername(name);
      error.text = "";
    }

    public void login() {
      player.GetComponent<Player>().login();
    }

    public void goTo(string scene) {
      SceneManager.LoadScene(scene);
    }
}
