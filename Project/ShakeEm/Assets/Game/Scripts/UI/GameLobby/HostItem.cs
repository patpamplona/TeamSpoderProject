using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HostItem : MonoBehaviour {

	[SerializeField] private Text buttonText = null;

	// Use this for initialization
	void Start () {
	
	}

	public Button GetButton() {

		Button tmpButton = this.gameObject.GetComponent<Button>();
		return tmpButton;
	}

	public void SetHostName(string hostName) {

		buttonText.text = hostName;
	}
}
