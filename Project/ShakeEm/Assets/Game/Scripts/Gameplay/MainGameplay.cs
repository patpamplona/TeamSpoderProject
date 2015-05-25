using UnityEngine;
using System.Collections;

public class MainGameplay : MonoBehaviour 
{
	private static bool isSinglePlayer = true;
	public static bool IsSinglePlayer
	{
		get
		{
			return isSinglePlayer;
		}
	}

	void Start()
	{
		CreateOrder();
	}

	public void CreateOrder()
	{
		CustomerHandler.Instance.GenerateRandomCustomer();
		OrderManager.Instance.GenerateOrder();
	}
}
