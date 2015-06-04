using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string LOCAL_HOST_IP = "127.0.0.1";
	private const int PORT = 1234;
	private const int MAX_REQUEST = 3;
	private bool isConnected = false;

	private const float TIMEOUT = 2.5f;
	private float timeOut = 0.0f;

	private HostData[] hosts = null;

	private void OnConnectedToServer()
	{
		isConnected = true;
	}

	private void OnServerInitialized()
	{
		isConnected = true;
	}

	private void OnPlayerDisconnected()
	{
		isConnected = false;
	}

	public void CreateServer()
	{
		if(isConnected) return;

		Network.InitializeServer(MAX_REQUEST, PORT, true);
		MasterServer.RegisterHost("TEAMSPODERHOST", gameName);
	}

	public void JoinServer(HostData host)
	{
		if(isConnected) return;

		Network.Connect(host);
	}

	void Update()
	{
		timeOut += Time.deltaTime;
		if(timeOut >= TIMEOUT)
		{
			timeOut = 0.0f;
			hosts = MasterServer.PollHostList();
			MasterServer.RequestHostList("TEAMSPODERHOST");
		}
	}

	private string gameName = "";

	void OnGUI()
	{
		if(isConnected)
		{
			GUILayout.Label("Connections: " + Network.connections.Length.ToString());
		}
		else
		{
			gameName = GUILayout.TextField(gameName);
			if(GUILayout.Button ("Create Server", GUILayout.Width(Screen.width * 0.25f), GUILayout.Height(Screen.height * 0.25f)))
			{
				CreateServer();
			}

			if(hosts != null)
			{
				foreach(HostData host in hosts)
				{
					if(GUILayout.Button("Join Game : " + host.gameName, GUILayout.Width(Screen.width * 0.25f), GUILayout.Height(Screen.height * 0.1f)))
					{
						JoinServer(host);
					}
				}
			}
		}
	}
}