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

	private int sequenceIndex = 0;

	private Recipe recipe = null;
	public Recipe CurrentRecipe
	{
		get
		{
			return recipe;
		}
	}

	public string CurrentIngredient
	{
		get
		{
			if(recipe == null) return "";
			if(sequenceIndex < 0 || sequenceIndex >= recipe.IngredientsNeeded) return "";
			return recipe.GetIngredient(sequenceIndex);
		}
	}

	[SerializeField] private AdjustableGrid recipeGrid;
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
	public void GenerateOrder()
	{
		if(recipeArray == null)
		{
			throw new System.Exception("We don't have any recipe");
		}

		int index = Random.Range(0, recipeArray.Length);

		recipe = recipeArray[index];

		List<string> ingredients = new List<string>();
		for(int i = 0; i < recipe.IngredientsNeeded; i++)
		{
			ingredients.Add(recipe.GetIngredient(i));
		}

		recipeGrid.SetGrid(ingredients);
		sequenceIndex = 0;

		recipeGrid.FocusOnChild(sequenceIndex);

		IngredientHandler.Instance.GenerateIngredients();
	}

	public void AddIngredient(string id)
	{
		if(CurrentRecipe.GetIngredient(sequenceIndex) == id)
		{
			sequenceIndex++;
		}
		else
		{
			sequenceIndex = 0;
		}

		if(sequenceIndex >= CurrentRecipe.IngredientsNeeded)
		{
			ScoreManager.Instance.AddScore(CustomerHandler.Instance.CurrentCustomer.Score);
			CustomerHandler.Instance.CurrentCustomer.MarkAsServed();
			CustomerHandler.Instance.GenerateRandomCustomer();
			GenerateOrder();
		}
		else
		{
			IngredientHandler.Instance.GenerateIngredients();
			recipeGrid.FocusOnChild(sequenceIndex);
		}
	}
	#endregion
}
