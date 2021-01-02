using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Collections;

public class Question : MonoBehaviour
{
    private QuestionData _questionData;
    private GameObject player;

    void Start()
    {
      player = GameObject.Find("player");
      _questionData = new QuestionData();
    }

    void Update()
    {

    }

    void FixedUpdate() {
      if (Input.GetKeyDown("up")) {
        string ans_correct = player.GetComponent<Player>().getCorrect();
        _questionData.type = "math";
        _questionData.level = 1;
        StartCoroutine(Download(_questionData.type, _questionData.level, ans_correct, result => {
          _questionData = result;
          Debug.Log(_questionData.Stringify());
        }));
      }
    }

    IEnumerator Download(string type, int level, string correct, System.Action<QuestionData> callback = null)
    {
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
    }

}
