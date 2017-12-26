using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class AchievementController : NetworkBehaviour {
	private PlayersHub hub;
	public bool isSuper;

	public override void OnStartServer () {
		if (isServer) {
		  hub = PlayersHub.GetInstance ();
		}
	}

	void Update () {
		transform.Rotate (Vector3.up * Time.deltaTime * 100, Space.World);
	}

	void OnTriggerEnter (Collider other)
	{
		CmdApplyAchievement(other.GetComponentInParent<NetworkIdentity> ().netId);
	}

	[Command]
	void CmdApplyAchievement (NetworkInstanceId netId) {
		lock (this) {
		  if (gameObject.activeSelf) {
		    gameObject.SetActive (false);
		    RpcSetInactive ();
			hub.ApplyAchievement (netId, isSuper);
		  }
		}
	}

	[ClientRpc]
	void RpcSetInactive () {
		gameObject.SetActive (false);
	}

	public static List<Vector2> GetRandomPositions (int count, RoadController roads) {
		return Enumerable
	  		.Range(0, count)
			.Select<int, Vector2>((c) => GetPoint(roads))
	  		.ToList<Vector2>();
	}

	public static Vector2 GetPoint (RoadController roads) {
		float x = GetX ();

		return new Vector2 (
			roads.InterpolateX(x),
	  		GetY (
	    		GetYRange (
					roads.GetYsInIntersections (x)
	    		)
	  		)
		);
	}

	private static float GetX () {
		return Random.Range (0f, 1f);
	}

	private static float GetY (Range[] yRanges) {
		var randomRangeIndex = new System.Random().Next(yRanges.Length);

		return Random.Range (yRanges [randomRangeIndex].min, yRanges [randomRangeIndex].max);
	}

	private static Range[] GetYRange (float[] ys) {
		var points = ys;

		Range[] ranges = new Range[(int)(points.Length / 2)];

		for (int i = 0; i < points.Length / 2; i++) {
			ranges [i] = new Range {
				min = points[2 * i],
				max = points[2 * i + 1]
			};
		}

		return ranges;
	}

	private class Range {
		public float min;
		public float max;

		public float Random {
		  get {
		    return UnityEngine.Random.Range (min, max);
		  }
		}
	}
}
