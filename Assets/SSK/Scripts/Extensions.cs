using UnityEngine;
using System.Collections;

public static class Extensions
{

	public static Vector2 RandomVector (this Vector2 orig, float min, float max)
	{
		return new Vector2 (orig.x + Random.Range (min, max), orig.y + Random.Range (min, max));
	}

	public static Vector3 RandomVector (this Vector3 orig, float min, float max)
	{
		return new Vector3 (orig.x + Random.Range (min, max), orig.y + Random.Range (min, max), 0f);
	}

	
}
