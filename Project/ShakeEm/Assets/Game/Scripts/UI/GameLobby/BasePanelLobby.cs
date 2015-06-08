using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class BasePanelLobby : MonoBehaviour {

	private float animDuration = 1.0f;

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

	public virtual void OnShowComplete() {

		// override to capture event when animation completes.
	}

	public virtual void OnHideComplete() {

		// override to capture event when animation completes.
	}

	public void AnimateEnter() {

		this.OnShow();

		rectTrans.localPosition = new Vector3 (0, GameConstants.SCREEN_HEIGHT * -1, 0);

		Sequence seqEnter = new Sequence();
		seqEnter.ApplyCallback(CallbackType.OnComplete, delegate() {
			this.OnShowComplete(); 
		});

		TweenParms parms = new TweenParms();
		parms.Ease(EaseType.EaseInOutCubic);
		parms.Prop("localPosition", Vector3.zero);

		seqEnter.Append(HOTween.To(rectTrans, animDuration, parms));
		seqEnter.Play();
	}

	public void AnimateEnterBack() {

		this.OnShow();
		
		rectTrans.localPosition = new Vector3 (0, GameConstants.SCREEN_HEIGHT, 0);
		
		Sequence seqEnter = new Sequence();
		seqEnter.ApplyCallback(CallbackType.OnComplete, delegate() {
			this.OnShowComplete(); 
		});
		
		TweenParms parms = new TweenParms();
		parms.Ease(EaseType.EaseInOutCubic);
		parms.Prop("localPosition", Vector3.zero);
		
		seqEnter.Append(HOTween.To(rectTrans, animDuration, parms));
		seqEnter.Play();
	}

	public void AnimateExit() {

		this.OnHide();

		rectTrans.localPosition = Vector3.zero;

		Sequence seqExit = new Sequence();
		seqExit.ApplyCallback(CallbackType.OnComplete, delegate() {
			this.OnHideComplete(); 
		});
		
		TweenParms parms = new TweenParms();
		parms.Ease(EaseType.EaseInOutCubic);
		parms.Prop("localPosition", new Vector3 (0, GameConstants.SCREEN_HEIGHT, 0));
		
		seqExit.Append(HOTween.To(rectTrans, animDuration, parms));
		seqExit.Play();
	}

	public void AnimateExitBack() {

		this.OnHide();
		
		rectTrans.localPosition = Vector3.zero;
		
		Sequence seqExit = new Sequence();
		seqExit.ApplyCallback(CallbackType.OnComplete, delegate() {
			this.OnHideComplete(); 
		});
		
		TweenParms parms = new TweenParms();
		parms.Ease(EaseType.EaseInOutCubic);
		parms.Prop("localPosition", new Vector3 (0, GameConstants.SCREEN_HEIGHT * -1, 0));
		
		seqExit.Append(HOTween.To(rectTrans, animDuration, parms));
		seqExit.Play();
	}

	public void ShowPanel() {

		this.OnShow();
		rectTrans.localPosition = Vector3.zero;
		this.OnShowComplete();
	}

	public void HidePanel() {

		this.OnHide();
		rectTrans.localPosition = new Vector3 (0, GameConstants.SCREEN_HEIGHT * -1, 0);
		this.OnHideComplete();
	}
}
