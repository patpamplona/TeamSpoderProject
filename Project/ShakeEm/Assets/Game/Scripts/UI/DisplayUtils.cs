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

	public static string ConvertToThousands(int val)
	{
		string str = val.ToString();

		int counter = 0;
		int index = str.Length - 1;

		while(index >= 0)
		{
			counter++;

			if(counter == 3 && index > 0)
			{
				counter = 0;
				str = str.Insert(index, ",");
			}

			index--;
		}

		return str;
	}
}