using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	private GameObject player;
	private Vector3 offset;

	static CameraController GetInstance () {
		return GameObject.Find ("Main Camera").GetComponent<CameraController> ();
	}

	public static void SetPlayer (GameObject player) {
		CameraController self = GetInstance ();
		self.player = player;
		self.offset = new Vector3 (0, 10, -15);
	}

	void LateUpdate () {
		if (player != null) {
			transform.position = player.transform.position + offset;
		}
	}
}
