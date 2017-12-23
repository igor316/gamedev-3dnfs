using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CarController : NetworkBehaviour {
	public List<AxleInfo> axleInfos;
	public float maxMotorTorque;
	public float maxSteeringAngle;

	[SyncVar]
	public bool raceActive;

	public override void OnStartServer () {
		raceActive = false;
		PlayersHub.AddPlayer (this);
	}

	public override void OnStartLocalPlayer ()
	{
		CameraController.SetPlayer (gameObject);
		TimerController.GetInstance ().OnStartLocal ();
	}

	public void ApplyLocalPositionToVisuals(WheelCollider wheelCollider) {
		if (wheelCollider.transform.childCount == 0) {
			return;
		}

		Transform wheellTransform = wheelCollider.transform.GetChild (0);
		Vector3 position;
		Quaternion rotation;
		wheelCollider.GetWorldPose (out position, out rotation);

		wheellTransform.transform.position = position;
		wheellTransform.transform.rotation = rotation;
	}

	public void FixedUpdate()
	{
		if (!isLocalPlayer || !raceActive) {
			return;
		}
		float motor = maxMotorTorque * Input.GetAxis("Vertical");
		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				float rate = 1;
				if (axleInfo.leftWheel.rpm * motor < 0) {
					rate *= 100;
				}

				axleInfo.leftWheel.motorTorque = rate * motor;
				axleInfo.rightWheel.motorTorque = rate * motor;
			}
			ApplyLocalPositionToVisuals (axleInfo.leftWheel);
			ApplyLocalPositionToVisuals (axleInfo.rightWheel);
		}
	}

	[ClientRpc]
	public void RpcSetFinishText (int count) {
		if (!isLocalPlayer) {
			return;
		}
		UIController.SetWinText ("You are the " + count + GetCountSuffix (count));
	}

	[ClientRpc]
	public void RpcUpdateTimeText (float time) {
		if (!isLocalPlayer || !raceActive) {
			return;
		}
		UIController.SetTimeText ("Time: " + time + "s");
	}

	string GetCountSuffix (int num) {
		if (num == 1) {
			return "-st";
		}
		if (num == 2) {
			return "-nd";
		}
		if (num == 1) {
			return "-rd";
		}

		return "-th";
	}
}

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}
