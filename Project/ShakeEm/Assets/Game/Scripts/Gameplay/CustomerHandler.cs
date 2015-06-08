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

	[SerializeField] Customer[] customers;
	[SerializeField] NetworkView customerNetwork;

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

		if(NetworkManager.Instance.IsServer)
		{
			int index = Random.Range (0, customers.Length);
			RPCHandler.Instance.CallRPCGenerateCustomer(index);
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

	public void Reset()
	{
		foreach(Customer customer in customers)
		{
			customer.MarkAsServed();
		}
	}
}
