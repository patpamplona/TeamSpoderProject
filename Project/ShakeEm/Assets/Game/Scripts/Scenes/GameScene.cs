﻿using UnityEngine;
using System.Collections;

public class GameScene : BasicScene
{
	public override void EnableScene()
	{
		CustomerHandler.Instance.Reset();
		ScoreManager.Instance.Reset();

		if(NetworkManager.Instance.IsConnected)
		{
			StartCoroutine(EnableConnections());
		}
		else
		{
			StartOrder();
		}
	}

	private IEnumerator EnableConnections()
	{
		yield return new WaitForSeconds(1.0f);

		NetworkManager.Instance.EnableConnections();
		StartOrder();
	}

	private void StartOrder()
	{
		CustomerHandler.Instance.GenerateRandomCustomer();
		OrderManager.Instance.GenerateOrder();
	}

	public override void DisableScene ()
	{
		OrderManager.Instance.Reset();
		//Reset the ingredients used by the handler before clearing the pool
		IngredientHandler.Instance.Reset();
		//Clear the ingredients pool
		IngredientPool.Instance.Clear();
	}
}