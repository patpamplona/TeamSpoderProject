using UnityEngine;
using System.Collections;

public class GameScene : BasicScene
{
	public override void EnableScene()
	{
		CustomerHandler.Instance.Reset();
		ScoreManager.Instance.Reset();

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