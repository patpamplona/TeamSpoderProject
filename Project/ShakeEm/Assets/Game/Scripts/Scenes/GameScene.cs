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
		IngredientHandler.Instance.SpreadActionIngredients();

		yield return new WaitForSeconds(0.5f);

		NetworkManager.Instance.EnableConnections();

		yield return new WaitForSeconds(0.5f);

		StartOrder();
	}

	private void StartOrder()
	{
		CustomerHandler.Instance.GenerateRandomCustomer(true);
		OrderManager.Instance.GenerateOrder();
	}

	public override void DisableScene ()
	{
		NetworkManager.Instance.CloseServer();
		NetworkManager.Instance.DisconnectToServer();

		OrderManager.Instance.Reset();
		//Reset the ingredients used by the handler before clearing the pool
		IngredientHandler.Instance.Reset();
		//Clear the ingredients pool
		IngredientPool.Instance.Clear();
	}

	public void BackToMenu()
	{
		SceneManager.Instance.SwitchToScene(SceneKeys.TITLE_SCENE);
	}
}