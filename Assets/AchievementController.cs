using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class AchievementController : NetworkBehaviour {
  void Update () {
    transform.Rotate (Vector3.up * Time.deltaTime * 100, Space.World);
  }

  public static List<Vector2> GetRandomPositions (int count) {
    return Enumerable
      .Range(0, count)
      .Select<int, Vector2>((c) => GetPoint())
      .ToList<Vector2>();
  }

  public static Vector2 GetPoint () {
    float x = InterpolateX (GetX ());

    return new Vector2 (
      x,
      GetY (
        GetYRange (
          x
        )
      )
    );
  }

  private static float GetX () {
    return Random.value;
  }

  private static float GetY (Range yRange) {
    return yRange.Random;
  }

  private static float InterpolateX (float oldX) {
    if (oldX <= 0.35) {
      return oldX * 19f - 9.5f;
    }

    if (oldX <= 0.65) {
      return oldX * 80 + 10;
    }

    return oldX * 19f + 90.5f;
  }

  private static Range GetYRange (float x) {
    if (x <= 10) {
      return new Range {
        min = 30,
        max = 110
      };
    }

    if (x <= 50) {
      return new Range {
        min = x + 82,
        max = x + 118
      };
    }

    if (x <= 90) {
      return new Range {
        min = -x + 182,
        max = -x + 218
      };
    }

    return new Range {
      min = 20,
      max = 110
    };
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
