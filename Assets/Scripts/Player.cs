using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed = 1.5f;

    //private Rigidbody2D _rigidBody2D;
    private Vector2 _movement;
    private PlayerData _playerData;

    void Start()
    {
        //_rigidBody2D = GetComponent<Rigidbody2D>();
        _playerData = new PlayerData();
        _playerData.username = "example";
        /*StartCoroutine(Download(_playerData.username, result => {
          Debug.Log(result.Stringify());
        }));*/
    }

    void Update()
    {
        // Mouse and keyboard input logic here ...
    }

    void FixedUpdate() {
        // Physics related updates here ...
        if (Input.GetKeyDown("space"))
        {
          StartCoroutine(Download(_playerData.username, result => {
            Debug.Log(result);
            if (result == null) {
              StartCoroutine(Upload(_playerData.Stringify(), added => {
                Debug.Log(added);
              }));
            }
            else {
              Debug.Log("Username already exists. Please select a different username or log in.");
            }
          }));
        }
    }

    IEnumerator Download(string id, System.Action<PlayerData> callback = null)
    {
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
}
