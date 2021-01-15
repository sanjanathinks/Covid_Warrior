using System.Collections;
using System.Collections.Generic;
using System;
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
    public Button videoPlay;
    public List<Button> videoControls;
    public Button attackButton;

    private TextMeshProUGUI aText;
    private TextMeshProUGUI bText;
    private TextMeshProUGUI cText;
    private TextMeshProUGUI dText;
    private bool changed;
    private Sprite imageSprite;
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
        PlayerMovement.gameIsPaused = true;
      }
    }

    public void newQuestion() {
      //TODO: will want this to pull the parameters from the level you're on or the player you are
      GetComponent<Question>().generateQuestion("math", "beginner");
      changed = true;
      imageSprite = Resources.Load<Sprite>("images/testImage");
    }

    public void ChoiceOption(string choice) {
      if (choice.Equals(GetComponent<Question>().correctAnswer())) {
        GetComponent<Question>().answeredQuestion(1);
        TextBox.text = "That's right! Here's the full solution:";
        foreach(GameObject monster in allMonsters) {
          if (monster.GetComponent<SpriteRenderer>().isVisible) {
            monster.GetComponent<Monster>().health+=-2;
          }
        }
      }
      else {
        GetComponent<Question>().answeredQuestion(-1);
        TextBox.text = "That's not the right answer. Take a look at the solution:";
        player.GetComponent<Player>().health+=-2;
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
        foreach (Button b in videoControls) {
          b.gameObject.SetActive(true);
        }

        solutionText.gameObject.SetActive(false);
        solutionImage.gameObject.SetActive(false);
        next.GetComponentInChildren<TextMeshProUGUI>().text = "Close";
      }
      else if (next.GetComponentInChildren<TextMeshProUGUI>().text.Equals("Close")) {
        next.GetComponentInChildren<TextMeshProUGUI>().text = "Next";

        next.gameObject.SetActive(false);
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
    }

}
