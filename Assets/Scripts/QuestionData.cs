using UnityEngine;

public class QuestionData
{
  public string id;
  public string question;
  public string type;
  public int level;
  public string correct;
  public string[] incorrect;

  public string Stringify()
  {
      return JsonUtility.ToJson(this);
  }

  public static QuestionData Parse(string json)
  {
      QuestionData q = JsonUtility.FromJson<QuestionData>(json);
      int id_start = json.IndexOf("$oid\":\"") + "$oid\":\"".Length;
      int id_end = json.IndexOf("question") - 4;
      q.id = json.Substring(id_start, id_end - id_start);
      return q;
  }
}
