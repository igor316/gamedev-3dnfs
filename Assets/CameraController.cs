using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private GameObject player;

	static CameraController GetInstance () {
		return GameObject.Find ("Main Camera").GetComponent<CameraController> ();
	}

	public static void SetPlayer (GameObject player) {
		CameraController self = GetInstance ();
		self.player = player;
	}

	void LateUpdate () {
		if (player != null) {
			Vector3 currWithVelocityPosition =
				transform.position + player.GetComponent<Rigidbody>().velocity * Time.deltaTime;
			Vector3 offset =  -15 * player.transform.forward;
			offset.y = 10;

			Vector3 origin = player.transform.position + offset;
			Vector3 delta = (origin - currWithVelocityPosition) / 30;
			Vector3 newPosition = currWithVelocityPosition + delta;

			transform.position = newPosition;
			transform.rotation = Quaternion.LookRotation(player.transform.position - newPosition);
		}
	}
}
