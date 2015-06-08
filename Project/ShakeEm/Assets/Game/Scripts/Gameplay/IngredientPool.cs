using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientPool : MonoBehaviour 
{
	private static IngredientPool _instance = null;
	public static IngredientPool Instance
	{
		get
		{
			return _instance;
		}
	}
	
	void Awake()
	{
		_instance = this;
	}
	
	void OnDestroy()
	{
		_instance = null;
	}

	[SerializeField] private Ingredient ingredientPrefab;

	private List<Ingredient> activeIngredients;
	private List<Ingredient> inactiveIngredients;

	public Ingredient Borrow()
	{
		Ingredient ing = null;

		if(inactiveIngredients == null || inactiveIngredients.Count == 0)
		{
			ing = GameObject.Instantiate(ingredientPrefab) as Ingredient;
		}
		else
		{
			ing = inactiveIngredients[0];
			inactiveIngredients.RemoveAt(0);
		}

		if(activeIngredients == null)
		{
			activeIngredients = new List<Ingredient>();
		}

		activeIngredients.Add(ing);

		return ing;
	}

	public void ReturnBorrowed(Ingredient ing)
	{
		if(inactiveIngredients == null)
		{
			inactiveIngredients = new List<Ingredient>();
		}

		activeIngredients.Remove(ing);
		inactiveIngredients.Add(ing);
	}

	public void Clear()
	{
		if(activeIngredients != null)
		{
			foreach(Ingredient ing in activeIngredients)
			{
				Destroy(ing);
			}

			activeIngredients.Clear();
		}

		if(inactiveIngredients != null)
		{
			foreach(Ingredient ing in inactiveIngredients)
			{
				Destroy(ing);
			}
			
			inactiveIngredients.Clear();
		}
	}
}
