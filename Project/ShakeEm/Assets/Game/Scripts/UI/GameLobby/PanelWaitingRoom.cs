using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelWaitingRoom : BasePanelLobby {

	[SerializeField] private Button buttonStartGame;
	[SerializeField] private Text textRoomName;
	[SerializeField] private Text textPrompt;

	private float timer = 0.0f;
	private const float TIMEOUT = 30.0f;

	// Use this for initialization
	public override void Start () {

		base.Start ();
	}

	public override void OnShow() {

		base.OnShow ();

		timer = 0.0f;

		if (PlayerProfile.GetInstance ().playerType == PlayerType.PlayerHost) 
		{
			buttonStartGame.interactable = true;
		}
		else 
		{
			buttonStartGame.enabled = false;
		}

		textRoomName.text = "[ Room: " + PlayerProfile.GetInstance ().storeName + " ]";
	}

	public void UpdateTimer()
	{
		timer = 0.0f;
	}

	void Update()
	{
		if(NetworkManager.Instance.IsConnectionMaxed)
		{
			timer = 0.0f;
			SceneManager.Instance.SwitchToScene(SceneKeys.GAME_SCENE);
		}

		if(NetworkManager.Instance.IsServer)
		{
			timer += Time.deltaTime;
			if(timer >= TIMEOUT)
			{
				timer = 0.0f;
				OnBackClicked();
			}
		}
	}

	public void OnStartGameClicked() 
	{

	}

	public void OnBackClicked() 
	{
		Debug.Log ("On Back Clicked!");

		if (PlayerProfile.GetInstance ().playerType == PlayerType.PlayerClient) 
		{
			NetworkManager.Instance.DisconnectToServer();
			PanelController.GetInstance ().ShowPanelBack (PanelType.PANEL_AVAILABLE_STORE);
		}
		else if (PlayerProfile.GetInstance ().playerType == PlayerType.PlayerHost) 
		{
			NetworkManager.Instance.CloseServer();
			PanelController.GetInstance ().ShowPanelBack (PanelType.PANEL_NAME_STORE);
		}
	}
}
