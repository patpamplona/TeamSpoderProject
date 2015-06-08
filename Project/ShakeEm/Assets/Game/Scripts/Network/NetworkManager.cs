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

	private HostData currentHost;

	private int clientCount = 0;
	public bool IsConnectionMaxed
	{
		get
		{
			if(!isConnected) return false;
			if(Network.isServer)
			{
				return Network.connections.Length == Network.maxConnections;
			}

			if(currentHost == null) return false;
			return clientCount == currentHost.playerLimit - 1;
		}
	}

	public bool IsServer
	{
		get
		{
			if(!isConnected) return true;
			return Network.isServer;
		}
	}

	public void UpdateClientCount(int count)
	{
		if(Network.isClient)
		{
			clientCount = count;
		}
	}

	private void OnConnectedToServer()
	{
		if(Network.isClient)
		{
			isConnected = true;
		}
		else
		{
			clientCount = Network.connections.Length;
			RPCHandler.Instance.CallNetworkUpdateClientCount(clientCount);
		}
	}

	private void OnPlayerConnected(NetworkPlayer player) 
	{
		if(Network.isServer)
		{
			clientCount = Network.connections.Length;
			RPCHandler.Instance.CallNetworkUpdateClientCount(clientCount);
		}
	}

	public void EnableConnections()
	{
		SetConnectionUpdates(true);
	}

	public void DisableConnections()
	{
		SetConnectionUpdates(false);
	}

	private void SetConnectionUpdates(bool enable)
	{
		Network.isMessageQueueRunning = enable;
		Network.SetSendingEnabled(0, enable);
	}

	private void OnServerInitialized()
	{
		if(Network.isServer)
		{
			isConnected = true;
			clientCount = Network.connections.Length;
			RPCHandler.Instance.CallNetworkUpdateClientCount(clientCount);
		}
	}

	private void OnPlayerDisconnected()
	{
		if(Network.isClient)
		{
			isConnected = false;
		}
		else
		{
			clientCount = Network.connections.Length;
			RPCHandler.Instance.CallNetworkUpdateClientCount(clientCount);
		}
	}

	private void OnDisconnectedFromServer()
	{
		if(Network.isClient)
		{
			isConnected = false;
		}

		clientCount = 0;
	}

	public void CreateServer(string gameName, int conn)
	{
		if(isConnected) return;

		clientCount = 0;
		Network.InitializeServer(conn, PORT, true);
		MasterServer.RegisterHost(UNIQUE_GAME_NAME, gameName);
	}

	public void JoinServer(HostData host)
	{
		if(isConnected) return;

		currentHost = host;
		Network.Connect(host);
	}

	public void CloseServer()
	{
		if(Network.connections == null || Network.connections.Length == 0) return;

		clientCount = 0;
		RPCHandler.Instance.CallNetworkUpdateClientCount(clientCount);

		isConnected = false;
		currentHost = null;

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

		clientCount = 0;

		currentHost = null;
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