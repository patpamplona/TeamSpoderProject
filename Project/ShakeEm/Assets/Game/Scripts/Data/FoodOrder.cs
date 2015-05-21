using System;

public class FoodOrder : AOrder
{
	public override void StartRecipe ()
	{
		sequenceIndex = 0;
	}

	public override bool AddRecipe (string ingredient)
	{
		if(recipe == null || recipe.Length == 0)
		{
			throw new System.Exception(ErrorMessages.ERR_MISSING_RECIPE);
		}

		if(sequenceIndex < 0 || sequenceIndex >= recipe.Length)
		{
			throw new System.Exception(ErrorMessages.ERR_SEQUENCE_OUT_OF_BOUNDS);
		}

		//Player got the ingredient in the right order
		if(recipe[sequenceIndex] == ingredient)
		{
			sequenceIndex++;
			return true;
		}

		//Player failed to add the right ingredient
		return false;
	}
}
