using UnityEngine;

public class PlayerData
{
  public string username;
  public string classCode;
  public string[] correct;
  public string[] incorrect;

  public string Stringify()
  {
      return JsonUtility.ToJson(this);
  }

  public static PlayerData Parse(string json)
  {
      return JsonUtility.FromJson<PlayerData>(json);
  }
}
