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
      if (GetComponent<Question>().getQuestion() != null && !TextBox.text.Equals(GetComponent<Question>().getQuestion()) && changed) {
        questionBoard.SetActive(true);
        ChoiceMade = false;
        TextBox.text = GetComponent<Question>().getQuestion();
        aText.text = GetComponent<Question>().getA();
        bText.text = GetComponent<Question>().getB();
        cText.text = GetComponent<Question>().getC();
        dText.text = GetComponent<Question>().getD();
        changed = false;
      }
    }

    public void newQuestion() {
      //TODO: will want this to pull the parameters from the level you're on or the player you are
      GetComponent<Question>().generateQuestion("math", "intermediate");
      changed = true;
    }

    public void ChoiceOption(string choice) {
      if (choice.Equals(GetComponent<Question>().correctAnswer())) {
        //TODO: set close button, see full solution if want to
        GetComponent<Question>().answeredQuestion(1);
        TextBox.text = "Good choice!";
      }
      else {
        //TODO: show solution
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
