using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UserStatsService;

public class PlayersHub : NetworkBehaviour {
	private List<CarController> players = new List<CarController> ();
	private int count = 0;
	private static int COINS_COUNT = 10;
	private static int MOTOR_TORQUE_INCREMENT = 100;
	private static int SUPER_MOTOR_TORQUE_INCREMENT = 1000;
	private float time;

	public GameObject achievementPrefab;
	public GameObject superAchievementPrefab;

	IApiInterface api;

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

		api = new HttpApiLogic (HttpRequestSender.GetInstance ());
	}

	public static PlayersHub GetInstance () {
		return GameObject.Find ("PlayersHub").GetComponent<PlayersHub> ();
	}

	public static void AddPlayer (CarController player) {
		GetInstance ().AddPlayerToInstance (player);
	}

	public void DoLogin (UserCredentialsModel model, NetworkInstanceId netId) {
		if (model.isNewPlayer) {
			api.DoLogin (model, (LoginResultModel response) => {
				var player = GetPlayerById (netId);

				player.RpcUpdateStatsTable (response);
				player.login = model.login;
				player.token = response.token;
			});
		} else {
			api.DoSignIn (model, (SignInResultModel response) => {
				var player = GetPlayerById (netId);

				player.login = model.login;
				player.token = response.token;
			});
		}
	}

	public void PostResult (NetworkInstanceId netId, int place) {
		var player = GetPlayerById (netId);

		api.DoPostRaceResult (new RaceResultModel {
			login = player.login,
			token = player.token,
			place = place,
			time = time
		}, (LoginResultModel model) => {
			player.RpcUpdateStatsTable (model);
		});
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
		this.time = time;
		foreach (CarController player in players) {
			if (player != null) {
				player.RpcUpdateTimeText (time);
			}
		}
	}

	public void UpdateFinishTextById (NetworkInstanceId id, int position) {
		GetPlayerById (id).RpcSetFinishText (position);
	}

	public void ApplyAchievement (NetworkInstanceId id, bool isSuper) {
		var increment = isSuper ? SUPER_MOTOR_TORQUE_INCREMENT : MOTOR_TORQUE_INCREMENT;
		GetPlayerById (id).RpcIncreaseMotorTorque (increment);
	}

	private CarController GetPlayerById (NetworkInstanceId id) {
		return NetworkServer.FindLocalObject (id).GetComponent<CarController> ();
	}

	private void AddPlayerToInstance (CarController player) {
		players.Add (player);
		count++;
	}
}
