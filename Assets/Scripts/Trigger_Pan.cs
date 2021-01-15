using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trigger_Pan : MonoBehaviour
{
    
   // public GameObject popUpBox;

    // Start is called before the first frame update
    void Start()
    {
        //popUpBox.setActive(false);
    }

    private Color old = Color.grey;
    void OnTriggerEnter2D(Collider2D col)
    {
        Renderer render = GetComponent<Renderer>();
        //old = render.material.color;
        render.material.color = Color.red;

        //if(col.GameObject.tag == "Player") {
           // popUpBox.setActive(true);
        //} 

    }
    void OnTriggerExit2D(Collider2D other) {
        //Debug.Log(old);
        Renderer render = GetComponent<Renderer>();
        render.material.color = old;

       // if(other.GameObject.tag == "Player") {
        //    popUpBox.setActive(false);
       // } 
    }
    
}

// when you set old to a render.material - the behavior is a pointer.
// if you set old = new color and put render.material.color, it should make a separate object that stays grey.

// want window to close with a different key
// one key to open chest, another key to close pop up