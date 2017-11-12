using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	public GameObject player;
	private Vector3 offset;

	public void SetPlayer (GameObject p) {
		player = p;
		offset = new Vector3 (0, 10, -15);
	}

	void LateUpdate () {
		if (player != null) {
			transform.position = player.transform.position + offset;
		}
	}
}
