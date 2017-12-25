using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UserStatsService;

public class FinishController : NetworkBehaviour {
	private int winnersCount;
	private PlayersHub hub;

	public override void OnStartServer () {
		winnersCount = 0;
		hub = PlayersHub.GetInstance ();
	}

	void OnTriggerEnter (Collider other)
	{
		CmdRegisterFinisher(other.GetComponentInParent<NetworkIdentity> ().netId);
	}

	[Command]
	void CmdRegisterFinisher (NetworkInstanceId netId) {
		lock (this) {
			hub.SetRaceActivityById (netId, false);
			hub.UpdateFinishTextById (netId, ++winnersCount);
			hub.PostResult (netId, winnersCount);
		}
	}
}
