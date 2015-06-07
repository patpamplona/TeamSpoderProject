using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	private static NetworkManager _instance = null;
	public static NetworkManager Instance
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

	private const string UNIQUE_GAME_NAME = "TEAMSPODERUID";

	private const int PORT = 54321;
	public const int MIN_CONNECTIONS = 1;
	public const int MAX_CONNECTIONS = 2;

	private bool isConnected = false;
	public bool IsConnected
	{
		get
		{
			return isConnected;
		}
	}

	private const float TIMEOUT = 1.5f;
	private float timeOut = 0.0f;

	private HostData[] hosts = null;
	public HostData[] AvailableServers
	{
		get
		{
			return hosts;
		}
	}

	public bool IsConnectionMaxed
	{
		get
		{
			if(!isConnected) return false;
			return Network.connections.Length == Network.maxConnections;
		}
	}

	private void OnConnectedToServer()
	{
		if(Network.isClient)
		{
			isConnected = true;
		}
	}

	private void OnServerInitialized()
	{
		if(Network.isServer)
		{
			isConnected = true;
		}
	}

	private void OnPlayerDisconnected()
	{
		if(Network.isClient)
		{
			isConnected = false;
		}
	}

	private void OnDisconnectedFromServer()
	{
		if(Network.isClient)
		{
			isConnected = false;
		}
	}

	public void CreateServer(string gameName, int conn)
	{
		if(isConnected) return;

		Network.InitializeServer(conn, PORT, true);
		MasterServer.RegisterHost(UNIQUE_GAME_NAME, gameName);
	}

	public void JoinServer(HostData host)
	{
		if(isConnected) return;

		Network.Connect(host);
	}

	public void CloseServer()
	{
		if(Network.connections == null || Network.connections.Length == 0) return;

		isConnected = false;
		Network.Disconnect();
		MasterServer.UnregisterHost();
	}

	public void DisconnectToServer()
	{
		if(!isConnected) return;

		if (Network.connections.Length == 1) 
		{
			Network.CloseConnection(Network.connections[0], true);
		}

		isConnected = false;
	}

	public void RefreshList()
	{
		hosts = MasterServer.PollHostList();
		MasterServer.RequestHostList(UNIQUE_GAME_NAME);

		timeOut = 0.0f;
	}

	void Update()
	{
		timeOut += Time.deltaTime;
		if(timeOut >= TIMEOUT)
		{
			RefreshList();
		}
	}
}