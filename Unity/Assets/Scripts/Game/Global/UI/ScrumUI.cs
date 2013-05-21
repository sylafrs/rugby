using UnityEngine;
using System.Collections;

public class ScrumUI{
	
	private Game game;
	
	public ScrumUI(Game _game)
	{
		game = _game;
		StartGUI();
	}
	
	public void DrawUI()
	{
		if (!game.scrumController.ChronoLaunched)
        {
            GUILayout.Label("BEGIN !");
        }
        else
        {
            GUILayout.Label("SMASH UNTIL " + (int)game.scrumController.TimeRemaining + " !");
        }

        this.DrawBar();

        if (game.scrumController.SuperLoading == 1)
        {
            GUI.DrawTexture(game.settings.UI.ScrumUI.ScrumSpecialRect, 
				game.settings.UI.ScrumUI.ScrumSpecialButton, 
				ScaleMode.ScaleToFit);
        }
	}
	
	void StartGUI()
    {
        Rect rect;        
       
        rect = UIManager.screenRelativeRect(game.settings.UI.ScrumUI.ScrumSpecialRect);
		game.settings.UI.ScrumUI.ScrumSpecialRect = rect;

        rect = UIManager.screenRelativeRect(game.settings.UI.ScrumUI.ScrumBarRect);
		game.settings.UI.ScrumUI.ScrumBarRect = rect;
    }

    void DrawBar()
    {       
        float leftPercent = (1 + game.scrumController.currentPosition) / 2;
        float leftWidth = leftPercent * game.settings.UI.ScrumUI.ScrumBarRect.width;

        Rect rightRect = game.settings.UI.ScrumUI.ScrumBarRect;
        Rect leftRect  = game.settings.UI.ScrumUI.ScrumBarRect;

        leftRect.width = leftWidth;
        rightRect.width -= leftWidth;
        rightRect.x += leftWidth;
        
        GUI.DrawTexture(rightRect, game.settings.UI.ScrumUI.ScrumRightBar);
        GUI.DrawTexture(leftRect,  game.settings.UI.ScrumUI.ScrumLeftBar);
    }
}
