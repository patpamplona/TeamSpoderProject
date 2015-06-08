using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelNameStore : BasePanelLobby {

	[SerializeField] 
	private Button buttonStart = null;

	[SerializeField] 
	private GameObject panelConnecting = null;

	private string storeName = "";

	public override void Start () {
		base.Start();
	}

	public override void OnShow ()
	{
		base.OnShow ();
		
		storeName = "";
		buttonStart.interactable = false;
		panelConnecting.SetActive (false);
	}

	private void ChangeToNextScreen() {

		// change to next screen
	}

	private void ShowConnectingPanel() {

		buttonStart.interactable = false;
		panelConnecting.SetActive (true);
	}

	private void HideConnectingPanel() {

		buttonStart.interactable = true;
		panelConnecting.SetActive (false);
	}

	#region UI Event Callbacks

	public void OnTextFieldValueChange(string str) {

		if (str.Length > 0)
			buttonStart.interactable = true;
		else 
			buttonStart.interactable = false;
	}

	public void OnTextFieldEndEdit(string str) {
		
		Debug.Log ("On End Edit: " + str);
		storeName = str;
		
		if (storeName.Length > 0)
			buttonStart.interactable = true;
		else 
			buttonStart.interactable = false;
	}
	
	public void OnStartClicked() {
		
		Debug.Log ("On Start Clicked!");
		PlayerProfile.GetInstance().storeName = this.storeName;

		ShowConnectingPanel();
		CreateServer();
	}

	#endregion

	#region Network Connections

	private void CreateServer()
	{
		string gameName = PlayerProfile.GetInstance().storeName;

		Network.InitializeServer(GameConstants.MAX_REQUEST, GameConstants.PORT, true);
		MasterServer.RegisterHost(GameConstants.UGID, gameName);
	}

	void OnServerInitialized()
	{
		Debug.Log("On Server Initialized");
		PlayerProfile.GetInstance().playerType = PlayerType.PlayerHost;
	}

	void OnMasterServerEvent(MasterServerEvent masterServerEvent) {

		if (masterServerEvent == MasterServerEvent.RegistrationSucceeded) {
		
			// change to next screen
			Debug.Log("Master Server Registration Successful!");
			HideConnectingPanel();
			ChangeToNextScreen();

		} else if (masterServerEvent == MasterServerEvent.RegistrationFailedGameName
			|| masterServerEvent == MasterServerEvent.RegistrationFailedGameType
			|| masterServerEvent == MasterServerEvent.RegistrationFailedNoServer) {

			// failed, make connect button available
			Debug.Log("Master Server Registration Failed!");
		}
	}

	#endregion
}
