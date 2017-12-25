using System;

namespace UserStatsService {
  public interface IApiInterface {
    void DoLogin (UserCredentialsModel model, Action<LoginResultModel> cb);
    void DoSignIn (UserCredentialsModel model, Action<SignInResultModel> cb);
    void DoPostRaceResult (RaceResultModel model, Action<LoginResultModel> cb);
  }

  [Serializable]
  public class UserCredentialsModel {
    public string login;
    public string password;
  }

  [Serializable]
  public class RaceResultModel {
    public string login;
    public string token;
    public int place;
    public float time;
  }

  [Serializable]
  public class LoginResultModel {
    public string token;
    RaceResultModel[] results;
  }

  [Serializable]
  public class SignInResultModel {
    public string token;
  }
}
