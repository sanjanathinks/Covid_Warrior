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

    private TextMeshProUGUI aText;
    private TextMeshProUGUI bText;
    private TextMeshProUGUI cText;
    private TextMeshProUGUI dText;
    private string original;
    private bool changed;

    void Start() {
      original = TextBox.text;
      aText = Choice01.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      bText = Choice02.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      cText = Choice03.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      dText = Choice04.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
    }

    void FixedUpdate() {
      if (Input.GetKeyDown("up")) {
        this.GetComponent<Question>().generateQuestion("math", "beginner");
        changed = true;
      }
      else if (Input.GetKeyDown("down")) {
        this.GetComponent<Question>().generateQuestion("math", "intermediate");
        changed = true;
      }

      if (this.GetComponent<Question>().getQuestion() != null && TextBox.text.Equals(original) && changed) {
        TextBox.text = this.GetComponent<Question>().getQuestion();
        aText.text = this.GetComponent<Question>().getA();
        bText.text = this.GetComponent<Question>().getB();
        cText.text = this.GetComponent<Question>().getC();
        dText.text = this.GetComponent<Question>().getD();
        changed = false;
        original = TextBox.text;
      }
    }

    public void ChoiceOption0() {
        TextBox.text = "Good choice!";
        ChoiceMade = 0;
    }
    public void ChoiceOption1() {
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
