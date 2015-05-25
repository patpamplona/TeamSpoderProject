using UnityEngine;
using System.Collections;

[System.Serializable]
public class Recipe
{
	[SerializeField] private string recipeId;
	public string RecipeId
	{
		get
		{
			return recipeId;
		}
	}

	[SerializeField] private string[] ingredientSequence;
	public int IngredientsNeeded
	{
		get
		{
			return ingredientSequence.Length;
		}
	}

	public string GetIngredient(int index)
	{
		if(index < 0 || index >= IngredientsNeeded)
		{
			return "";
		}

		return ingredientSequence[index];
	}

	public bool IsPartOfRecipe(string ingredientId)
	{
		foreach(string ingredient in ingredientSequence)
		{
			if(ingredient == ingredientId)
			{
				return true;
			}
		}

		return false;
	}
}
