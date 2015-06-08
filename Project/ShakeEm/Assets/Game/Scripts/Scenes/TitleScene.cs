using UnityEngine;
using System.Collections;

public class TitleScene : BasicScene
{
	public override void EnableScene ()
	{
		
	}
	
	public override void DisableScene ()
	{
		
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width * 0.125f, Screen.height * 0.125f, Screen.width * 0.75f, Screen.height * 0.75f));

		if(GUILayout.Button("Single Player"))
		{
			SceneManager.Instance.SwitchToScene(SceneKeys.GAME_SCENE);
		}

		if(GUILayout.Button("Create Server"))
		{
			SceneManager.Instance.SwitchToScene(SceneKeys.CREATE_SERVER_SCENE);
		}

		if(GUILayout.Button("Join Server"))
		{
			SceneManager.Instance.SwitchToScene(SceneKeys.JOIN_SERVER_SCENE);
		}

		GUILayout.EndArea();
	}
}