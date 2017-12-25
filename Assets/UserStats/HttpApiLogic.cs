using UnityEngine;
using UnityEngine.Networking;
using System;

namespace UserStatsService {
  public class HttpApiLogic : IApiInterface {
    private IRequestSender requestSender;

    public HttpApiLogic (IRequestSender requestSender) {
      this.requestSender = requestSender;
    }

    public void DoLogin (UserCredentialsModel model, Action<LoginResultModel> cb) {
      requestSender.Send ("/sessions", model, (json) => cb(JsonUtility.FromJson<LoginResultModel>(json)));
    }

    public void DoSignIn (UserCredentialsModel model, Action<SignInResultModel> cb) {
      requestSender.Send ("/players", model, (json) => cb(JsonUtility.FromJson<SignInResultModel>(json)));
    }

    public void DoPostRaceResult (RaceResultModel model, Action<LoginResultModel> cb) {
      requestSender.Send ("/results", model, (json) => cb(JsonUtility.FromJson<LoginResultModel>(json)));
    }
  }
}
