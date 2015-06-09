using UnityEngine;
using System.Collections;

public class RPCHandler : MonoBehaviour 
{
	private static RPCHandler _instance = null;
	public static RPCHandler Instance
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

	public void CallAssignPlayerId(int index, NetworkPlayer target)
	{
		if(NetworkManager.Instance.IsConnected && Network.isServer)
		{
			networkView.RPC("AssignPlayerId", target, new object[] { index });
		}
	}

	[RPC]
	public void AssignPlayerId(int index)
	{
		NetworkManager.Instance.AssignNetworkId(index);
	}

	public void CallNetworkUpdateClientCount(int count)
	{
		if(NetworkManager.Instance.IsConnected && Network.isServer)
		{
			networkView.RPC("NetworkUpdateClientCount", RPCMode.Others, new object[]{ count });
		}
	}

	[RPC]
	public void NetworkUpdateClientCount(int count)
	{
		NetworkManager.Instance.UpdateClientCount(count);
	}

	public void CallRPCGenerateCustomer(int index)
	{
		if(NetworkManager.Instance.IsConnected && Network.isServer)
		{
			networkView.RPC("RPCGenerateCustomer", RPCMode.All, new object[]{ index });
		}
		else if(!NetworkManager.Instance.IsConnected)
		{
			CustomerHandler.Instance.SetCustomer(index);
		}
	}

	[RPC]
	public void RPCGenerateCustomer(int index)
	{
		CustomerHandler.Instance.SetCustomer(index);
	}

	public void CallRPCGenerateOrder(int index)
	{
		if(NetworkManager.Instance.IsConnected && Network.isServer)
		{
			networkView.RPC("RPCGenerateOrder", RPCMode.All, new object[] { index });
		}
		else if(!NetworkManager.Instance.IsConnected)
		{
			OrderManager.Instance.SetRecipe(index);
		}
	}

	[RPC]
	public void RPCGenerateOrder(int index)
	{
		OrderManager.Instance.SetRecipe(index);
	}

	public void CallRPCSpreadIngredients(string merged)
	{
		if(NetworkManager.Instance.IsConnected && Network.isServer)
		{
			networkView.RPC("RPCSpreadIngredients", RPCMode.All, new object[] { merged });
		}
	}

	[RPC]
	public void RPCSpreadIngredients(string merged)
	{
		IngredientHandler.Instance.ReceiveAssignedIngredient(merged);
	}

	public void CallRPCCheckIngredient(string id, string sender)
	{
		if(NetworkManager.Instance.IsConnected)
		{
			if(Network.isServer)
			{
				OrderManager.Instance.CheckIngredient(id, sender);
			}
			else
			{
				networkView.RPC("RPCCheckIngredient", RPCMode.Server, new object[] { id, sender });
			}
		}
		else if(!NetworkManager.Instance.IsConnected)
		{
			OrderManager.Instance.CheckIngredient(id, sender);
		}
	}

	[RPC]
	public void RPCCheckIngredient(string id, string sender)
	{
		OrderManager.Instance.CheckIngredient(id, sender);
	}

	public void CallRPCProgressRecipe(int seqIndex, bool changeRecipe, bool isCorrect, string sender)
	{
		if(NetworkManager.Instance.IsConnected && Network.isServer)
		{
			networkView.RPC("RPCProgressRecipe", RPCMode.All, new object[] { seqIndex, changeRecipe, isCorrect, sender });
		}
		else if(!NetworkManager.Instance.IsConnected)
		{
			OrderManager.Instance.ProgressRecipe(seqIndex, changeRecipe, isCorrect, sender);
		}
	}

	[RPC]
	public void RPCProgressRecipe(int seqIndex, bool changeRecipe, bool isCorrect, string sender)
	{
		OrderManager.Instance.ProgressRecipe(seqIndex, changeRecipe, isCorrect, sender);
	}

	public void CallRPCAddScore(int score)
	{
		if(NetworkManager.Instance.IsConnected && Network.isServer)
		{
			networkView.RPC("RPCAddScore", RPCMode.All, new object[] { score });
		}
		else if(!NetworkManager.Instance.IsConnected)
		{
			ScoreManager.Instance.AddScore(score);
		}
	}

	[RPC]
	public void RPCAddScore(int score)
	{
		ScoreManager.Instance.AddScore(score);
	}

	public void CallRPCCustomerIsServed()
	{
		if(NetworkManager.Instance.IsConnected && Network.isServer)
		{
			networkView.RPC("RPCCustomerIsServed", RPCMode.All, null);
		}
		else if(!NetworkManager.Instance.IsConnected)
		{
			CustomerHandler.Instance.CurrentCustomer.MarkAsServed();
		}
	}

	[RPC]
	public void RPCCustomerIsServed()
	{
		CustomerHandler.Instance.CurrentCustomer.MarkAsServed();
	}
}
