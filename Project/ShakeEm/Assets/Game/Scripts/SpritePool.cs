using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SpritePool : MonoBehaviour 
{
	#region Singleton
	private static SpritePool _instance = null;
	public static SpritePool Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
		OnPoolAwake();
	}

	void OnDestroy()
	{
		_instance = null;
		OnPoolDestroy();
	}
	#endregion

	#region Fields and Properties
	//Use this for initializing the dictionary
	[SerializeField] private Sprite[] sprites;
	//Use this for unset sprites
	[SerializeField] private Sprite emptySprite;

	private Dictionary<string, Sprite> spriteDict;
	#endregion

	#region SpritePool methods
	private void OnPoolAwake()
	{
		spriteDict = new Dictionary<string, Sprite>();
		for(int s = 0; s < sprites.Length; s++)
		{
			Sprite spr = sprites[s];
			spriteDict.Add(spr.name, spr);
		}

		sprites = null;
	}

	private void OnPoolDestroy()
	{
		spriteDict.Clear();
		spriteDict = null;

		emptySprite = null;
	}

	public Sprite GetSprite(string id)
	{
		if(!spriteDict.ContainsKey(id)) return emptySprite;
		return spriteDict[id];
	}
	#endregion
}
