using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Switch_Priority : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera vcam1; //CM vcam1
    [SerializeField]
    private CinemachineVirtualCamera vcam2; //CM vcam2

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag.Equals("Player")) {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
    }

    void OnTriggerExit2D(Collider2D col) {
         if(col.gameObject.tag.Equals("Player")) {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
         }
    }
}
