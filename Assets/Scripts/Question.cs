using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using System.Reflection;

public class Question : MonoBehaviour
{
    private QuestionData _questionData;
    private GameObject player;
    private bool generating;

    public Level currentLevel;
    public int level_number;

    void Start()
    {
      player = GameObject.Find("player");
      _questionData = new QuestionData();
      currentLevel = new Level(level_number);
    }

    public void generateQuestion(string type, string level) {
      if (!generating) {
        string ans = player.GetComponent<Player>().getAnswered();
        Debug.Log(ans);
        _questionData.type = type;
        _questionData.level = level;
        StartCoroutine(Download(_questionData.type, _questionData.level, ans, result => {
          _questionData = result;
          Debug.Log(_questionData.Stringify());
        }));
      }
    }

    public void answeredQuestion(int correct) {
      //>0 indicates answered correct, <0 incorrect
      string variableName;
      if (correct > 0) {
        variableName = _questionData.level + "_correct";
      }
      else {
        variableName = _questionData.level + "_incorrect";
      }
      FieldInfo fieldInfo = currentLevel.GetType().GetField(variableName);
      fieldInfo.SetValue(currentLevel, currentLevel.getLevelStats(variableName)+1);

      player.GetComponent<Player>().answered(_questionData.id, correct);
      string info = "{\"questionID\":\"" + _questionData.id +"\", \"correct\":" + correct + "}";
      StartCoroutine(Answer(info, result => {
        Debug.Log(result);
      }));
    }

    public string getQuestion() {
      return _questionData.question;
    }

    public string getA() {
      return _questionData.a;
    }

    public string getB() {
      return _questionData.b;
    }

    public string getC() {
      return _questionData.c;
    }

    public string getD() {
      return _questionData.d;
    }

    public string correctAnswer() {
      return _questionData.correct;
    }

    IEnumerator Download(string type, string level, string correct, System.Action<QuestionData> callback = null)
    {
      generating = true;
      string url;
      if (correct != null) {
        url = "https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/getQuestion?select_type=" + type + "&select_level=" + level + "&answered=" + correct;
      }
      else {
        url = "https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/getQuestion?select_type=" + type + "&select_level=" + level;
      }
      using (UnityWebRequest request = UnityWebRequest.Get(url))
      {
          yield return request.SendWebRequest();

          if (request.isNetworkError || request.isHttpError)
          {
              Debug.Log(request.error);
              if (callback != null)
              {
                  callback.Invoke(null);
              }
          }
          else
          {
            if (request.downloadHandler.text == "null")
            {
              callback.Invoke(null);
            }
            else if (callback != null)
            {
                callback.Invoke(QuestionData.Parse(request.downloadHandler.text.Substring(1, request.downloadHandler.text.Length - 2)));
            }
          }
      }
      generating = false;
    }

    IEnumerator Answer(string info, System.Action<string> callback = null)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(info);
        using (UnityWebRequest request = UnityWebRequest.Put("https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/answered", bodyRaw))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                if(callback != null)
                {
                    callback.Invoke("false");
                }
            }
            else
            {
                if(callback != null)
                {
                    callback.Invoke(request.downloadHandler.text);
                }
            }
        }
    }
}
