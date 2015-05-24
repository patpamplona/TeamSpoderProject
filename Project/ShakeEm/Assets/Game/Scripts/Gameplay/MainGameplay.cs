using UnityEngine;
using System.Collections;

public class MainGameplay : MonoBehaviour 
{
	[SerializeField] bool isSinglePlayer = true;
	void Start()
	{
		CreateOrder();
	}

	public void CreateOrder()
	{
		CustomerHandler.Instance.GenerateRandomCustomer();
		IngredientHandler.Instance.SetCurrentRecipt(OrderManager.Instance.GetOrder());

		if(isSinglePlayer)
		{
			IngredientHandler.Instance.GenerateIngredientsTable();
		}
		else
		{
			IngredientHandler.Instance.SpreadIngredientsToPlayers();
		}
	}
}
