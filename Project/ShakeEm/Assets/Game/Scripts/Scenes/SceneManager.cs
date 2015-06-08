using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour 
{
	private static SceneManager _instance = null;
	public static SceneManager Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
	}

	void OnDestroy()
	{
		_instance = null;
	}

	[SerializeField] private SceneKeyPair[] scenes;
	[SerializeField] private SceneKeys startingScene;

	private SceneKeyPair currentScene;

	void Start()
	{
		SwitchToScene(startingScene);
	}

	public void SwitchToScene(SceneKeys key)
	{
		if(currentScene == null)
		{
			currentScene = GetScene(key);
		}
		else if(currentScene != null && currentScene.Key != key)
		{
			SceneKeyPair newScene = GetScene(key);

			if(newScene != null)
			{
				currentScene.Scene.DisableScene();
				currentScene.Scene.gameObject.SetActive(false);
				currentScene = newScene;
			}
		}

		if(currentScene != null)
		{
			currentScene.Scene.gameObject.SetActive(true);
			currentScene.Scene.EnableScene();
		}
		else
		{
			Debug.LogError("Scene for key : " + key.ToString() + " not found!");
		}
	}

	private SceneKeyPair GetScene(SceneKeys key)
	{
		foreach(SceneKeyPair pair in scenes)
		{
			if(pair.Key == key)
			{
				return pair;
			}
		}

		return null;
	}
}

[System.Serializable]
public class SceneKeyPair
{
	[SerializeField] private SceneKeys  key;
	[SerializeField] private BasicScene scene;

	public SceneKeys Key
	{
		get
		{
			return key;
		}
	}

	public BasicScene Scene
	{
		get
		{
			return scene;
		}
	}
}

public enum SceneKeys
{
	TITLE_SCENE,
	CREATE_SERVER_SCENE,
	JOIN_SERVER_SCENE,
	GAME_SCENE
}