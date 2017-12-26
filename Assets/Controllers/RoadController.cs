using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using RoadsService;

public class RoadController : NetworkBehaviour {
	public GameObject borderItemPrefab;
	public GameObject finishPrefab;
	private IRoadMapProvider roadProvider;

	private RoadMap globalMap;

	public static RoadController GetInstance () {
		return GameObject.Find ("RoadManager").GetComponent<RoadController> ();
	}

	public void OnCustomStartServer () {
		roadProvider = new HardCodeRoadMapProvider ();
		RoadMap map = roadProvider.GetMap ();

		int index = 0;
		Vector2 prev = new Vector2 { x = 0, y = 0 };
		KneeModel prevModel = new KneeModel { left = new Vector2 { x = -RoadServices.ROAD_WIDTH / 2, y = 0 }, right = new Vector2 { x = RoadServices.ROAD_WIDTH / 2, y = 0 } };
		Vector2[] arrayBaseLine = map.baseLine.ToArray ();

		map.baseLine.ForEach ((point) => {
			KneeModel kneeModel;

			if (index == arrayBaseLine.Length - 1) {
				kneeModel = RoadServices.ClaculateEnd (prev, point);
			} else {
				kneeModel = RoadServices.ClaculateTurn (prev, point, arrayBaseLine [++ index]);
			}

			BorderModel leftModel = RoadServices.GetBorderTransform (prevModel.left, kneeModel.left);
			BorderModel rightModel = RoadServices.GetBorderTransform (prevModel.right, kneeModel.right);

			map.leftLine.Add (kneeModel.left);
			map.rightLine.Add (kneeModel.right);

			var leftWall = Instantiate (borderItemPrefab, leftModel.position, leftModel.rotation);
			var rightWall = Instantiate (borderItemPrefab, rightModel.position, rightModel.rotation);
			leftWall.transform.localScale = leftModel.scale;
			rightWall.transform.localScale = rightModel.scale;

			NetworkServer.Spawn (leftWall);
			NetworkServer.Spawn (rightWall);
			prev = point;
			prevModel = kneeModel;
		});

		globalMap = map;

		var lastPoint = arrayBaseLine[arrayBaseLine.Length - 1];
		var preLastPoint = arrayBaseLine[arrayBaseLine.Length - 2];
		var lookVector = lastPoint - preLastPoint;

		NetworkServer.Spawn (
			Instantiate (
				finishPrefab,
				new Vector3 (lastPoint.x, 1, lastPoint.y),
				Quaternion.LookRotation (new Vector3(lookVector.x, 0, lookVector.y))
			)
		);
	}

	public float InterpolateX (float x) {
		return _interpolateX (x);
	}

	public float[] GetYsInIntersections (float x) {
		float randomX = _interpolateX (x);
		
		Vector2 prevLeft = new Vector2 {
			x = -RoadServices.ROAD_WIDTH / 2,
			y = 0
		};
		Vector2 prevRight = new Vector2 {
			x = RoadServices.ROAD_WIDTH / 2,
			y = 0
		};

		List<Line> lines = new List<Line> () {
			new Line {
				start = prevLeft,
				end = prevRight
			},
			new Line {
				start = globalMap.leftLine.Last (),
				end = globalMap.rightLine.Last ()
			}
		}.Concat(
			globalMap.leftLine
			.Select ((point) => {
				var line = new Line { start = prevLeft, end = point };
				prevLeft = point;
				return line;
			})
			.Concat(
				globalMap.rightLine.Select ((point) => {
					var line = new Line { start = prevRight, end = point };
					prevLeft = point;
					return line;
				})
			)
		).ToList<Line> ();

		List<float> ys = lines.FindAll (
			(line) => Mathf.Max (line.start.x, line.end.x) > randomX && randomX > Mathf.Min (line.start.x, line.end.x)
			)
			.Select ((line) =>
				(line.end.y - line.start.y) / (line.end.x - line.start.x) * (randomX - line.start.x) + line.start.y
			)
			.ToList<float> ();

		ys.Sort ();

		return ys.ToArray ();
	}

	private float _interpolateX (float x) {
		List<float> xs = globalMap.leftLine
			.Select ((point) => point.x)
			.Concat(
				globalMap.rightLine.Select ((point) => point.x)
			).ToList<float> ();

		var min = xs.Min ();
		var max = xs.Max ();

		return x * (max - min) + min;
	}
}