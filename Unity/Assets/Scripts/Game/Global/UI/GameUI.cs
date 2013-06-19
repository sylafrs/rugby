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

    //    ShowOutsideScreenUnit();
    }

   // public void ShowOutsideScreenUnit()
   // {
   //     ShowOutsideScreenUnit(game.southTeam.Player.Controlled);
   //     ShowOutsideScreenUnit(game.northTeam.Player.Controlled);
   // }
   //
   // private Vector2 GetOutsideScreenUnit(Transform position)
   // {
   //     float w = Screen.width;
   //     float h = Screen.height;
   //
   //     Camera cam = game.refs.managers.camera.gameCamera.camera;
   //
   //     Vector3 screenPoint = cam.WorldToScreenPoint(position);
   //     screenPoint.y = h - screenPoint.y;
   //
   //     bool inside = true;
   //
   //     if (screenPoint.x > w)
   //     {
   //         inside = false;
   //         screenPoint.x = w;
   //     }
   //     else if (screenPoint.x < 0)
   //     {
   //         inside = false;
   //         screenPoint.x = 0;
   //     }
   //
   //     if (screenPoint.y > h)
   //     {
   //         inside = false;
   //         screenPoint.y = h;
   //     }
   //     else if (screenPoint.y < 0)
   //     {
   //         inside = false;
   //         screenPoint.y = 0;
   //     }
   //
   //     if (screenPoint.z < 0)
   //     {
   //         inside = false;
   //         screenPoint.y = h;
   //         screenPoint.x = w - screenPoint.x;
   //     }
   //
   //     screenPoint.z = 0;
   //
   //     if (inside)
   //     {
   //         return Vector2.zero;
   //     }
   //
   //     return screenPoint;
   // }
   // 
   // private void ShowOutsideScreenUnit(Unit u) {
   //
   //     Vector2 pos = this.GetOutsideScreenUnit(u.transform);
   //     if (pos != Vector2.zero)
   //     {
   //         const float xMin = -684;
   //         const float xMax = 684;
   //         const float yMin = -382;
   //         const float yMax = 382;
   //
   //         Vector2 v = new Vector2();
   //
   //         if (pos.x == 0)
   //         {
   //             v.x = xMin;
   //
   //
   //         }
   //         else if (pos.y == 0)
   //         {
   //             v.y = yMin;
   //         }
   //         else if (pos.x == Screen.width)
   //         {
   //             v.x = xMax;
   //         }
   //         else if (pos.y == Screen.height)
   //         {
   //             v.y = yMax;
   //         }
   //
   //         //GUI.Box(new Rect(pos.x, pos.y, 20, 20), u.name);
   //         //Debug.Log(u + " est hors vision !\n" + test);
   //     }
   // }

}
