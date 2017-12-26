using System;
using System.Collections.Generic;
using UnityEngine;

namespace RoadsService
{
	public interface IRoadMapProvider
	{
		RoadMap GetMap ();
	}

	public class RoadMap {
		public List<Vector2> baseLine;
		public List<Vector2> leftLine;
		public List<Vector2> rightLine;
	}
}