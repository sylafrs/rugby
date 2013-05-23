using UnityEngine;

public partial class Referee
{
    public void PlacePlayersForTouch()
    {
        Team interceptTeam = game.Ball.Team;
        Team touchTeam = interceptTeam.opponent;

        // Fixe les unités			
        if (interceptTeam.Player != null) interceptTeam.Player.stopMove();
        if (touchTeam.Player != null) touchTeam.Player.stopMove();
        interceptTeam.fixUnits = touchTeam.fixUnits = true;

        // Bouttons pour la touche.			
        interceptTeam[0].buttonIndicator.ApplyTexture("A");
        interceptTeam[1].buttonIndicator.ApplyTexture("B");
        interceptTeam[2].buttonIndicator.ApplyTexture("X");

        touchTeam[1].buttonIndicator.ApplyTexture("A");
        touchTeam[2].buttonIndicator.ApplyTexture("B");
        touchTeam[3].buttonIndicator.ApplyTexture("X");

        interceptTeam[0].buttonIndicator.target.renderer.enabled = true;
        interceptTeam[1].buttonIndicator.target.renderer.enabled = true;
        interceptTeam[2].buttonIndicator.target.renderer.enabled = true;

        touchTeam[1].buttonIndicator.target.renderer.enabled = true;
        touchTeam[2].buttonIndicator.target.renderer.enabled = true;
        touchTeam[3].buttonIndicator.target.renderer.enabled = true;

        // Touche à droite ?
        bool right = (this.game.refs.placeHolders.touchPlacement.position.x > 0);

        // Place les unités


        Transform blueTeam, redTeam, rightTeam, leftTeam;
        rightTeam = this.game.refs.placeHolders.touchPlacement.FindChild("RightTeam");
        leftTeam = this.game.refs.placeHolders.touchPlacement.FindChild("LeftTeam");

        if (right)
        {
            redTeam = rightTeam;
            blueTeam = leftTeam;
        }
        else
        {
            redTeam = leftTeam;
            blueTeam = rightTeam;
        }

        Transform interceptConfiguration = this.game.refs.placeHolders.touchPlacement.FindChild("InterceptionTeam");
        if (interceptTeam == this.game.northTeam/*(red)*/)
        {
            interceptConfiguration.transform.position = redTeam.transform.position;
            interceptConfiguration.transform.rotation = redTeam.transform.rotation;
        }
        else
        {
            interceptConfiguration.transform.position = blueTeam.transform.position;
            interceptConfiguration.transform.rotation = blueTeam.transform.rotation;
        }

        interceptTeam.placeUnits(interceptConfiguration, true);

        Transform passConfiguration = this.game.refs.placeHolders.touchPlacement.FindChild("TouchTeam");
        if (touchTeam == this.game.northTeam/*(red)*/)
        {
            passConfiguration.transform.position = redTeam.transform.position;
            passConfiguration.transform.rotation = redTeam.transform.rotation;
        }
        else
        {
            passConfiguration.transform.position = blueTeam.transform.position;
            passConfiguration.transform.rotation = blueTeam.transform.rotation;
        }

        touchTeam.placeUnits(passConfiguration, 1, true);

        Transform passUnitPosition = this.game.refs.placeHolders.touchPlacement.FindChild("TouchPlayer");
        touchTeam.placeUnit(passUnitPosition, 0, true);


        game.Ball.Owner = touchTeam[0];
        game.refs.managers.camera.setTarget(null);
    }

    public void OnTouch(Touche t)
    {
        if (t == null || t.a == null || t.b == null)
        {
            return;
        }

        // Indique que le jeu passe en mode "Touche"			

        // Placement dans la scène de la touche.
        Vector3 pos = Vector3.Project(game.Ball.transform.position - t.a.position, t.b.position - t.a.position) + t.a.position;
        pos.y = 0; // A terre           

        if (this.game.refs.placeHolders.touchPlacement == null)
        {
            throw new UnityException("I need to know how place the players when a touch occurs");
        }

        bool right = (pos.x > 0);

        if (right)
        {
            this.game.refs.placeHolders.touchPlacement.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            this.game.refs.placeHolders.touchPlacement.localRotation = Quaternion.Euler(0, 90, 0);
        }

        this.game.refs.placeHolders.touchPlacement.position = pos;

        Team interceptTeam = game.Ball.Team;
        Team touchTeam = interceptTeam.opponent;

        // Règlage du mini-jeu
        TouchManager tm = this.game.refs.managers.touch;

        tm.game = this.game;

        // On indique les équipes
        tm.gamerIntercept = interceptTeam.Player;
        tm.gamerTouch = touchTeam.Player;

        // On indique si l'un ou l'autre sera fait au pif
        // TODO : patch j2
        tm.randomTouch = (tm.gamerTouch == null || (tm.gamerTouch == game.northTeam.Player && !game.northTeam.Player.XboxController.IsConnected));
        tm.randomIntercept = (tm.gamerIntercept == null || (tm.gamerTouch == game.northTeam.Player && !game.northTeam.Player.XboxController.IsConnected));

        // Fonction à appeller à la fin de la touche
        tm.CallBack = delegate(TouchManager.Result result, int id)
        {

            // Charger le super à la touche

            // On donne la balle à la bonne personne
            if (result == TouchManager.Result.INTERCEPTION)
            {
                game.Ball.Owner = interceptTeam[id];
                //super
                this.IncreaseSuper(game.settings.Global.Super.touchInterceptSuperPoints, interceptTeam);
                this.IncreaseSuper(game.settings.Global.Super.touchLooseSuperPoints, touchTeam);
            }
            else
            {
                game.Ball.Owner = touchTeam[id + 1];
                //super
                this.IncreaseSuper(game.settings.Global.Super.touchWinSuperPoints, touchTeam);
            }

            // Indicateur de bouton
            // foreach(Unit u in interceptTeam)
            // 	u.buttonIndicator.target.renderer.enabled = false;
            // 	
            // foreach(Unit u in touchTeam)
            // 	u.buttonIndicator.target.renderer.enabled = false;
            // 	
            // Retour en jeu
            // interceptTeam.fixUnits = touchTeam.fixUnits = false;	
            // if(interceptTeam.Player != null) interceptTeam.Player.enableMove();
            // if(touchTeam.Player != null) touchTeam.Player.enableMove();

            game.OnResumeSignal();
        };

        tm.enabled = true;
    }

}