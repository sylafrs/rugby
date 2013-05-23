using UnityEngine;

public partial class Referee
{
    public void OnScrum()
    {
        this.game.Ball.Owner = null;

        ScrumCinematicMovement();
        NowScrum();
    }

    public void NowScrum()
    {
        Renderer bloc = this.game.refs.gameObjects.ScrumBloc;
        bloc.transform.position = this.game.Ball.transform.position;

        ScrumManager sc = this.game.refs.managers.scrum;
        sc.InitialPosition = this.game.Ball.transform.position;
        sc.ScrumBloc = bloc.transform;

        this.game.southTeam.ShowPlayers(false);
        this.game.northTeam.ShowPlayers(false);
        this.game.Ball.Model.enabled = false;
        bloc.enabled = true;

        sc.callback = (Team t, Vector3 endPos) =>
        {
            game.Ball.Owner = t[0];

            this.game.Ball.Model.enabled = true;
            this.game.southTeam.ShowPlayers(true);
            this.game.northTeam.ShowPlayers(true);
            bloc.enabled = false;

            game.OnResumeSignal();
        };

        sc.enabled = true;
    }

    public void ScrumCinematicMovement()
    {
        Vector3 pos = this.game.Ball.transform.position;
        Transform cinematic = this.game.refs.placeHolders.scrumPlacement.FindChild("CinematicPlacement");
        cinematic.position = new Vector3(pos.x, 0, pos.z);

        Transform red = cinematic.FindChild("RedTeam");
        Transform blue = cinematic.FindChild("BlueTeam");

        this.game.southTeam.placeUnits(red, false);
        this.game.northTeam.placeUnits(blue, false);
    }
}