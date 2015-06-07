using UnityEngine;
using System.Collections;

public class CreateServerScene : BasicScene
{
	private string gameName  = "";
	private int    playerCnt = NetworkManager.MIN_CONNECTIONS;
	private bool   waitForPlayers = false;

	public override void EnableScene ()
	{
		gameName = "";
	}
	
	public override void DisableScene ()
	{
		waitForPlayers = false;
	}

	void Update()
	{
		if(waitForPlayers)
		{
			if(NetworkManager.Instance.IsConnectionMaxed)
			{
				NetworkManager.Instance.DisableConnections();
				SceneManager.Instance.SwitchToScene(SceneKeys.GAME_SCENE);
			}
		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(new Rect(Screen.width * 0.125f, Screen.height * 0.125f, Screen.width * 0.75f, Screen.height * 0.75f));

		if(!waitForPlayers)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Game Name : ");
			gameName = GUILayout.TextField(gameName);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Max Players : ");
			int.TryParse(GUILayout.TextField(playerCnt.ToString()), out playerCnt);
			playerCnt = Mathf.Clamp(playerCnt, NetworkManager.MIN_CONNECTIONS, NetworkManager.MAX_CONNECTIONS);
			GUILayout.EndHorizontal();

			if(GUILayout.Button("Create"))
			{
				NetworkManager.Instance.CreateServer(gameName, playerCnt);
				waitForPlayers = true;
			}
		}
		else
		{
			GUILayout.Label("Game Name : " + gameName);
			GUILayout.Label("Max Players : " + playerCnt);
			GUILayout.Label ("Waiting for players. . .");
		}

		if(GUILayout.Button("Back"))
		{
			if(waitForPlayers)
			{
				NetworkManager.Instance.CloseServer();
				waitForPlayers = false;
			}

			SceneManager.Instance.SwitchToScene(SceneKeys.TITLE_SCENE);
		}
		
		GUILayout.EndArea();
	}
}