using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelAvailableStores : BasePanelLobby {

	[SerializeField] private GameObject panelQueryingServer = null;
	[SerializeField] private GameObject panelHostList = null;

	private HostData[] hostsList;

	public override void Start () {
		//base.Start ();
	}

	public override void OnShow ()
	{
		base.OnShow ();
	}

	public void ChangeToNextScreen() {

		//Change to next screen;
	}

	private void PopulateAvailableHosts() {

		// Generate different buttons,
		// Add listeners depending on index

	}

	#region Query Server for Hosts List

	private void ShowQueryingServer() {

		panelQueryingServer.SetActive (true);
		panelHostList.SetActive (false);
	}

	private void ShowHostsList() {

		panelHostList.SetActive (true);
		panelQueryingServer.SetActive (false);
	}

	public IEnumerator RefreshHostsList() {

		Debug.Log ("Refreshing...");
		MasterServer.RequestHostList(GameConstants.UGID);

		float timeStart = Time.time;
		float timeEnd = timeStart + GameConstants.REQUEST_REFRESH_LENGTH;

		while (timeEnd < Time.time) {

			hostsList = MasterServer.PollHostList();
			yield return new WaitForEndOfFrame();
		}

		if (hostsList != null && hostsList.Length > 0) {
			ShowHostsList();
			PopulateAvailableHosts();
		} else {

			// No servers found.
		}
	}

	#endregion


	#region Select and Connect to Server

	public void OnConnectToHostClick(int hostIdx) {

		HostData host = hostsList [hostIdx];

		if (host == null)
			return;

		Network.Connect (host);
	}
	
	void OnConnectedToServer() {

		Debug.Log ("Connected to Server!");
		ChangeToNextScreen();
	}

	#endregion
}
