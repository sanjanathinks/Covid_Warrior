using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{
    public Camera MainCamera;
    private PlayerMovement playerMov;

    void Awake() {
      playerMov = GameObject.Find("player").GetComponent<PlayerMovement>();
    }

    public void limitPlayerMovement() {
      //clamp player movement
      //TODO: this works but the forces get wonky - think about setting a fixed one (in playermovement)
            //OR make a bunch of colliders and set them to active when needed
      playerMov.setBattle(true, MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z)), MainCamera.transform.position);
    }
}
