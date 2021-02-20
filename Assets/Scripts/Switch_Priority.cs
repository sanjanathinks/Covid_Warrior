using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Switch_Priority : MonoBehaviour
{
    //[SerializeField]
    public CinemachineVirtualCamera vcam1; //CM vcam1
    //[SerializeField]
    public CinemachineVirtualCamera vcam2; //CM vcam2

<<<<<<< HEAD
    //private bool in_vcam1 = true;

    /*
    void Start()
    {
        //action.performed += _ => SwitchP();
    }*/

    void OnTriggerEnter2D(Collider2D col)
    {
        //Renderer render = GetComponent<Renderer>();
=======
    void OnTriggerEnter2D(Collider2D col)
    {
>>>>>>> gin1/15/21
        if(col.gameObject.tag.Equals("Player")) {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
<<<<<<< HEAD
=======
    }

    void OnTriggerExit2D(Collider2D col) {
         if(col.gameObject.tag.Equals("Player")) {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
         }
>>>>>>> gin1/15/21
    }
    
    void OnTriggerExit2D(Collider2D col) {
         //Renderer render = GetComponent<Renderer>();
         if(col.gameObject.tag.Equals("Player")) {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
         }
    }
    /*
    void Update() {
        if (Input.GetKeyDown("0")) {
            //print("space key was pressed");
            SwitchP();
        }
    }*/
}
