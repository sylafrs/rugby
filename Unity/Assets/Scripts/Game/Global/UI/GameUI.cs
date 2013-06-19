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
        GameUISettings settings = game.settings.UI.GameUI;

        settings.blueSuper.percent = blueProgress;
		settings.redSuper.percent = redProgress;
		
        settings.blueScore.number = game.southTeam.nbPoints;
        settings.redScore.number = game.northTeam.nbPoints;

		int reverseTime = (int)(game.settings.Global.Game.period_time - game.Referee.IngameTime);
		
        settings.timeNumber.number = reverseTime;

  //      ShowOutsideScreenUnit();
    }

   // public void ShowOutsideScreenUnit()
   // {
   //     GameUISettings settings = game.settings.UI.GameUI;
   //
   //     Vector3 v = ShowOutsideScreenUnit(game.southTeam.Player.Controlled);
   //     settings.j1.transform.position = v;
   //
   //     v = ShowOutsideScreenUnit(game.northTeam.Player.Controlled);
   // 
   // 
   // }
   //    
   // private Vector3 ShowOutsideScreenUnit(Unit u) {
   //
   //     Camera perpCam = Camera.main;
   //     Camera orthoCam = GameObject.FindGameObjectWithTag("UICamera").camera;
   //
   //     Vector3 pos = perpCam.WorldToViewportPoint(u.transform.position);
   //
   //     pos.x = Mathf.Clamp01(pos.x);
   //     pos.y = Mathf.Clamp01(pos.y);
   //
   //     return orthoCam.ViewportToWorldPoint(pos);
   // }

}
