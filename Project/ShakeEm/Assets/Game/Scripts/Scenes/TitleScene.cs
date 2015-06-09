using UnityEngine;
using System.Collections;

public class TitleScene : BasicScene
{
	public override void EnableScene ()
	{
		PanelController.GetInstance().ShowPanel(PanelType.PANEL_START_GAME);
	}
	
	public override void DisableScene ()
	{

	}
}