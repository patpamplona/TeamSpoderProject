using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Customer : MonoBehaviour 
{
	[SerializeField] private int scoreGiven;
	[SerializeField] private int scoreDeduction;

	[SerializeField] private float patience;
	[SerializeField] private Image patienceTimer;

	private bool isWaiting = false;
	private float timer = 0.0f;

	public int Score
	{
		get
		{
			return Mathf.FloorToInt(scoreGiven * (timer / patience));
		}
	}

	public void StartTimer()
	{
		timer = 0.0f;
		isWaiting = true;
	}

	public void MarkAsServed()
	{
		timer = 0.0f;
		isWaiting = false;
	}

	void Update()
	{
		if(!isWaiting) return;

		timer += Time.deltaTime;
		
		UpdateTimer();

		if(timer >= patience)
		{
			patienceTimer.fillAmount = 0.0f;
			timer = 0.0f;
			isWaiting = false;
			
			if(NetworkManager.Instance.IsServer)
			{
				RPCHandler.Instance.CallRPCAddScore(scoreDeduction);
				CustomerHandler.Instance.GenerateRandomCustomer();
				OrderManager.Instance.GenerateOrder();
			}
		}
	}

	private void UpdateTimer()
	{
		patienceTimer.fillAmount = 1.0f - (timer / patience);

		/*
		if(patienceTimer.fillAmount >= 0.75f)
		{
			patienceTimer.color = Color.blue;
		}
		else if(patienceTimer.fillAmount >= 0.5f)
		{
			patienceTimer.color = Color.green;
		}
		else if(patienceTimer.fillAmount >= 0.25f)
		{
			patienceTimer.color = Color.yellow;
		}
		else
		{
			patienceTimer.color = Color.red;
		}
		*/
	}
}
