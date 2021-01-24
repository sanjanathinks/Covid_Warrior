using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Statistic : MonoBehaviour
{
    public string difficulty;

    private GameObject levelStats;

    void Awake()
    {
        levelStats = GameObject.Find("LevelStats");
    }

    void Start()
    {
        int correct = levelStats.GetComponent<LevelStats>().getLevelStats(difficulty + "_correct");
        int incorrect = levelStats.GetComponent<LevelStats>().getLevelStats(difficulty + "_incorrect");
        double percent = 0;
        if (correct + incorrect > 0) {
          percent = (double)correct/(correct+incorrect)*100;
        }
        if (percent == 0) {
          GetComponent<TextMeshProUGUI>().text = "No " + difficulty + " questions in this level";
        } else {
          GetComponent<TextMeshProUGUI>().text = Char.ToUpper(difficulty[0]) + difficulty.Substring(1) +
            "  " + correct + "/" + (correct + incorrect) + " = " + Math.Round(percent, MidpointRounding.AwayFromZero) + "% correct";
        }
    }
}
