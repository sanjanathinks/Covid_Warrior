using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ChoiceScript : MonoBehaviour
{
    public TextMeshProUGUI TextBox;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public GameObject Choice04;
    public GameObject questionBoard;
    public TextMeshProUGUI solutionText;
    public Image solutionImage;
    public VideoPlayer solutionVideo;
    public Button next;
    public RawImage videoRender;

    private TextMeshProUGUI aText;
    private TextMeshProUGUI bText;
    private TextMeshProUGUI cText;
    private TextMeshProUGUI dText;
    private bool changed;
    private Sprite imageSprite;

    void Start() {
      aText = Choice01.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      bText = Choice02.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      cText = Choice03.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      dText = Choice04.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      questionBoard.SetActive(false);
    }

    void FixedUpdate() {
      if (GetComponent<Question>().getQuestion() != null && !TextBox.text.Equals(GetComponent<Question>().getQuestion()) && changed) {
        questionBoard.SetActive(true);
        Choice01.SetActive(true);
        Choice02.SetActive(true);
        Choice03.SetActive(true);
        Choice04.SetActive(true);
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
      imageSprite = Resources.Load<Sprite>("images/testImage");
    }

    public void ChoiceOption(string choice) {
      if (choice.Equals(GetComponent<Question>().correctAnswer())) {
        //TODO: set close button, see full solution if want to
        GetComponent<Question>().answeredQuestion(1);
        TextBox.text = "That's right! Here's the full solution:";
      }
      else {
        //TODO: show solution
        GetComponent<Question>().answeredQuestion(-1);
        TextBox.text = "That's not the right answer. Take a look at the solution:";
      }

      Choice01.SetActive(false);
      Choice02.SetActive(false);
      Choice03.SetActive(false);
      Choice04.SetActive(false);

      solutionText.gameObject.SetActive(true);
      solutionImage.gameObject.SetActive(true);
      solutionImage.sprite = imageSprite;
      solutionVideo.url = "https://github.com/sanjanathinks/Covid-Warrior/blob/selena/Assets/stockVideo.mp4?raw=true";
      next.gameObject.SetActive(true);
    }

    public void nextClicked() {
      if (next.GetComponentInChildren<TextMeshProUGUI>().text.Equals("Next")) {
        solutionVideo.gameObject.SetActive(true);
        videoRender.gameObject.SetActive(true);

        solutionText.gameObject.SetActive(false);
        solutionImage.gameObject.SetActive(false);
        next.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
      }
      else if (next.GetComponentInChildren<TextMeshProUGUI>().text.Equals("Close")) {
        solutionVideo.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
        videoRender.gameObject.SetActive(false);
        questionBoard.gameObject.SetActive(false);
        //move player and monster away from each other
        PlayerMovement.gameIsPaused = false;
      }
    }

    void Update() {

    }

}
