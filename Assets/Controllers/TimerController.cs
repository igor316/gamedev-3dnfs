﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UserStatsService;

public class TimerController : NetworkBehaviour {
	public Button button;
	public GameObject player;

	private int readyPlayers;
	private PlayersHub hub;
	private float time;
	private bool countdownOn = false;
	private bool timerOn = false;

	[SyncVar(hook = "UpdateCountdownText")]
	string countdownText = "";

	public static TimerController GetInstance () {
		return GameObject.Find ("Timer").GetComponent<TimerController> ();
	}

	public static void SetPlayer (GameObject player) {
		TimerController self = GetInstance ();
		self.player = player;
	}

	public override void OnStartServer () {
		hub = PlayersHub.GetInstance ();
	}

	public void OnStartLocal () {
		button.onClick.AddListener (OnReady);
	}

	void OnReady () {
		var model = UIController.GetUserCredentialsModel ();

		if (model.login.Length > 2 && model.password.Length > 4) {
			CmdOnPlayerReady (model, player.GetComponent<NetworkIdentity> ().netId);
			UIController.ToggleCredentialsActive ();
		}
	}

	[Command]
	void CmdOnPlayerReady (UserCredentialsModel model, NetworkInstanceId netId) {
		hub.DoLogin (model, netId);

		if (++readyPlayers == hub.Count) {
			time = 3f;
			countdownText = "3";
			countdownOn = true;
		}
	}

	void UpdateCountdownText (string newCountdownText) {
		UIController.SetWinText (newCountdownText);
	}

	void Update () {
		if (countdownOn) {
			time -= Time.deltaTime;
			if (time <= 0 && time >= -1) {
				countdownText = "Start!";
				hub.SetRaceActivity (true);
			}

			if (time < -1) {
				countdownText = "";
				time = 0;
				countdownOn = false;
				timerOn = true;
			}

			if (time > 0) {
				countdownText = Mathf.Ceil (time).ToString ();
			}
		}

		if (timerOn) {
			time += Time.deltaTime;

			hub.UpdateTimeText (time);
		}
	}
}
