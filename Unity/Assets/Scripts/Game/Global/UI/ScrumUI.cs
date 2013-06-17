using UnityEngine;
using System.Collections;

public class ScrumUI{
	
	//private Game game;
    private ScrumManager manager;
    private ScrumUISettings settings;
	
	public ScrumUI(Game _game)
	{
		//game = _game;
        manager = _game.refs.managers.scrum;
        settings = _game.settings.UI.ScrumUI;
        StartGUI();
	}
    
	public void DrawUI()
	{
		//if (!manager.ChronoLaunched)
        //{
        //    GUILayout.Label("BEGIN !");
        //}
        //else
        //{
        //    GUILayout.Label("SMASH UNTIL " + (int)manager.TimeRemaining + " !");
        //}

        this.DrawBar();

        if (manager.SuperLoading == 1)
        {
            GUI.DrawTexture(settings.ScrumSpecialRect,
                settings.ScrumSpecialButton, 
				ScaleMode.ScaleToFit);
        }

        ManageMalus();       
	}
	
	void StartGUI()
    {
        Rect rect;

        rect = UIManager.screenRelativeRect(settings.ScrumSpecialRect);
        settings.ScrumSpecialRect = rect;

        rect = UIManager.screenRelativeRect(settings.ScrumBarRect);
        settings.ScrumBarRect = rect;

        rect = UIManager.screenRelativeRect(settings.ScrumLeftSpecialFailedButtonRect);
        settings.ScrumLeftSpecialFailedButtonRect = rect;

        rect = UIManager.screenRelativeRect(settings.ScrumRightSpecialFailedButtonRect);
        settings.ScrumRightSpecialFailedButtonRect = rect;
    }   

    void DrawBar()
    {       
        float leftPercent = (1 + manager.currentPosition) / 2;
        float leftWidth = leftPercent * settings.ScrumBarRect.width;

        Rect rightRect = settings.ScrumBarRect;
        Rect leftRect  = settings.ScrumBarRect;

        leftRect.width = leftWidth;
        rightRect.width -= leftWidth;
        rightRect.x += leftWidth;
        
        GUI.DrawTexture(rightRect, settings.ScrumRightBar);
        GUI.DrawTexture(leftRect,  settings.ScrumLeftBar);
    }

    void ManageMalus()
    {
        float t = Time.time;

        if(manager.MalusSouth != -1 && (t - manager.MalusSouth < settings.MalusDuration)) {
            GUI.DrawTexture(settings.ScrumRightSpecialFailedButtonRect, settings.ScrumSpecialFailedButton);
        }

        if(manager.MalusNorth != -1 && (t - manager.MalusNorth < settings.MalusDuration) ){
            GUI.DrawTexture(settings.ScrumLeftSpecialFailedButtonRect, settings.ScrumSpecialFailedButton);
        }
    }
}
