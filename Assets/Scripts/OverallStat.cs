using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class OverallStat : MonoBehaviour
{
    private GameObject levelStats;
    private string[] types = new string[]{"beginner", "intermediate", "advanced"};

    public Slider bar;

    void Awake()
    {
        levelStats = GameObject.Find("LevelStats");
    }

    void Start()
    {
        int correct = 0;
        int incorrect = 0;
        foreach (string s in types) {
          correct+=levelStats.GetComponent<LevelStats>().getLevelStats(s + "_correct");
          incorrect+=levelStats.GetComponent<LevelStats>().getLevelStats(s + "_incorrect");
        }
        double percent = (double)correct/(correct+incorrect)*100;
        GetComponent<TextMeshProUGUI>().text = correct + "/" + (correct + incorrect) + " = " + Math.Round(percent, MidpointRounding.AwayFromZero) + "% correct";
        bar.maxValue = correct + incorrect;
        bar.value = correct;
    }
}
