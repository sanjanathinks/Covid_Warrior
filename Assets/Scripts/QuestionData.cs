using UnityEngine;

[System.Serializable]
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
      int start = json.IndexOf("$oid\":\"") + "$oid\":\"".Length;
      int end = json.IndexOf("question") - 4;
      q.id = json.Substring(start, end - start);

      start = json.IndexOf("level\":{\"$numberInt\":\"") + "level\":{\"$numberInt\":\"".Length;
      end = json.IndexOf("\"},\"correct");
      Debug.Log(json.Substring(start, end - start));
      q.level = int.Parse(json.Substring(start, end - start));

      start = json.IndexOf("bridges_unit\":{\"$numberInt\":\"") + "bridges_unit\":{\"$numberInt\":\"".Length;
      end = json.IndexOf("\"},\"nc_standard");
      Debug.Log(json.Substring(start, end - start));
      q.bridges_unit = int.Parse(json.Substring(start, end - start));

      return q;
  }
}
