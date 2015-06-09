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
	[SerializeField] NetworkView customerNetwork;

	[SerializeField] private int   customersPerDifficulty  = 5;
	[SerializeField] private float difficultyMultiplierInc = 0.5f;
	[SerializeField] private float maxDifficultyMultiplier = 5.0f;

	[SerializeField] private int hearts = 3;

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
		}

		if(hearts <= 0)
		{
			hearts = 0;
			gameOver = true;
		}
		else
		{
			if(NetworkManager.Instance.IsServer)
			{
				int index = Random.Range (0, customers.Length);
				RPCHandler.Instance.CallRPCGenerateCustomer(index);
			}
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

			Debug.Log ("DIFFICULTY MULTIPLIER : " + difficultyMultiplier);
		}
	}

	public void Reset()
	{
		gameOver = false;
		customersServed = 0;
		difficultyMultiplier = 1.0f;
		hearts = 3;

		foreach(Customer customer in customers)
		{
			customer.MarkAsServed();
		}
	}
}
