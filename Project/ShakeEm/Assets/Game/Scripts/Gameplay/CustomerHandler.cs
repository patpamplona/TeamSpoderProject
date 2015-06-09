using UnityEngine;
using System.Collections;

public class CustomerHandler : MonoBehaviour 
{
	#region Singleton
	private static CustomerHandler _instance = null;
	public static CustomerHandler Instance
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
	#endregion

	private bool gameOver = false;
	public bool GameOver
	{
		get
		{
			return gameOver;
		}
	}

	[SerializeField] Customer[] customers;

	[SerializeField] private int   customersPerDifficulty  = 5;
	[SerializeField] private float difficultyMultiplierInc = 0.5f;
	[SerializeField] private float maxDifficultyMultiplier = 5.0f;

	[SerializeField] private GameObject[] heartImages;
	[SerializeField] private int hearts = 3;

	[SerializeField] private GameObject gameOverScreen;

	private int customersServed = 0;
	private float difficultyMultiplier = 1.0f;
	public float DifficultyMultiplier
	{
		get
		{
			return difficultyMultiplier;
		}
	}

	private Customer currentCustomer;
	public Customer CurrentCustomer
	{
		get
		{
			return currentCustomer;
		}
	}

	public void GenerateRandomCustomer(bool isServed = true)
	{
		if(customers == null)
		{
			throw new System.Exception("We don't have any customer");
		}

		if(!isServed)
		{
			hearts--;
			if(NetworkManager.Instance.IsServer)
			{
				RPCHandler.Instance.CallRPCUpdateHearts(hearts);
			}
		}

		if(hearts > 0);
		{
			if(NetworkManager.Instance.IsServer)
			{
				int index = Random.Range (0, customers.Length);
				RPCHandler.Instance.CallRPCGenerateCustomer(index);
			}
		}
	}

	public void UpdateHearts(int heartsUpdated)
	{
		hearts = heartsUpdated;
		for(int h = 0; h < heartImages.Length; h++)
		{
			heartImages[h].SetActive(hearts > h);
		}

		if(hearts <= 0)
		{
			hearts = 0;
			gameOver = true;
			gameOverScreen.SetActive(true);
		}
	}

    public void SetCustomer(int index)
	{
		foreach(Customer c in customers)
		{
			c.gameObject.SetActive(false);
		}
		
		currentCustomer = customers[index];
		currentCustomer.gameObject.SetActive(true);
		currentCustomer.StartTimer();
	}

	public void CustomerIsServed()
	{
		customersServed++;
		if(customersServed >= customersPerDifficulty)
		{
			customersServed = 0;
			difficultyMultiplier += difficultyMultiplierInc;
			if(difficultyMultiplier > maxDifficultyMultiplier)
			{
				difficultyMultiplier = maxDifficultyMultiplier;
			}
		}
	}

	public void Reset()
	{
		gameOver = false;
		customersServed = 0;
		difficultyMultiplier = 1.0f;
		hearts = 3;
		gameOverScreen.SetActive(false);

		if(heartImages != null)
		{
			foreach(GameObject img in heartImages)
			{
				img.SetActive(true);
			}
		}

		foreach(Customer customer in customers)
		{
			customer.MarkAsServed();
		}
	}
}
