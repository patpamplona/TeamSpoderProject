using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientHandler : MonoBehaviour 
{
	#region Singleton
	private static IngredientHandler _instance = null;
	public static IngredientHandler Instance
	{
		get
		{
			return _instance;
		}
	}
	
	void Awake()
	{
		_instance = this;
		OnSingletonAwake();
	}
	
	void OnDestroy()
	{
		_instance = null;
		OnSingletonDestroy();
	}
	#endregion

	#region Fields and Properties
	[SerializeField] private string[] ingredientIds;
	[SerializeField] private int maxIngredientsInTable;

	private Recipe currentRecipe;
	#endregion

	#region IngredientHandler methods
	private void OnSingletonAwake()
	{

	}

	private void OnSingletonDestroy()
	{

	}

	public void SetCurrentRecipt(Recipe recipe)
	{
		this.currentRecipe = recipe;
	}

	public void GenerateIngredientsTable()
	{
		for(int i = 0; i < maxIngredientsInTable; i++)
		{
			string id = GetRandomIngredientSinglePlayer();
			Ingredient ing = IngredientPool.Instance.Borrow();

			ing.transform.SetParent(transform);
			ing.transform.localScale = Vector3.one;
			ing.SetIngredient(id);
		}
	}

	public void SpreadIngredientsToPlayers()
	{

	}

	public string GetRandomIngredientSinglePlayer()
	{
		if(this.currentRecipe == null)
		{
			throw new System.Exception("We don't have a recipe yet");
		}

		//Make sure that the inredient is still part of the recipe
		string id = "";
		bool foundAnIngredient = false;
		HashSet<int> invalid = new HashSet<int>();
		while(!foundAnIngredient)
		{
			int index = -1;
			do
			{
				index = Random.Range(0, ingredientIds.Length);
			}
			while(invalid.Contains(index) && invalid.Count < ingredientIds.Length);

			if(invalid.Count == ingredientIds.Length)
			{
				throw new System.Exception("We don't have the ingredients of the given recipe");
			}

			id = ingredientIds[index];
			foundAnIngredient = this.currentRecipe.IsPartOfRecipe(id);
		}

		return id;
	}

	public string GetRandomIngredientMultiPlayer()
	{
		//Send only one valid ingredient per player
		return "";
	}
	#endregion
}
