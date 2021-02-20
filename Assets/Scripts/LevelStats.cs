using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStats : MonoBehaviour
{
    public int level_number;

    public int beginner_correct = 0;
    public int intermediate_correct = 0;
    public int advanced_correct = 0;

    public int beginner_incorrect = 0;
    public int intermediate_incorrect = 0;
    public int advanced_incorrect = 0;

    void Awake() {
      GameObject[] objs = GameObject.FindGameObjectsWithTag("stats");

      if (objs.Length > 1)
      {
          Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
    }

    void Update() {
      if (Input.GetKeyDown("p")) {
        SceneManager.LoadScene("level2");
      }
    }

    public int getLevelStats(string variableName) {
      return (int)this.GetType().GetField(variableName).GetValue(this);
    }

    public void newLevel(int levelNum) {
      level_number = levelNum;

      beginner_correct = 0;
      intermediate_correct = 0;
      advanced_correct = 0;

      beginner_incorrect = 0;
      intermediate_incorrect = 0;
      advanced_incorrect = 0;
    }
}
