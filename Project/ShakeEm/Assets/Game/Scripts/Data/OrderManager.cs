using UnityEngine;
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
	[SerializeField] private AOrder[] arrOrders;

	private Dictionary<OrderClassification, List<AOrder>> orderDictionary;
	#endregion

	#region Setup
	private void AwakeOrderManager()
	{
		orderDictionary = new Dictionary<OrderClassification, List<AOrder>>();

		for(int o = 0; o < arrOrders.Length; o++)
		{
			AOrder order = arrOrders[o];
			if(!orderDictionary.ContainsKey(order.OrderType))
			{
				orderDictionary.Add(order.OrderType, new List<AOrder>());
			}

			orderDictionary[order.OrderType].Add(order);
		}

		arrOrders = null;
	}

	private void DestroyOrderManager()
	{
		foreach(KeyValuePair<OrderClassification, List<AOrder>> pair in orderDictionary)
		{
			pair.Value.Clear();
		}

		orderDictionary.Clear();
		orderDictionary = null;
	}
	#endregion

	#region Order Manager methods
	public AOrder GetOrder(OrderClassification orderType)
	{
		if(!orderDictionary.ContainsKey(orderType))
		{
			throw new System.Exception(ErrorMessages.ERR_INVALID_ORDER_TYPE);
		}

		List<AOrder> newOrder = orderDictionary[orderType];

		if(newOrder.Count == 0)
		{
			throw new System.Exception(ErrorMessages.ERR_ORDER_TYPE_EMPTY);
		}

		int index = Random.Range(0, newOrder.Count);
		return newOrder[index];
	}
	#endregion
}
