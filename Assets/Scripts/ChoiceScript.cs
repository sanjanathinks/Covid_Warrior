using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ChoiceScript : MonoBehaviour
{
    public static bool animationFinished;

    public TextMeshProUGUI questionText;
    public RectTransform questionScroll;
    public RectTransform questionScrollbar;
    public Image questionImage;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public GameObject Choice04;
    public GameObject questionBoard;
    public TextMeshProUGUI solutionText;
    public RectTransform solutionScroll;
    public Image solutionImage;
    public VideoPlayer solutionVideo;
    public Button next;
    public Button close;
    public RawImage videoRender;
    public Button videoPlay;
    public List<Button> videoControls;
    public Button attackButton;

    private TextMeshProUGUI aText;
    private TextMeshProUGUI bText;
    private TextMeshProUGUI cText;
    private TextMeshProUGUI dText;
    public bool changed;
    private Sprite solutionSprite;
    private Sprite questionSprite;
    private GameObject[] allMonsters;
    private GameObject player;

    void Start() {
      aText = Choice01.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      bText = Choice02.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      cText = Choice03.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      dText = Choice04.transform.Find("answer").gameObject.GetComponent<TextMeshProUGUI>();
      allMonsters = GameObject.FindGameObjectsWithTag("monster");
      player = GameObject.Find("player");
      questionBoard.SetActive(false);
    }

    void FixedUpdate() {
      if (GetComponent<Question>()._questionData.question != null && !questionText.text.Equals(GetComponent<Question>()._questionData.question) && changed && animationFinished) {
        showQuestion();
      }
    }

    public static void animationIsFinished() {
      animationFinished = true;
    }

    public void showQuestion() {
      questionBoard.SetActive(true);
      Choice01.SetActive(true);
      Choice02.SetActive(true);
      Choice03.SetActive(true);
      Choice04.SetActive(true);

      questionText.text = GetComponent<Question>()._questionData.question;
      //TODO: will need to change pathing on these images
      questionSprite = Resources.Load<Sprite>("images/" + GetComponent<Question>()._questionData.img_q);
      if (questionSprite != null) {
        questionImage.gameObject.SetActive(true);
        questionImage.sprite = questionSprite;
      }
      else {
        questionScroll.sizeDelta = new Vector2(1000, 260);
        questionScrollbar.sizeDelta = new Vector2(20, 260);
      }
      Debug.Log(questionSprite);
      //if no image, want text to be bigger while question is up then need change after answer
      aText.text = GetComponent<Question>()._questionData.a;
      bText.text = GetComponent<Question>()._questionData.b;
      cText.text = GetComponent<Question>()._questionData.c;
      dText.text = GetComponent<Question>()._questionData.d;
      solutionText.text = GetComponent<Question>()._questionData.solution;
      solutionSprite = Resources.Load<Sprite>("images/" + GetComponent<Question>()._questionData.img_s);

      changed = false;
      animationFinished = false;
      PlayerMovement.gameIsPaused = true;
    }

    public void newQuestion() {
      //TODO: will want this to pull the parameters from the level you're on or the player you are
      GetComponent<Question>().generateQuestion("math", "beginner");
    }

    public void ChoiceOption(string choice) {
      questionScroll.sizeDelta = new Vector2(1000, 120);
      questionScrollbar.sizeDelta = new Vector2(20, 120);

      if (choice.Equals(GetComponent<Question>()._questionData.correct)) {
        GetComponent<Question>().answeredQuestion(1);
        questionText.text = "That's right! Here's the full solution:";
        foreach(GameObject monster in allMonsters) {
          if (monster.GetComponent<SpriteRenderer>().isVisible) {
            monster.GetComponent<Monster>().health+=-2;
          }
        }
      }
      else {
        GetComponent<Question>().answeredQuestion(-1);
        questionText.text = "That's not the right answer. Take a look at the solution:";
        player.GetComponent<Player>().health+=-2;
      }

      Choice01.SetActive(false);
      Choice02.SetActive(false);
      Choice03.SetActive(false);
      Choice04.SetActive(false);
      questionImage.gameObject.SetActive(false);

      solutionScroll.gameObject.SetActive(true);
      if (solutionSprite!=null) {
        solutionImage.sprite = solutionSprite;
        solutionImage.gameObject.SetActive(true);
      }
      solutionVideo.url = "https://github.com/sanjanathinks/Covid-Warrior/blob/selena/Assets/stockVideo.mp4?raw=true";
      next.gameObject.SetActive(true);
    }

    public void nextClicked() {
      solutionVideo.gameObject.SetActive(true);
      videoRender.gameObject.SetActive(true);
      foreach (Button b in videoControls) {
        b.gameObject.SetActive(true);
      }
      videoPlay.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";

      solutionScroll.gameObject.SetActive(false);
      solutionImage.gameObject.SetActive(false);
      next.gameObject.SetActive(false);
      close.gameObject.SetActive(true);
    }
    public void closeClicked() {
      close.gameObject.SetActive(false);
      solutionVideo.gameObject.SetActive(false);
      videoRender.gameObject.SetActive(false);
      foreach (Button b in videoControls) {
        b.gameObject.SetActive(false);
      }

      questionBoard.gameObject.SetActive(false);
      attackButton.gameObject.SetActive(true);
      Monster.timeInRange = 0.0f;

      //move monster and player away from each other
      foreach(GameObject monster in allMonsters) {
        if (monster.GetComponent<SpriteRenderer>().isVisible) {
          float monsterX = monster.transform.position.x;
          float playerX = player.transform.position.x;

          float xDist = monsterX - playerX;
          float needMove = (11.0f - Math.Abs(xDist))/2;
          monsterX+=needMove*Math.Sign(xDist);
          playerX-=needMove*Math.Sign(xDist);

          monster.transform.position = new Vector3(monsterX, monster.transform.position.y, monster.transform.position.z);
          player.transform.position = new Vector3(playerX, player.transform.position.y, player.transform.position.z);
        }
      }
      PlayerMovement.gameIsPaused = false;
    }

    public void videoControl() {
      if (videoPlay.GetComponentInChildren<TextMeshProUGUI>().text.Equals("Pause")) {
        solutionVideo.Pause();
        videoPlay.GetComponentInChildren<TextMeshProUGUI>().text = "Play";
      }
      else if (videoPlay.GetComponentInChildren<TextMeshProUGUI>().text.Equals("Play")) {
        solutionVideo.Play();
        videoPlay.GetComponentInChildren<TextMeshProUGUI>().text = "Pause";
      }
    }

    public void videoSkip(float time) {
      solutionVideo.time+=time;
      Debug.Log("video skip " + solutionVideo.time);
    }

}
