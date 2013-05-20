using UnityEngine;
using System.Collections;

public class GameUI{
	
	private Game game;
	
	public GameUI(Game _game)
	{
		game = _game;
	}

	public void DrawUI(float blueProgress, float redProgress)
	{
		int offset = 200;
		
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
		
		//scrum bars
		Rect scrumBarBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.scrumBarBoxXPercentage - game.settings.UI.GameUI.scrumBarBoxWidthPercentage/2,
			game.settings.UI.GameUI.scrumBarBoxYPercentage - game.settings.UI.GameUI.scrumBarBoxHeightPercentage/2, 
			game.settings.UI.GameUI.scrumBarBoxWidthPercentage, game.settings.UI.GameUI.scrumBarBoxHeightPercentage);

		Rect scrumRedBarBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.scrumBarBoxXPercentage - game.settings.UI.GameUI.scrumBarBoxWidthPercentage/2,
			game.settings.UI.GameUI.scrumBarBoxYPercentage - game.settings.UI.GameUI.scrumBarBoxHeightPercentage/2, 
			game.settings.UI.GameUI.scrumBarBoxWidthPercentage, game.settings.UI.GameUI.scrumBarBoxHeightPercentage);
		
		//scrum special
		Rect scrumSpecialBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.scrumSpecialBoxXPercentage - game.settings.UI.GameUI.scrumSpecialBoxWidthPercentage/2,
			game.settings.UI.GameUI.scrumSpecialBoxYPercentage -game.settings.UI.GameUI. scrumSpecialBoxHeightPercentage/2, 
			game.settings.UI.GameUI.scrumSpecialBoxWidthPercentage, game.settings.UI.GameUI.scrumSpecialBoxHeightPercentage);
		
		//Time before Scrum
		Rect scrumTimeBox = UIManager.screenRelativeRect(game.settings.UI.GameUI.scrumTimeBoxXPercentage - game.settings.UI.GameUI.scrumTimeBoxWidthPercentage/2,
			game.settings.UI.GameUI.scrumTimeBoxYPercentage - game.settings.UI.GameUI.scrumTimeBoxHeightPercentage/2, 
			game.settings.UI.GameUI.scrumTimeBoxWidthPercentage, game.settings.UI.GameUI.scrumTimeBoxHeightPercentage);
		
		//player on left Box
		float playerLeftBoxWidth  = 25;
		float playerLeftBoxHeight = 10;	
		Rect playerLeftBox = UIManager.screenRelativeRect(5 - playerLeftBoxWidth/2, 0 + playerLeftBoxHeight/2, playerLeftBoxWidth, playerLeftBoxHeight);
	
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
		GUI.Label(scoreBox, game.right.nbPoints+"  -  "+game.left.nbPoints,game.settings.UI.GameUI.gameScoreTextStyle);
		
		//time
		GUI.Label(timeBox,  "Time : "+(int)game.arbiter.IngameTime, game.settings.UI.GameUI.gameTimeTextStyle);
	}
}
