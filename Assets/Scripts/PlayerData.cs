using UnityEngine;

public class PlayerData
{
  public string username;
  public string class_code;
  public string[] questions_correct;
  public string[] questions_incorrect;
  public string progress;
  public int coins;

  public string Stringify()
  {
      return JsonUtility.ToJson(this);
  }

  public static PlayerData Parse(string json)
  {
      PlayerData p = JsonUtility.FromJson<PlayerData>(json);
      int start = json.IndexOf("$numberInt\":\"") + "$numberInt\":\"".Length;
      int end = json.IndexOf("\"}}");
      p.coins = int.Parse(json.Substring(start, end - start));
      return p;
  }
}
