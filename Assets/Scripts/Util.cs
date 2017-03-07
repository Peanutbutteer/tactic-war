using System;
using UnityEngine;
public static class Util
{
	public static Quaternion Turning(float x, float y)
	{
			float rad = Mathf.Atan2 (x, y);
			float deg = rad * (180 / Mathf.PI);
			return Quaternion.Euler (new Vector3 (0, deg, 0));
	}

    public static Quaternion TurningFix(float x, float y)
    {
        float rad = Mathf.Atan2(x, y);
        float deg = rad * (180 / Mathf.PI);
        return Quaternion.Euler(new Vector3(90, 0, 90-deg));
        }
}