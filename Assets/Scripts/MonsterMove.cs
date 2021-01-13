using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterMove : MonoBehaviour
{
  protected bool stop = false;
  private Vector3 originalPosition;

  void Start()
  {
    originalPosition = transform.position;
  }

  public void pause(bool pause) {
    stop = pause;
  }
}
