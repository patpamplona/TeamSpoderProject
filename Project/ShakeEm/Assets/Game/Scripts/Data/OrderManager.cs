using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrderManager : MonoBehaviour 
{
	#region Singleton and Instance management
	private static OrderManager _instance = null;
	public static OrderManager Instance
	{
		get
		{
			return _instance;
		}
	}

	void Awake()
	{
		_instance = this;
		AwakeOrderManager();
	}

	void OnDestroy()
	{
		_instance = null;
		DestroyOrderManager();
	}
	#endregion
	#region Instance Fields and Properties
	//Only use this for inspector setup
	[SerializeField] private Recipe[] recipeArray;
	#endregion

	#region Setup
	private void AwakeOrderManager()
	{

	}

	private void DestroyOrderManager()
	{
		recipeArray = null;
	}
	#endregion

	#region Order Manager methods
	public Recipe GetOrder()
	{
		if(recipeArray == null)
		{
			throw new System.Exception("We don't have any recipe");
		}

		int index = Random.Range(0, recipeArray.Length);

		Recipe recipe = recipeArray[index];

		for(int s = 0; s < recipe.IngredientsNeeded; s++)
		{
			string id = recipe.GetIngredient(s);
			Ingredient ing = IngredientPool.Instance.Borrow();
			ing.transform.SetParent(transform);
			ing.transform.localScale = Vector3.one;
			ing.SetIngredient(id, false);

			Ingredient append = IngredientPool.Instance.Borrow();
			append.transform.SetParent(transform);
			append.transform.localScale = Vector3.one;

			if(s < recipe.IngredientsNeeded - 1)
			{
				append.SetIngredient("plus", false);
			}
			else
			{
				append.SetIngredient("equal", false);
			}
		}

		Ingredient product = IngredientPool.Instance.Borrow();
		product.transform.SetParent(transform);
		product.transform.localScale = Vector3.one;
		product.SetIngredient(recipe.RecipeId, false);

		return recipe;
	}
	#endregion
}
