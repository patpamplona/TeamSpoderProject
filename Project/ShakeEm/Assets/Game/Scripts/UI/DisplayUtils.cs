using UnityEngine;
using System.Collections;

public class DisplayUtils
{
	public static void SetLayer(GameObject go, int layer)
	{
		go.layer = layer;
		foreach(Transform child in go.transform)
		{
			child.gameObject.layer = layer;
		}
	}
}