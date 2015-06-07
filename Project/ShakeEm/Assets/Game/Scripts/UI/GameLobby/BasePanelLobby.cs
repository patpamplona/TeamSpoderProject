using UnityEngine;
using System.Collections;

public class BasePanelLobby : MonoBehaviour {

	RectTransform rectTrans = null;

	public virtual void Awake() {

		rectTrans = this.gameObject.GetComponent<RectTransform> ();
	}

	public virtual void Start() {

		HidePanel();
	}

	public virtual void OnShow() {

		// override to initialize before showing the panel
	}

	public virtual void OnHide() {

		// override to capture on hide event.
	}

	public void ShowPanel() {

		this.OnShow();
		rectTrans.localPosition = Vector3.zero;
	}

	public void HidePanel() {

		rectTrans.localPosition = new Vector3 (0, GameConstants.SCREEN_HEIGHT * -1, 0);
		this.OnHide();
	}
}
