using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 1.5f;
    public bool newUser = false;
    public bool downloading = false;
    public int health = 10;
    public int maxHealth = 10;

    private Vector2 _movement;
    private PlayerData _playerData;
    private List<string> qCorrect;
    private List<string> qIncorrect;
    private List<string> qIncorrectRecent;

    void Awake() {
      GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

      if (objs.Length > 1)
      {
          Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        _playerData = new PlayerData();
        qCorrect = new List<string>();
        qIncorrect = new List<string>();
        qIncorrectRecent = new List<string>();
    }

    public void answered(string questionID, int correct) {
      if (correct > 0) qCorrect.Add(questionID);
      else {
        qIncorrect.Add(questionID);
        qIncorrectRecent.Add(questionID);
      }
    }

    public void updateUser() {
      _playerData.questions_correct = qCorrect.ToArray();
      _playerData.questions_incorrect = qIncorrect.ToArray();
      StartCoroutine(UpdateInfo(_playerData.Stringify(), updated => {
        Debug.Log(updated);
      }));
    }

    public void chooseUsername(string username) {
      _playerData.username = username;
      StartCoroutine(Download(_playerData.username, result => {
        Debug.Log(result);
        if (result == null) {
          newUser = true;
        }
      }));
    }

    public void enterClassCode(string classCode) {
      _playerData.class_code = classCode;
    }

    public void signup() {
      if (newUser && _playerData.class_code != null) {
        StartCoroutine(Upload(_playerData.Stringify(), added => {
          Debug.Log(added);
          SceneManager.LoadScene("Game");
        }));
      }
      else if (newUser) {
        Debug.Log("Please enter a class code.");
      }
      else {
        Debug.Log("Username already exists. Please select a different username or log in.");
      }
    }

    public void loginUsername(string username) {
      _playerData.username = username;
    }

    public void login() {
      StartCoroutine(Download(_playerData.username, result => {
        if (result == null) {
          Debug.Log("No registered user with this username. Please sign up or use a different username.");
        }
        else {
          _playerData = result;
          qCorrect.AddRange(_playerData.questions_correct);
          qIncorrect.AddRange(_playerData.questions_incorrect);
          Debug.Log(_playerData.Stringify());
          SceneManager.LoadScene("Game");
        }
      }));
    }

    public string getAnswered() {
      string correct = "";
      string incorrectRecent = "";
      if (qCorrect != null && qCorrect.Count > 0) {
        correct = string.Join(",", qCorrect);
      }
      if (qIncorrectRecent != null && qIncorrectRecent.Count > 0) {
        incorrectRecent = string.Join(",", qIncorrectRecent);
      }
      if (correct.Length > 0 && incorrectRecent.Length > 0) {
        return correct + "," +incorrectRecent;
      }
      else if (correct.Length > 0) {
        return correct;
      }
      else if (incorrectRecent.Length > 0) {
        return incorrectRecent;
      }
      else return null;
    }

    public string getIncorrectRecent() {
      if (qIncorrectRecent != null && qIncorrectRecent.Count > 0) {
        return string.Join(",", qIncorrectRecent);
      }
      else return null;
    }

    public string getUsername() {
      return _playerData.username;
    }

    IEnumerator Download(string id, System.Action<PlayerData> callback = null)
    {
      downloading = true;
      using (UnityWebRequest request = UnityWebRequest.Get("https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/getUser?username=" + id))
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
                callback.Invoke(PlayerData.Parse(request.downloadHandler.text));
            }
          }
      }
      downloading = false;
    }

    IEnumerator Upload(string profile, System.Action<string> callback = null)
    {
        Debug.Log(profile);
        using (UnityWebRequest request = new UnityWebRequest("https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/addUser", "POST"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
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

    IEnumerator UpdateInfo(string profile, System.Action<string> callback = null)
    {
        Debug.Log(profile);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(profile);
        using (UnityWebRequest request = UnityWebRequest.Put("https://webhooks.mongodb-realm.com/api/client/v2.0/app/covidwarrior-xhivn/service/CovidWarriorInfo/incoming_webhook/updateUser", bodyRaw))
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
