using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace UserStatsService {
  public class HttpRequestSender : MonoBehaviour, IRequestSender {
    private static string BASE_URL = "http://localhost:3000";

    public static HttpRequestSender GetInstance () {
      return GameObject.Find ("NetworkManager").GetComponent<HttpRequestSender> ();
    }

    public void Send (string uri, object model, Action<string> cb) {
      StartCoroutine (SendHttpRequest(uri, model, cb));
    }

    private IEnumerator SendHttpRequest (string uri, object model, Action<string> cb) {
      UnityWebRequest req = UnityWebRequest.Post (BASE_URL + uri, JsonUtility.ToJson(model));

      req.SetRequestHeader ("Content-Type", "application/json");

      yield return req.Send ();

      if(req.isNetworkError || req.isHttpError) {
          Debug.Log(req.error);
      }
      else {
          cb(req.downloadHandler.text);
      }
    }
  }
}
