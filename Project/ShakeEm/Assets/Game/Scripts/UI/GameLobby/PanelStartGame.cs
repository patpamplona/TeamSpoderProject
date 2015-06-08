using UnityEngine;
using System.Collections;

public class PanelStartGame : BasePanelLobby {

	// Use this for initialization
	public override void Start () {

		// Do not hide on start.
		PanelController.GetInstance().SetCurrentPanel(this);
	}

	public void OnStartMultiplayerClicked() {

		Debug.Log ("Start Game Clicked!");
		PanelController.GetInstance().ShowPanel(PanelType.PANEL_INPUT_NAME);
	}

	public void OnStartSinglePlayerClicked() {

		Debug.Log ("Start Single Player Clicked!");
	}
}
