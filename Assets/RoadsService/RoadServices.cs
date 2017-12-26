using System;
using UnityEngine;

namespace RoadsService
{
	public static class RoadServices
	{
		public static float ROAD_WIDTH = 20f;

		public static BorderModel GetBorderTransform (Vector2 start, Vector2 end) {
			var snippet = end - start; 

			var center = (start + end) / 2;
			var length = snippet.magnitude;

			return new BorderModel {
				position = new Vector3 (center.x, 0.5f, center.y),
				scale = new Vector3 (1, 1, length),
				rotation = Quaternion.LookRotation (new Vector3 (snippet.x, 0, snippet.y))
			};
		}

		public static KneeModel ClaculateTurn (Vector2 prev, Vector2 curr, Vector2 next) {
			Vector2 prevSnippet = curr - prev;
			Vector2 nextSnippet = next - curr;
			Vector2 direction = nextSnippet / nextSnippet.magnitude - prevSnippet / prevSnippet.magnitude;

			var cosFi = Vector2.Dot (nextSnippet, prevSnippet) / (nextSnippet.magnitude * prevSnippet.magnitude);
			var sinFi = (nextSnippet.x * prevSnippet.y - nextSnippet.y * prevSnippet.x) / (nextSnippet.magnitude * prevSnippet.magnitude);
			float kneeWidth = (sinFi / Mathf.Abs (sinFi)) * ROAD_WIDTH / Mathf.Sqrt ((1 + cosFi) / 2);
	
			Vector2 offset = direction * (kneeWidth / 2) / direction.magnitude;

			return new KneeModel {
				left = curr - offset,
				right = curr + offset
			};
		}

		public static KneeModel ClaculateEnd (Vector2 prev, Vector2 curr) {
			Vector2 snippet = curr - prev;
			Vector3 prod = Vector3.Cross (new Vector3(snippet.x, 0, snippet.y), Vector3.up);
			Vector2 offset = new Vector2 { x = prod.x, y = prod.z };

			return new KneeModel {
				left = curr + offset * (ROAD_WIDTH / 2) / offset.magnitude,
				right = curr - offset * (ROAD_WIDTH / 2) / offset.magnitude
			};
		}
	}

	public class BorderModel {
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;
	}

	public class KneeModel {
		public Vector2 left;
		public Vector2 right;
	}

	public class Line {
		public Vector2 start;
		public Vector2 end;
	}
}