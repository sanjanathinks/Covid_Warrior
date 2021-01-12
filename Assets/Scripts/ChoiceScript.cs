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
    public bool ChoiceMade;
    public GameObject questionBoard;

    private TextMeshProUGUI aText;
    private TextMeshProUGUI bText;
    private TextMeshProUGUI cText;
    private TextMeshProUGUI dText;
    private bool changed;

    void Start() {
      ChoiceMade = false;
      aText = Choice01.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      bText = Choice02.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      cText = Choice03.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      dText = Choice04.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      questionBoard.SetActive(false);
    }

    void FixedUpdate() {
      if (this.GetComponent<Question>().getQuestion() != null && !TextBox.text.Equals(this.GetComponent<Question>().getQuestion()) && changed) {
        questionBoard.SetActive(true);
        ChoiceMade = false;
        TextBox.text = this.GetComponent<Question>().getQuestion();
        aText.text = this.GetComponent<Question>().getA();
        bText.text = this.GetComponent<Question>().getB();
        cText.text = this.GetComponent<Question>().getC();
        dText.text = this.GetComponent<Question>().getD();
        changed = false;
      }
    }

    public void newQuestion() {
      //TODO: will want this to pull the parameters from the level you're on or the player you are
      this.GetComponent<Question>().generateQuestion("math", "intermediate");
      changed = true;
    }

    public void ChoiceOption(string choice) {
      Debug.Log(choice);
      Debug.Log(this.GetComponent<Question>().correctAnswer());
      if (choice.Equals(this.GetComponent<Question>().correctAnswer())) {
        GetComponent<Question>().answeredQuestion(1);
        TextBox.text = "Good choice!";
      }
      else {
        GetComponent<Question>().answeredQuestion(-1);
        TextBox.text = "Bad choice!";
      }
      ChoiceMade = true;
    }

    void Update() {
        if (ChoiceMade) {
            Choice01.SetActive(false);
            Choice02.SetActive(false);
            Choice03.SetActive(false);
            Choice04.SetActive(false);
        }
        else {
          Choice01.SetActive(true);
          Choice02.SetActive(true);
          Choice03.SetActive(true);
          Choice04.SetActive(true);
        }
    }

}
