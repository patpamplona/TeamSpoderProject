﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PanelAvailableStores : BasePanelLobby {

	[SerializeField] private GameObject panelQueryingServer = null;
	[SerializeField] private GameObject panelQueryComplete = null;
	[SerializeField] private GameObject panelHostsList = null;
	[SerializeField] private GameObject HostItemPrefab = null;
	
	private HostData[] hostsList;
	private List<HostItem> hostItemList;

	public override void Start () {

		base.Start ();
		hostItemList = new List<HostItem>();
	}

	public override void OnShow () {

		base.OnShow ();
		ShowQueryingServer();
	}

	public override void OnShowComplete() 
	{
		this.StartCoroutine(this.RefreshHostsList());
	}

	public void ChangeToNextScreen() 
	{
		PanelController.GetInstance ().ShowPanel (PanelType.PANEL_WAITING_ROOM);
	}

	public void OnBackClicked() 
	{
		Debug.Log ("On Back Clicked!");
		PanelController.GetInstance ().ShowPanelBack (PanelType.PANEL_HOST_OR_JOIN);
	}

	private void PopulateAvailableHosts() 
	{
		ClearHostItemsList();

		// Generate different buttons,
		// Add listeners depending on index

		int maxCount = hostsList.Length;
		for(int i = 0; i < maxCount; i++) 
		{

			HostData hostData = hostsList[i];

			Debug.Log("Host #" + (i + 1) + ": " + hostData.gameName);
			GameObject item = Instantiate(HostItemPrefab) as GameObject;
			item.transform.SetParent(panelHostsList.transform);
			item.transform.localPosition = Vector3.zero;
			item.transform.localScale = Vector3.one;

			HostItem hostItem = item.GetComponent<HostItem>();
			hostItem.SetHostName(hostData.gameName);
			hostItem.GetButton().onClick.AddListener(() => {
				OnConnectToHostClick(hostData);
			});

			hostItemList.Add(hostItem);
		}
	}


	private void PopulateAvailableHostsDebug() 
	{
		ClearHostItemsList();

		int maxCount = 1;
		for(int i = 0; i < maxCount; i++) 
		{
			string tmpGameName = "Temp Server #" + (i + 1);

			Debug.Log("Host #" + (i + 1) + ": " + tmpGameName);
			GameObject item = Instantiate(HostItemPrefab) as GameObject;
			item.transform.SetParent(panelHostsList.transform);
			item.transform.localPosition = Vector3.zero;
			item.transform.localScale = Vector3.one;
			
			HostItem hostItem = item.GetComponent<HostItem>();
			hostItem.SetHostName(tmpGameName);
			hostItem.GetButton().onClick.AddListener(() => {
				OnConnectToHostClick(null);
			});
			
			hostItemList.Add(hostItem);
		}

		RectTransform panelRectTransform = panelHostsList.GetComponent<RectTransform>();
		panelRectTransform.sizeDelta = new Vector2(800, (100 + 20) * maxCount);
	}

	private void ClearHostItemsList() {

		while(hostItemList.Count > 0) {

			int maxCount = hostItemList.Count;
			HostItem item = hostItemList[maxCount - 1];
			hostItemList.Remove(item);

			Destroy(item.gameObject);
		}

		hostItemList.Clear();
	}

	#region Query Server for Hosts List

	private void ShowQueryingServer() {

		panelQueryingServer.SetActive (true);
		panelQueryComplete.SetActive (false);
	}

	private void ShowHostsList() {

		panelQueryComplete.SetActive (true);
		panelQueryingServer.SetActive (false);
	}

	public IEnumerator RefreshHostsList() {

		Debug.Log ("Refreshing...");
		MasterServer.RequestHostList(GameConstants.UGID);

		float timeStart = Time.time;
		float timeEnd = timeStart + GameConstants.REQUEST_REFRESH_LENGTH;

		while (Time.time < timeEnd) 
		{
			hostsList = NetworkManager.Instance.AvailableServers;
			yield return new WaitForEndOfFrame();
		}

		if (hostsList != null && hostsList.Length > 0) 
		{
			Debug.Log ("Servers Available!");
		}
		else 
		{
			// No servers found.
			Debug.Log ("No Servers Found!");
		}

		ShowHostsList();
		PopulateAvailableHosts();
		//PopulateAvailableHostsDebug();
	}

	#endregion


	#region Select and Connect to Server

	public void OnConnectToHostClick(HostData host) {

		if(host == null)
			return;

		Debug.Log("Connecting to Host: " + host.gameName);
		PlayerProfile.GetInstance ().storeName = host.gameName;

		NetworkManager.Instance.OnConnectedToServerEvt += OnConnectedToServer;
		NetworkManager.Instance.JoinServer(host);
	}
	
	public void OnConnectedToServer() 
	{
		Debug.Log ("Connected to Server!");
		NetworkManager.Instance.OnConnectedToServerEvt -= OnConnectedToServer;
		PlayerProfile.GetInstance().playerType = PlayerType.PlayerClient;
		ChangeToNextScreen();
	}

	#endregion
}
