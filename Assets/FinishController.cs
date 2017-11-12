using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FinishController : NetworkBehaviour {
	private int winnersCount;

	public override void OnStartServer () {
		winnersCount = 0;
	}

	void OnTriggerEnter (Collider other)
	{
		CmdRegisterFinisher(other.GetComponentInParent<NetworkIdentity> ().netId);
	}

	[Command]
	void CmdRegisterFinisher (NetworkInstanceId netId) {
		lock (this) {
			CarController player = NetworkServer.FindLocalObject (netId).GetComponent<CarController> ();
			player.RpcSetFinishText (++winnersCount);
			player.raceActive = false;
		}
	}
}
