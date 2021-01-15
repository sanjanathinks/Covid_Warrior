using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int level_number;

    public int beginner_correct = 0;
    public int intermediate_correct = 0;
    public int advanced_correct = 0;

    public int beginner_incorrect = 0;
    public int intermediate_incorrect = 0;
    public int advanced_incorrect = 0;

    public Level(int num) {
      level_number = num;
    }

    public int getLevelStats(string variableName) {
      return (int)this.GetType().GetField(variableName).GetValue(this);
    }
}
