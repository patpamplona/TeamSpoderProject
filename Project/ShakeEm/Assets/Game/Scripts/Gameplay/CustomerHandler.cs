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

	//TODO: Make a customer class with patience and range of recipe ids
	[SerializeField] Customer[] customers;

	private Customer currentCustomer;
	public Customer CurrentCustomer
	{
		get
		{
			return currentCustomer;
		}
	}

	public void GenerateRandomCustomer()
	{
		if(customers == null)
		{
			throw new System.Exception("We don't have any customer");
		}

		foreach(Customer c in customers)
		{
			c.gameObject.SetActive(false);
		}

		int index = Random.Range (0, customers.Length);

		currentCustomer = customers[index];

		currentCustomer.gameObject.SetActive(true);
		currentCustomer.StartTimer();
	}

	public void Reset()
	{
		foreach(Customer customer in customers)
		{
			customer.MarkAsServed();
		}
	}
}
