using UnityEngine;
using System.Collections;

public class PanelStartGame : BasePanelLobby {

	// Use this for initialization
	public override void Start () {

		// Do not hide on start.
	}

	public void OnStartGameClicked() {
		Debug.Log ("Start Game Clicked!");
	}
}
