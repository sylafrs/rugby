using UnityEngine;
using System.Collections;

public class EndUI{
		
	public void DrawUI()
	{
        Game _game = Game.instance;
        EndUISettings settings = _game.settings.UI.EndUI;

        Rect resultRect = UIManager.screenRelativeRect(settings.ResultRect);
        Rect resultButtonRect = UIManager.screenRelativeRect(settings.ResultButtonRect);
        Rect resultScoreRect = UIManager.screenRelativeRect(settings.ResultScoreRect);

        string result = "";
        if (_game.southTeam.nbPoints < _game.northTeam.nbPoints)
        {
            result = _game.northTeam.Name + " win !";
        }
        else if (_game.northTeam.nbPoints < _game.southTeam.nbPoints)
        {
            result = _game.southTeam.Name + " win !";
        }
        else
        {
            result = "Draw !";
        }

        GUI.Label(resultRect, result, settings.ResultStyle);
        GUI.Label(resultScoreRect, _game.southTeam.nbPoints + "  -  " + _game.northTeam.nbPoints, settings.ResultScoreStyle);

        GUIStyle btnStyle = GUI.skin.button;
        btnStyle.fontSize = settings.btnFontSize;

        if (GUI.Button(resultButtonRect, "restart", btnStyle))
            _game.Reset();
	}
}
