using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Level : MonoBehaviour
{
    public int level_number;

    public int beginner_correct = 0;
    public int intermediate_correct = 0;
    public int advanced_correct = 0;

    public int beginner_incorrect = 0;
    public int intermediate_incorrect = 0;
    public int advanced_incorrect = 0;

    public Vector3 playerStartPosition;
    public Vector3 playerCheckpoint1;
    public Vector3 playerCheckpoint2;

    public TextMeshProUGUI levelNum;

    void Start() {
      levelNum.text = "Level " + level_number;
    }

    public int getLevelStats(string variableName) {
      return (int)this.GetType().GetField(variableName).GetValue(this);
    }
}
