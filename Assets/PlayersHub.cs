using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayersHub : NetworkBehaviour {
	private List<CarController> players = new List<CarController> ();
	private int count = 0;

	public int Count {
		get {
			return count;
		}
	}

	public static PlayersHub GetInstance () {
		return GameObject.Find ("PlayersHub").GetComponent<PlayersHub> ();
	}

	public static void AddPlayer (CarController player) {
		GetInstance ().AddPlayerToInstance (player);
	}

	public void SetRaceActivity (bool isActive) {
		foreach (CarController player in players) {
			player.raceActive = isActive;
		}
	}

	public void SetRaceActivityById (NetworkInstanceId id, bool isActive) {
		CarController player = GetPlayerById (id);
		player.raceActive = isActive;
	}

	public void UpdateTimeText (float time) {
		foreach (CarController player in players) {
			player.RpcUpdateTimeText (time);
		}
	}

	public void UpdateFinishTextById (NetworkInstanceId id, int position) {
		GetPlayerById (id).RpcSetFinishText (position);
	}

	private CarController GetPlayerById (NetworkInstanceId id) {
		return NetworkServer.FindLocalObject (netId).GetComponent<CarController> ();
	}

	private void AddPlayerToInstance (CarController player) {
		players.Add (player);
		count++;
	}
}
