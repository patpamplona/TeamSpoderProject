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

	private Ingredient[] ingredients;
	private Ingredient[] connectors;
	private Ingredient product;
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

		if(ingredients != null)
		{
			foreach(Ingredient ing in ingredients)
			{
				IngredientPool.Instance.ReturnBorrowed(ing);
				ing.gameObject.SetActive(false);
			}

			ingredients = null;
		}

		if(connectors != null)
		{
			foreach(Ingredient conn in connectors)
			{
				IngredientPool.Instance.ReturnBorrowed(conn);
				conn.gameObject.SetActive(false);
			}
			
			connectors = null;
		}

		if(product != null)
		{
			IngredientPool.Instance.ReturnBorrowed(product);
			product.gameObject.SetActive(false);
			product = null;
		}

		int index = Random.Range(0, recipeArray.Length);

		recipe = recipeArray[index];
		ingredients = new Ingredient[recipe.IngredientsNeeded];
		connectors  = new Ingredient[recipe.IngredientsNeeded];

		for(int s = 0; s < recipe.IngredientsNeeded; s++)
		{
			string id = recipe.GetIngredient(s);
			Ingredient ing = IngredientPool.Instance.Borrow();
			ing.transform.SetParent(transform);
			ing.transform.localScale = Vector3.one;
			ing.SetIngredient(id, false);
			ing.SetImageFocus(s == 0 ? 1.0f : 0.5f);
			ing.gameObject.SetActive(true);

			ingredients[s] = ing;

			Ingredient append = IngredientPool.Instance.Borrow();
			append.transform.SetParent(transform);
			append.transform.localScale = Vector3.one;
			append.SetImageFocus(1.0f);
			append.gameObject.SetActive(true);

			if(s < recipe.IngredientsNeeded - 1)
			{
				append.SetIngredient("plus", false);
			}
			else
			{
				append.SetIngredient("equal", false);
			}

			connectors[s] = append;
		}

		product = IngredientPool.Instance.Borrow();
		product.transform.SetParent(transform);
		product.transform.localScale = Vector3.one;
		product.SetIngredient(recipe.RecipeId, false);
		product.SetImageFocus(1.0f);
		product.gameObject.SetActive(true);

		sequenceIndex = 0;
		IngredientHandler.Instance.GenerateIngredients();
	}

	public void AddIngredient(string id)
	{
		ingredients[sequenceIndex].SetImageFocus(0.5f);

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
			CustomerHandler.Instance.GenerateRandomCustomer();
			GenerateOrder();
		}
		else
		{
			IngredientHandler.Instance.GenerateIngredients();
			ingredients[sequenceIndex].SetImageFocus(1.0f);
		}
	}
	#endregion
}
