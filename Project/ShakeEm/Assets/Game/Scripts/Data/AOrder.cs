using UnityEngine;

public abstract class AOrder : MonoBehaviour
{
	[SerializeField] protected string recipeId;
	public virtual string RecipeId
	{
		get
		{
			return recipeId;
		}
	}

	[SerializeField] protected OrderClassification orderType;
	public OrderClassification OrderType
	{
		get
		{
			return orderType;
		}
	}

	protected int sequenceIndex;
	[SerializeField] protected string[] recipe;

	public abstract void StartRecipe();
	public abstract bool AddRecipe(string ingredient);
}

public enum OrderClassification
{
	FOOD,
	DRINK,
	MAINTENANCE
}