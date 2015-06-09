﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelWaitingRoom : BasePanelLobby {

	[SerializeField] private Button buttonStartGame;
	[SerializeField] private Text textRoomName;
	[SerializeField] private Text textPrompt;

	// Use this for initialization
	public override void Start () {

		base.Start ();
	}

	public override void OnShow() {

		base.OnShow ();

		if (PlayerProfile.GetInstance ().playerType == PlayerType.PlayerHost) {
			buttonStartGame.interactable = true;
		} else {
			buttonStartGame.enabled = false;
		}

		textRoomName.text = "[ Room: " + PlayerProfile.GetInstance ().storeName + " ]";
	}

	public void OnStartGameClicked() {

		Debug.Log("Start Game Clicked!");
	}

	public void OnBackClicked() {
		
		Debug.Log ("On Back Clicked!");

		Network.Disconnect();

		if (PlayerProfile.GetInstance ().playerType == PlayerType.PlayerClient) {
			PanelController.GetInstance ().ShowPanelBack (PanelType.PANEL_AVAILABLE_STORE);
		} else if (PlayerProfile.GetInstance ().playerType == PlayerType.PlayerHost) {
			PanelController.GetInstance ().ShowPanelBack (PanelType.PANEL_NAME_STORE);
		}
	}
}