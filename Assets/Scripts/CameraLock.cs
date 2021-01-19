using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    public Camera MainCamera;
    private PlayerMovement playerMov;

    void Start() {
      playerMov = GameObject.Find("player").GetComponent<PlayerMovement>();
    }

    public void limitPlayerMovement() {
      //clamp player movement
      playerMov.setBattle(true, MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z)), MainCamera.transform.position);
    }
}
