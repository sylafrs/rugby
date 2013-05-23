using UnityEngine;

public partial class Referee
{
    public void PlacePlayersForTransfo()
    {
        game.Ball.transform.position = game.Ball.Owner.BallPlaceHolderTransformation.transform.position;
        //float x = game.Ball.transform.position.x;

        Team t = game.Ball.Owner.Team;

        t.placeUnits(this.game.refs.placeHolders.conversionPlacement.FindChild("TeamShoot"), 1, true);
        t.placeUnit(this.game.refs.placeHolders.conversionPlacement.FindChild("ShootPlayer"), 0, true);
        Team.switchPlaces(t[0], game.Ball.Owner);
        t.opponent.placeUnits(this.game.refs.placeHolders.conversionPlacement.FindChild("TeamLook"), true);

        //Team opponent = game.Ball.Owner.Team.opponent;

        // Joueur face au look At
        Transform butPoint = t.opponent.But.transform.FindChild("Transformation LookAt");
        game.Ball.Owner.transform.LookAt(butPoint);
    }

    public void EnableTransformation()
    {
        TransformationManager tm = this.game.refs.managers.conversion;
        tm.enabled = true;
    }

    private void PlaceTransfoPlaceholders()
    {
        Team t = game.Ball.Owner.Team;
        float x = game.Ball.transform.position.x;

        Transform point = t.opponent.But.transformationPoint;
        point.transform.position = new Vector3(x, 0, point.transform.position.z);

        this.game.refs.placeHolders.conversionPlacement.transform.position = point.position;
        this.game.refs.placeHolders.conversionPlacement.transform.rotation = point.rotation;
    }

    public void OnTry()
    {

        Team t = game.Ball.Owner.Team;

        t.fixUnits = t.opponent.fixUnits = true;
        if (t.Player != null) t.Player.stopMove();
        if (t.opponent.Player != null) t.opponent.Player.stopMove();


        t.nbPoints += game.settings.Global.Game.points_essai;
        Team opponent = game.Ball.Owner.Team.opponent;

        //super for try
        IncreaseSuper(game.settings.Global.Super.tryWinSuperPoints, t);
        IncreaseSuper(game.settings.Global.Super.tryLooseSuperPoints, opponent);

        TransformationManager tm = this.game.refs.managers.conversion;

        tm.ball = game.Ball;
        tm.gamer = t.Player;

        dropBut = t.opponent.But;

        tm.OnLaunch = this.game.OnConversionShot;

        // After the transformation is done, according to the result :
        tm.CallBack = delegate(TransformationManager.Result transformed)
        {

            if (transformed == TransformationManager.Result.TRANSFORMED)
            {
                MyDebug.Log("Transformation");
                t.nbPoints += game.settings.Global.Game.points_transfo;

                //transfo super
                IncreaseSuper(game.settings.Global.Super.conversionWinSuperPoints, t);
            }
            else
            {
                //transfo super
                IncreaseSuper(game.settings.Global.Super.conversionLooseSuperPoints, t);
            }

            IncreaseSuper(game.settings.Global.Super.conversionOpponentSuperPoints, t.opponent);

            if (game.settings.GameStates.MainState.PlayingState.GameActionState.ConvertingState.TransfoRemiseAuCentre || transformed != TransformationManager.Result.GROUND)
            {
                UnitToGiveBallTo = unitToGiveBallAfterConversion();
                this.StartPlacement();
            }

            this.game.OnResumeSignal();
        };

        PlaceTransfoPlaceholders();
    }

    private But dropBut;
    public void OnDropTransformed(But but)
    {
        dropBut = but;

        // On donne les points
        but.Owner.opponent.nbPoints += this.game.settings.Global.Game.points_drop;

        // A faire en caméra :
        this.StartPlacement();
        this.game.Ball.Owner = unitToGiveBallAfterConversion();

        //this.game.TimedDisableIA(3);
    }

    public Unit unitToGiveBallAfterConversion()
    {
        return dropBut.Owner[2];
    }
}