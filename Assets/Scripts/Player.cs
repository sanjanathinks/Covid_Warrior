using UnityEngine;
using System.Text;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

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
    private TextMeshProUGUI error;

    void Awake() {
      GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");

      if (objs.Length > 1)
      {
          Destroy(this.gameObject);
      }
      DontDestroyOnLoad(this.gameObject);

      _playerData = new PlayerData();
      qCorrect = new List<string>();
      qIncorrect = new List<string>();
      qIncorrectRecent = new List<string>();
    }

    //need this and OnSceneLoaded because object doesn't destroy
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      if (scene.name.Contains("level")) {
        LevelLoaded();
      } else {
        error = GameObject.Find("ErrorMessage").GetComponent<TextMeshProUGUI>();
      }
    }

    private void LevelLoaded() {
      SetCameraFollow();
      GameObject usernameText = GameObject.Find("username");
      if (usernameText!=null && _playerData!=null) {
        usernameText.GetComponent<TextMeshProUGUI>().text = _playerData.username;
      }

      if (_playerData.progress!=null && _playerData.progress.Length > 1) {
        string location = _playerData.progress.Substring(2, 1);
        if (location.Equals("1")) {
          transform.position = GameObject.Find("GameObject").GetComponent<Level>().playerCheckpoint1;
        } else if (location.Equals("2")) {
          transform.position = GameObject.Find("GameObject").GetComponent<Level>().playerCheckpoint2;
        } else if (location.Equals("3")) {
          transform.position = GameObject.Find("GameObject").GetComponent<Level>().playerCheckpoint3;
        }
      }
      else {
        transform.position = GameObject.Find("GameObject").GetComponent<Level>().playerStartPosition;
      }
    }

    private void SetCameraFollow() {
      foreach (GameObject cam in GameObject.FindGameObjectsWithTag("vcam")) {
        cam.GetComponent<CinemachineVirtualCamera>().Follow = gameObject.transform;
        Debug.Log(cam);
        /**if (!cam.name.Equals("CM vcam1")) {
          cam.SetActive(false);
        }*/ //TODO: don't know if need this section - depends on how zoom cameras are handled
      }
    }

    public int coinCount() {
      return _playerData.coins;
    }

    public void coinCount(int add) {
      _playerData.coins+=add;
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

    public void updateUser(string prog) {
      _playerData.progress = prog;
      updateUser();
    }

    public void chooseUsername(string username) {
      _playerData.username = username;
      StartCoroutine(Download(_playerData.username, result => {
        Debug.Log(result);
        if (result == null) {
          newUser = true;
        } else {
          newUser = false;
        }
      }));
    }

    public void enterClassCode(string classCode) {
      _playerData.class_code = classCode;
    }

    public string getProgress() {
      return _playerData.progress;
    }

    public void signup() {
      if (newUser && _playerData.class_code.Length > 0) {
        _playerData.progress = "1";
        StartCoroutine(Upload(_playerData.Stringify(), added => {
          Debug.Log(added);
          SceneManager.LoadScene("level1");
        }));
      }
      else if (newUser) {
        error.text = "Please enter a class code.";
        Debug.Log("Please enter a class code.");
      }
      else {
        error.text = "Username already exists. Please select a different username or log in.";
        Debug.Log("Username already exists. Please select a different username or log in.");
      }
    }

    public void loginUsername(string username) {
      _playerData.username = username;
    }

    public void login() {
      StartCoroutine(Download(_playerData.username, result => {
        if (result == null) {
          error.text = "No registered user with this username. Please sign up or use a different username.";
          Debug.Log("No registered user with this username. Please sign up or use a different username.");
        }
        else {
          _playerData = result;
          qCorrect.AddRange(_playerData.questions_correct);
          qIncorrect.AddRange(_playerData.questions_incorrect);
          Debug.Log(_playerData.Stringify());
          SceneManager.LoadScene("level" + _playerData.progress.Substring(0,1)); //TODO: change this to be based on player info
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
              Debug.Log(request.downloadHandler.text);
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
