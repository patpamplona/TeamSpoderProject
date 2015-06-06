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
	[SerializeField] private int singlePlayerMaxIngredients;
	[SerializeField] private int multiPlayerMaxIngredients;

	//Special part of ingredients
	[SerializeField] private Ingredient blender;
	[SerializeField] private Ingredient cup;
	[SerializeField] private Ingredient straw;

	[SerializeField] GameObject woodenTable;
	private Ingredient[] ingredientsInBoard;
	#endregion

	#region IngredientHandler methods
	private void OnSingletonAwake()
	{

	}

	private void OnSingletonDestroy()
	{
		ingredientIds = null;
	}

	public void GenerateIngredients()
	{
		if(MainGameplay.IsSinglePlayer)
		{
			GenerateIngredientsTable();
		}
		else
		{
			SpreadIngredientsToPlayers();
		}
	}

	public void GenerateIngredientsTable()
	{
		if(ingredientsInBoard != null)
		{
			foreach(Ingredient iib in ingredientsInBoard)
			{
				IngredientPool.Instance.ReturnBorrowed(iib);
				iib.gameObject.SetActive(false);
			}
		}

		string[] ingInBoard = new string[singlePlayerMaxIngredients];
		ingredientsInBoard  = new Ingredient[singlePlayerMaxIngredients];

		string validId = OrderManager.Instance.CurrentIngredient;

		int start = 1;

		if(validId == cup.IngredientID || validId == blender.IngredientID || validId == straw.IngredientID)
		{
			start = 0;
		}
		else
		{
			ingInBoard[0] = validId;
		}

		for(int i = start; i < singlePlayerMaxIngredients; i++)
		{
			string id = GetRandomIngredientSinglePlayer();
			ingInBoard[i] = id;
		}

		ingInBoard = ShuffleIngredients(ingInBoard);

		for(int s = 0; s < ingInBoard.Length; s++)
		{
			string ingId = ingInBoard[s];
			
			Ingredient ing = IngredientPool.Instance.Borrow();
			
			ing.transform.SetParent(woodenTable.transform);
			ing.transform.localScale = Vector3.one;
			DisplayUtils.SetLayer(ing.gameObject, woodenTable.layer);
			ing.SetIngredient(ingId);
			ing.gameObject.SetActive(true);

			ingredientsInBoard[s] = ing;
		}
	}

	private string[] ShuffleIngredients(string[] inBoard)
	{
		for (int i = inBoard.Length; i > 0; i--)
		{
			int j = Random.Range(0, i);
			string k = inBoard[j];
			inBoard[j] = inBoard[i - 1];
			inBoard[i - 1]  = k;
		}

		return inBoard;
	}

	public void SpreadIngredientsToPlayers()
	{

	}

	public string GetRandomIngredientSinglePlayer()
	{
		/*
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
			foundAnIngredient = OrderManager.Instance.CurrentRecipe.IsPartOfRecipe(id);
		}
		*/

		int index = Random.Range(0, ingredientIds.Length);

		return ingredientIds[index];
	}

	public string GetRandomIngredientMultiPlayer()
	{
		//Send only one valid ingredient per player
		return "";
	}
	#endregion
}
