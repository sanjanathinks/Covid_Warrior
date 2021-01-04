using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public float speed = 1.5f;
    public bool newUser = false;
    public bool downloading = false;

    //private Rigidbody2D _rigidBody2D;
    private Vector2 _movement;
    private PlayerData _playerData;
    private List<string> qCorrect;
    private List<string> qIncorrect;

    void Start()
    {
        //_rigidBody2D = GetComponent<Rigidbody2D>();
        _playerData = new PlayerData();
        qCorrect = new List<string>();
        qIncorrect = new List<string>();
    }

    void Update()
    {
        // Mouse and keyboard input logic here ...
    }

    void FixedUpdate() {
        // Physics related updates here ...
    }

    public void updateUser() {
      _playerData.questions_correct = qCorrect.ToArray();
      _playerData.questions_incorrect = qIncorrect.ToArray();
      StartCoroutine(Update(_playerData.Stringify(), updated => {
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
        }));
      }
      else if (newUser) {
        Debug.Log("Please enter a class code.");
      }
      else {
        Debug.Log("Username already exists. Please select a different username or log in.");
      }
    }

    public void login(string username) {
      _playerData.username = username;
      StartCoroutine(Download(_playerData.username, result => {
        if (result == null) {
          Debug.Log("No registered user with this username. Please sign up or use a different username.");
        }
        else {
          _playerData = result;
          qCorrect.AddRange(_playerData.questions_correct);
          qIncorrect.AddRange(_playerData.questions_incorrect);
          Debug.Log(_playerData.Stringify());
        }
      }));
    }

    public string getCorrect() {
      if (_playerData.questions_correct != null && _playerData.questions_correct.Length > 0) {
        return string.Join(",", _playerData.questions_correct);
      }
      else return null;
    }

    public void addCorrect(string q) {
      qCorrect.Add(q);
    }

    public void addIncorrect(string q) {
      qIncorrect.Add(q);
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

    IEnumerator Update(string profile, System.Action<string> callback = null)
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
