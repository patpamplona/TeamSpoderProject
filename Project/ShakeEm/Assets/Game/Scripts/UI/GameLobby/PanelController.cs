using UnityEngine;
using System.Collections;
using Holoville.HOTween;


public enum PanelType {
	PANEL_START_GAME = 0,
	PANEL_INPUT_NAME,
	PANEL_HOST_OR_JOIN,
	PANEL_NAME_STORE,
	PANEL_AVAILABLE_STORE,
	PANEL_WAITING_ROOM
}

public class PanelController : MonoBehaviour {


	private static PanelController instance = null;
	public static PanelController GetInstance() {

		return instance;
	}

	void Awake() {

		if(instance != null) {
			Destroy(this.gameObject);
			return;
		}

		instance = this;
		HOTween.Init(true, true, true);
	}


	[SerializeField] private BasePanelLobby[] panelsList = null;

	private BasePanelLobby currPanel;
	private BasePanelLobby oldPanel;

	public void SetCurrentPanel(BasePanelLobby panel) {
		currPanel = panel;
	}

	public void ShowPanel(PanelType panelType) {

		if(currPanel != null) {

			//animate old panel out.
			oldPanel = currPanel;
			oldPanel.AnimateExit();

		}

		//animate new panel in.
		currPanel = panelsList[(int)panelType];
		currPanel.AnimateEnter();
	}

	public void ShowPanelBack(PanelType panelType) {

		if(currPanel != null) {
			
			//animate old panel out.
			oldPanel = currPanel;
			oldPanel.AnimateExitBack();
			
		}
		
		//animate new panel in.
		currPanel = panelsList[(int)panelType];
		currPanel.AnimateEnterBack();
	}
}
