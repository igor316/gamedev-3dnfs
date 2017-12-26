using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoadsService
{
	public class HardCodeRoadMapProvider : IRoadMapProvider
	{
		public RoadMap GetMap () {
			return new RoadMap {
				baseLine = new List<Vector2> () {
					new Vector2 { x = 0, y = 50 },
					new Vector2 { x = 35, y = 85 },
					new Vector2 { x = 70, y = 85 },
					new Vector2 { x = 70, y = 0 },
					new Vector2 { x = 35, y = -35 },
					new Vector2 { x = 0, y = -20 }
				},
				/*baseLine = new List<Vector2> () {
					new Vector2 { x = 0, y = 100 },
					new Vector2 { x = 50, y = 120 },
					new Vector2 { x = 100, y = 100 },
					new Vector2 { x = 100, y = 0 }
				},*/
				leftLine = new List<Vector2> (),
				rightLine = new List<Vector2> ()
			};
		}
	}
}

