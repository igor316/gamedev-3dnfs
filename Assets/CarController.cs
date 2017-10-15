using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class CarController : NetworkBehaviour {
	public List<AxleInfo> axleInfos;
	public float maxMotorTorque;
	public float maxSteeringAngle;

	public override void OnStartLocalPlayer ()
	{
		GameObject camera = GameObject.Find ("Main Camera");
		CameraController cc = camera.GetComponent<CameraController> ();
		cc.SetPlayer (gameObject);
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
				Console.WriteLine (rate);
				axleInfo.leftWheel.motorTorque = rate * motor;
				axleInfo.rightWheel.motorTorque = rate * motor;
			}
			ApplyLocalPositionToVisuals (axleInfo.leftWheel);
			ApplyLocalPositionToVisuals (axleInfo.rightWheel);
		}
	}
}

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;
	public bool steering;
}