using UnityEngine;

public class PlayerData
{
  public string username;
  public string class_code;
  public string[] questions_correct;
  public string[] questions_incorrect;
  public string progress;

  public string Stringify()
  {
      return JsonUtility.ToJson(this);
  }

  public static PlayerData Parse(string json)
  {
      return JsonUtility.FromJson<PlayerData>(json);
  }
}
