using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayersHub : NetworkBehaviour {
	private List<CarController> players = new List<CarController> ();
	private int count = 0;
	private static int COINS_COUNT = 10;

	public GameObject achievementPrefab;
	public GameObject superAchievementPrefab;

	public int Count {
		get {
			return count;
		}
	}

	public override void OnStartServer () {
		var randomPositions = AchievementController.GetRandomPositions (COINS_COUNT);

		foreach (Vector2 position in randomPositions) {
			NetworkServer.Spawn (
				Instantiate (
					achievementPrefab,
					new Vector3 (position.x, 1.5f, position.y),
					Quaternion.AngleAxis(45, Vector3.up) * Quaternion.AngleAxis(90, Vector3.forward)
				)
			);
		}

		var superPosition = AchievementController.GetPoint();
		NetworkServer.Spawn (
			Instantiate (
				superAchievementPrefab,
				new Vector3 (superPosition.x, 1.5f, superPosition.y),
				Quaternion.AngleAxis(45, Vector3.up) * Quaternion.AngleAxis(90, Vector3.forward)
			)
		);
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
		return NetworkServer.FindLocalObject (id).GetComponent<CarController> ();
	}

	private void AddPlayerToInstance (CarController player) {
		players.Add (player);
		count++;
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
  }
}
