using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Switch_Priority : MonoBehaviour
{
    //[SerializeField]
    public CinemachineVirtualCamera vcam1; //CM vcam1
    //[SerializeField]
    public CinemachineVirtualCamera vcam2; //CM vcam2

    private bool in_vcam1 = true;

    void Start()
    {
        //action.performed += _ => SwitchP();
    }
    
    void Update() {
        if (Input.GetKeyDown("0")) {
            //print("space key was pressed");
            SwitchP();
        }
    }

    private void SwitchP() {
        //Debug.Log("here 32");
        if (in_vcam1) {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
        else {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
        in_vcam1 = !in_vcam1;
    }
}
