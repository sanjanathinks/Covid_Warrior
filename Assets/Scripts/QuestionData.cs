using UnityEngine;

public class QuestionData
{
  public string id;
  public string question;
  public string type;
  public int level;
  public string a;
  public string b;
  public string c;
  public string d;
  public string correct;
  public string solution;
  public int bridges_unit;
  public string nc_standard;

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
