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
	}

	public void OnJoinClicked() {

		Debug.Log ("On Join Clicked!");
		PlayerProfile.GetInstance().playerType = PlayerType.PlayerClient;
	}
}
