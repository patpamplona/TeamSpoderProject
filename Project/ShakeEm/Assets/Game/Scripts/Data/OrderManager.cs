using UnityEngine;
using UnityEngine.UI;
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
	[SerializeField] private Image    product;

	[SerializeField] private GameObject correctFeedback;
	[SerializeField] private GameObject wrongFeedback;

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
		if(CustomerHandler.Instance.GameOver) return;

		if(recipeArray == null)
		{
			throw new System.Exception("We don't have any recipe");
		}

		if(NetworkManager.Instance.IsServer)
		{
			int index = Random.Range(0, recipeArray.Length);
			RPCHandler.Instance.CallRPCGenerateOrder(index);
		}
	}

	public void SetRecipe(int index)
	{
		recipe = recipeArray[index];
		product.gameObject.SetActive(true);
		product.sprite = SpritePool.Instance.GetSprite(recipe.RecipeId);
		UpdateOrder();
	}

	private void UpdateOrder()
	{
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

	public void CheckIngredient(string id)
	{
		if(CurrentRecipe.GetIngredient(sequenceIndex) == id)
		{
			sequenceIndex++;

			if(!correctFeedback.activeInHierarchy)
			{
				correctFeedback.SetActive(true);
				StartCoroutine(ParticleDisabler(correctFeedback));
			}
		}
		else
		{
			sequenceIndex = 0;

			if(!wrongFeedback.activeInHierarchy)
			{
				wrongFeedback.SetActive(true);
				StartCoroutine(ParticleDisabler(wrongFeedback));
			}
		}

		bool changeRecipe = sequenceIndex >= CurrentRecipe.IngredientsNeeded;

		RPCHandler.Instance.CallRPCProgressRecipe(sequenceIndex, changeRecipe);
	}

	public void ProgressRecipe(int sequence, bool changeRecipe)
	{
		if(changeRecipe)
		{
			CustomerHandler.Instance.CustomerIsServed();

			sequenceIndex = 0;
			RPCHandler.Instance.CallRPCAddScore(CustomerHandler.Instance.CurrentCustomer.Score);
			RPCHandler.Instance.CallRPCCustomerIsServed();

			CustomerHandler.Instance.GenerateRandomCustomer(true);
			GenerateOrder();
		}
		else
		{
			sequenceIndex = sequence;
			IngredientHandler.Instance.GenerateIngredients();
			recipeGrid.FocusOnChild(sequenceIndex);
		}
	}

	private IEnumerator ParticleDisabler(GameObject go)
	{
		yield return new WaitForSeconds(0.65f);

		go.SetActive(false);
	}

	public void Reset()
	{
		recipe = null;
		recipeGrid.Clear();
	}
	#endregion
}
