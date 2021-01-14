using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterMove : MonoBehaviour
{
  private Vector3 originalPosition;

  void Start()
  {
    originalPosition = transform.position;
  }

}
