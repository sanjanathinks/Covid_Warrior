using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgmt : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
      player = GameObject.Find("player");
    }

    public void goToSignup() {
      SceneManager.LoadScene("Signup");
    }

    public void goToLogin() {
      SceneManager.LoadScene("Login");
    }

    public void signup() {
      player.GetComponent<Player>().signup();
    }

    public void classCode(string code) {
      player.GetComponent<Player>().enterClassCode(code);
    }

    public void username(string name) {
      player.GetComponent<Player>().chooseUsername(name);
    }

    public void loginUsername(string name) {
      player.GetComponent<Player>().loginUsername(name);
    }

    public void login() {
      player.GetComponent<Player>().login();
    }
}
