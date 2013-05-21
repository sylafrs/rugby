using UnityEngine;
using System.Collections;

public class ScrumUI{
	
	private Game game;
    private scrumController manager;
	
	public ScrumUI(Game _game)
	{
		game = _game;
        manager = _game.refs.managers.scrum;
		StartGUI();
	}
	
	public void DrawUI()
	{
		if (!manager.ChronoLaunched)
        {
            GUILayout.Label("BEGIN !");
        }
        else
        {
            GUILayout.Label("SMASH UNTIL " + (int)manager.TimeRemaining + " !");
        }

        this.DrawBar();

        if (manager.SuperLoading == 1)
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
        float leftPercent = (1 + manager.currentPosition) / 2;
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
