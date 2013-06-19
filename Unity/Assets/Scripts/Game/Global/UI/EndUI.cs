using UnityEngine;
using System.Collections;

public class EndUI{
		
	public void DrawUI()
	{
        Game _game = Game.instance;
        EndUISettings settings = _game.settings.UI.EndUI;

        settings.Draw.gameObject.SetActive(false);
        settings.JapanWins.gameObject.SetActive(false);
        settings.MaoriWins.gameObject.SetActive(false);

        if (_game.southTeam.nbPoints < _game.northTeam.nbPoints)
        {
            settings.MaoriWins.gameObject.SetActive(true);
        }
        else if (_game.northTeam.nbPoints < _game.southTeam.nbPoints)
        {
            settings.JapanWins.gameObject.SetActive(true);
        }
        else
        {
            settings.Draw.gameObject.SetActive(true);
        }

        settings.NorthScore.number = _game.northTeam.nbPoints;
        settings.SouthScore.number = _game.southTeam.nbPoints;

        if (_game.northTeam.Player.XboxController.GetButtonDown(_game.settings.Inputs.shortPass.xbox) ||
           _game.southTeam.Player.XboxController.GetButtonDown(_game.settings.Inputs.shortPass.xbox) ||
           Input.GetKeyDown(_game.settings.Inputs.shortPass.keyboardP1) ||
           Input.GetKeyDown(_game.settings.Inputs.shortPass.keyboardP2))
        {
            settings.Abtn.SetActive(false);
            //settings.fade.Inverse();
            //Timer.AddTimer(2, () =>
            //{
                Application.LoadLevel("credits");
            //});
        }

        Timer.AddTimer(2, () =>
        {
            Application.LoadLevel("menu");
        });
	}
}
