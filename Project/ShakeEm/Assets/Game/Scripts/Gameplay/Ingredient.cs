using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ingredient : MonoBehaviour 
{
	[SerializeField] private string ingredientId;
	[SerializeField] private Image ingredientIcon;
	[SerializeField] private Button button;

	public string IngredientID
	{
		get
		{
			return ingredientId;
		}
	}

	public void SetImageFocus(float alpha)
	{
		Color c = ingredientIcon.color;
		c.a = alpha;
		ingredientIcon.color = c;
	}

	//Allow ingredient to change its id which will reflect in its display icon
	public void SetIngredient(string id, bool asButton = true)
	{
		this.ingredientId = id;
		this.ingredientIcon.sprite = SpritePool.Instance.GetSprite(this.ingredientId);
		button.enabled = asButton;
	}

	public void OnIngredientTapped()
	{
		if(CustomerHandler.Instance.GameOver) return;
		RPCHandler.Instance.CallRPCCheckIngredient(ingredientId, PlayerProfile.GetInstance().playerName);
	}
}