using UnityEngine;
using System.Collections;

public enum PlayerType {

	PlayerHost,
	PlayerClient,
	None
}


public class PlayerProfile {
	
	private static PlayerProfile instance = null;
	public static PlayerProfile GetInstance() {

		if (instance == null) {
			instance = new PlayerProfile();
		}

		return instance;
	}

	public string playerName = "defaultPlayerName";
	public string storeName = "defaultStoreName";
	public PlayerType playerType = PlayerType.None;

	public bool isConnected = false;

	private PlayerProfile() {

		// Constructor
	}
}
