using UnityEngine;
using System;
using System.Collections;

public class GameUI{
	
	private Game game;
	
	public GameUI(Game _game)
	{
		game = _game;
	}

	public void DrawUI(float blueProgress, float redProgress)
	{
		//int offset = 200;
		
		//time box
		Rect timeBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.timeBoxXPercentage-game.settings.UI.GameUI.timeBoxWidthPercentage/2, 
			game.settings.UI.GameUI.timeBoxYPercentage - game.settings.UI.GameUI.timeBoxHeightPercentage/2, 
			game.settings.UI.GameUI.timeBoxWidthPercentage, game.settings.UI.GameUI.timeBoxHeightPercentage);
		
		//score box
		Rect scoreBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.scoreBoxXPercentage - game.settings.UI.GameUI.scoreBoxWidthPercentage/2,
			game.settings.UI.GameUI.scoreBoxYPercentage - game.settings.UI.GameUI.scoreBoxHeightPercentage/2, 
			game.settings.UI.GameUI.scoreBoxWidthPercentage, game.settings.UI.GameUI.scoreBoxHeightPercentage);
		
		//super gauges
		Rect blueGaugeBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.blueGaugeBoxXPercentage - game.settings.UI.GameUI.blueGaugeBoxWidthPercentage/2,
			game.settings.UI.GameUI.blueGaugeBoxYPercentage - game.settings.UI.GameUI.blueGaugeBoxHeightPercentage/2, 
			game.settings.UI.GameUI.blueGaugeBoxWidthPercentage, game.settings.UI.GameUI.blueGaugeBoxHeightPercentage);

		Rect blueProgressGaugeBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.blueGaugeBoxXPercentage - game.settings.UI.GameUI.blueGaugeBoxWidthPercentage/2,
			game.settings.UI.GameUI.blueGaugeBoxYPercentage - game.settings.UI.GameUI.blueGaugeBoxHeightPercentage/2, 
			game.settings.UI.GameUI.blueGaugeBoxWidthPercentage * blueProgress,
			game.settings.UI.GameUI.blueGaugeBoxHeightPercentage);

        Rect redGaugeBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.redGaugeBoxXPercentage - game.settings.UI.GameUI.redGaugeBoxWidthPercentage / 2,
            game.settings.UI.GameUI.redGaugeBoxYPercentage - game.settings.UI.GameUI.redGaugeBoxHeightPercentage / 2,
            game.settings.UI.GameUI.redGaugeBoxWidthPercentage, game.settings.UI.GameUI.redGaugeBoxHeightPercentage);

		Rect redprogressGaugeBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.redGaugeBoxXPercentage - game.settings.UI.GameUI.redGaugeBoxWidthPercentage/2,
			game.settings.UI.GameUI.redGaugeBoxYPercentage - game.settings.UI.GameUI.redGaugeBoxHeightPercentage/2, 
			game.settings.UI.GameUI.redGaugeBoxWidthPercentage * redProgress, 
			game.settings.UI.GameUI.redGaugeBoxHeightPercentage);
		
		//superbars
		//blue 
		GUI.DrawTexture(blueGaugeBox, game.settings.UI.GameUI.emptyBar);
		GUI.DrawTexture(blueProgressGaugeBox, game.settings.UI.GameUI.blueBar);
		if(blueProgress == 1f)GUI.Label(blueGaugeBox, "SUPER READY !",game.settings.UI.GameUI.superOkTextStyle);
		
		//red
		GUI.DrawTexture(redGaugeBox, game.settings.UI.GameUI.emptyBar);
		GUI.DrawTexture(redprogressGaugeBox, game.settings.UI.GameUI.redBar);
		if(redProgress == 1f)GUI.Label(redGaugeBox, "SUPER READY !",game.settings.UI.GameUI.superOkTextStyle);
		
		//score
		GUI.Label(scoreBox, game.southTeam.nbPoints+"  -  "+ game.northTeam.nbPoints,game.settings.UI.GameUI.gameScoreTextStyle);
		
		//time
		GUI.Label(timeBox,  "Time : "+   (int)(game.settings.Global.Game.period_time - game.Referee.IngameTime), game.settings.UI.GameUI.gameTimeTextStyle);
    }
}
