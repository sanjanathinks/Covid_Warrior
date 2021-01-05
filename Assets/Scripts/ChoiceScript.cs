using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceScript : MonoBehaviour
{
    public TextMeshProUGUI TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public GameObject Choice04;
    public int ChoiceMade;

    public void ChoiceOption0() {
        TextBox.text = "Good choice!";
        ChoiceMade = 0;
    }
    public void ChoiceOption1() {
        //Debug.Log(TextBox.GetComponent<TextMeshPro>());
        TextBox.text = "Bad choice!";
        ChoiceMade = 1;
    }
    public void ChoiceOption2() {
        TextBox.text = "Bad choice!";
        ChoiceMade = 2;
    }
    public void ChoiceOption3() {
        TextBox.text = "Bad choice!";
        ChoiceMade = 3;
    }

    void Update() {
        if (ChoiceMade >= 0) {
            Choice01.SetActive(false);
            Choice02.SetActive(false);
            Choice03.SetActive(false);
            Choice04.SetActive(false);
        }
    }

}
