using System;
using System.Collections;

namespace UserStatsService {
  public interface IRequestSender {
    void Send (string uri, object model, Action<string> cb);
  }
}
