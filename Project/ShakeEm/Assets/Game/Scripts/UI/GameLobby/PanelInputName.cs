using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PanelInputName : BasePanelLobby {

	[SerializeField]
	private Button buttonOk = null;
	private string playerName = "";
	
	public override void Start () {
		base.Start();
	}

	public override void OnShow ()
	{
		base.OnShow ();

		playerName = "";
		buttonOk.interactable = false;
	}

	public void OnTextFieldValueChange(string str) {
		
		if (str.Length > 0)
			buttonOk.interactable = true;
		else 
			buttonOk.interactable = false;
	}

	public void OnTextFieldEndEdit(string str) {

		Debug.Log ("On End Edit: " + str);
		playerName = str;

		if (playerName.Length > 0)
			buttonOk.interactable = true;
		else 
			buttonOk.interactable = false;
	}
	
	public void OnOkClicked() {

		Debug.Log ("On Ok Clicked!");
		PlayerProfile.GetInstance ().playerName = this.playerName;
		PanelController.GetInstance().ShowPanel(PanelType.PANEL_HOST_OR_JOIN);
	}
}
