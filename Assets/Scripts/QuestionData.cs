using UnityEngine;

[System.Serializable]
public class QuestionData
{
  public string id;
  public string question;
  public string type;
  public string img_q;
  public string level;
  public string a;
  public string b;
  public string c;
  public string d;
  public string correct;
  public string solution;
  public string img_s;
  public string explain_record;
  public int bridges_unit;
  public string bridges_title;
  public string topic;
  public string nc_standard;
  public string nc_description;
  public string[] nc_standard_all;
  public string[] nc_description_all;

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

      start = json.IndexOf("bridges_unit\":{\"$numberInt\":\"") + "bridges_unit\":{\"$numberInt\":\"".Length;
      end = json.IndexOf("\"},\"bridges_title");
      Debug.Log(json.Substring(start, end - start));
      q.bridges_unit = int.Parse(json.Substring(start, end - start));

      q.nc_standard_all = q.nc_standard.Split('\\');
      q.nc_description_all = q.nc_description.Split('\\');

      return q;
  }
}
