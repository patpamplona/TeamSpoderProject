using UnityEngine;
using System.Collections;

public class JoinServerScene : BasicScene
{
	private Vector2 scrollPos;
	private bool waitForPlayers = false;

	public override void EnableScene ()
	{
		NetworkManager.Instance.RefreshList();
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
			if(GUILayout.Button("Refresh Server List"))
		   	{
				NetworkManager.Instance.RefreshList();
			}

			if(NetworkManager.Instance.AvailableServers != null)
			{
				scrollPos = GUILayout.BeginScrollView(scrollPos);

				foreach(HostData host in NetworkManager.Instance.AvailableServers)
				{
					if(GUILayout.Button("Join Server : " + host.gameName))
					{
						waitForPlayers = true;
						NetworkManager.Instance.JoinServer(host);
					}
				}

				GUILayout.EndScrollView();
			}
		}
		else
		{
			GUILayout.Label("Waiting for players. . .");
		}

		if(GUILayout.Button("Back"))
		{
			if(waitForPlayers)
			{
				NetworkManager.Instance.DisconnectToServer();
			}

			SceneManager.Instance.SwitchToScene(SceneKeys.TITLE_SCENE);
		}

		GUILayout.EndArea();
	}
}