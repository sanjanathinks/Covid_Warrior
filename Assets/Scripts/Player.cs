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
        _playerData.username = "test";
        StartCoroutine(Download(_playerData.username, (result) => {
          Debug.Log(result.Stringify());
        }));
    }

    void Update()
    {
        // Mouse and keyboard input logic here ...
    }

    void FixedUpdate() {
        // Physics related updates here ...
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
              if (callback != null)
              {
                  callback.Invoke(PlayerData.Parse(request.downloadHandler.text));
              }
          }
      }
  }
}
