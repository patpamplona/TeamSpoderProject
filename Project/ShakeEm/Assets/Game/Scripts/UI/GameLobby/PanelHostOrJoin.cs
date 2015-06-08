using UnityEngine;
using System.Collections;

public class PanelHostOrJoin : BasePanelLobby {

	// Use this for initialization
	public override void Start () {
		base.Start();
	}

	public void OnHostClicked() {

		Debug.Log ("On Host Clicked!");
		PlayerProfile.GetInstance().playerType = PlayerType.PlayerHost;
		PanelController.GetInstance().ShowPanel(PanelType.PANEL_NAME_STORE);
	}

	public void OnJoinClicked() {

		Debug.Log ("On Join Clicked!");
		PlayerProfile.GetInstance().playerType = PlayerType.PlayerClient;
		PanelController.GetInstance().ShowPanel(PanelType.PANEL_AVAILABLE_STORE);
	}

	public void OnBackClicked() {
		
		Debug.Log ("On Back Clicked!");
		PanelController.GetInstance ().ShowPanelBack (PanelType.PANEL_INPUT_NAME);
	}
}
