using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private const string LOCAL_HOST_IP = "127.0.0.1";
	private const int PORT = 1234;
	private const int MAX_REQUEST = 3;
	private bool isConnected = false;

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
	}

	public void JoinServer()
	{
		if(isConnected) return;

		Network.Connect(LOCAL_HOST_IP, PORT);
	}

	void OnGUI()
	{
		if(isConnected)
		{
			GUILayout.Label("Connections: " + Network.connections.Length.ToString());
		}
		else
		{
			if(GUILayout.Button ("Create Server"))
			{
				CreateServer();
			}
			else if(GUILayout.Button("Join Server"))
			{
				JoinServer();
			}
		}
	}
}