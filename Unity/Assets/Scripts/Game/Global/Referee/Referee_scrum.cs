using UnityEngine;

public partial class Referee
{
    public void OnScrum()
    {
        foreach (Unit u in this.game.southTeam)
        {
            u.canCatchTheBall = false;
        }

        foreach (Unit u in this.game.northTeam)
        {
            u.canCatchTheBall = false;
        }

        this.game.Ball.Put();
        
        //ScrumCinematicMovement();
        //NowScrum();
    }

    public void ScrumSwitchToBloc()
    {
        Renderer bloc = this.game.refs.gameObjects.ScrumBloc;
        bloc.transform.position = this.game.Ball.transform.position;

        this.game.southTeam.ShowPlayers(false);
        this.game.northTeam.ShowPlayers(false);
        this.game.Ball.Model.enabled = false;
        bloc.enabled = true;
    }

    private void ScrumSwitchToPlayers()
    {
        Renderer bloc = this.game.refs.gameObjects.ScrumBloc;
        
        this.game.Ball.Model.enabled = true;
        this.game.southTeam.ShowPlayers(true);
        this.game.northTeam.ShowPlayers(true);
        bloc.enabled = false;
    }

    public void NowScrum()
    {
        Renderer bloc = this.game.refs.gameObjects.ScrumBloc;

        ScrumManager sc = this.game.refs.managers.scrum;
        sc.InitialPosition = this.game.Ball.transform.position;
        sc.ScrumBloc = bloc.transform;

        sc.callback = (Team t, Vector3 endPos) =>
        {
            game.Ball.Owner = t[0];

            ScrumSwitchToPlayers();
            ScrumEndPlacement(t, endPos);
            

            game.OnResumeSignal();
        };

        sc.enabled = true;
    }

    private void ScrumEndPlacement(Team t, Vector3 endPos)
    {
        Transform placement = this.game.refs.placeHolders.scrumPlacement.FindChild("EndPlacement");
    }

    public void ScrumCinematicMovement()
    {
        Vector3 pos = this.game.Ball.transform.position;
        Transform cinematic = this.game.refs.placeHolders.scrumPlacement.FindChild("CinematicPlacement");
        cinematic.position = new Vector3(pos.x, 0, pos.z);

        Transform red = cinematic.FindChild("RedTeam");
        Transform blue = cinematic.FindChild("BlueTeam");

        const bool teleport = false;

        foreach (Unit u in this.game.southTeam)
        {
            u.sm.event_Untackle();
        }

        foreach (Unit u in this.game.northTeam)
        {
            u.sm.event_Untackle();
        }

        this.game.southTeam.placeUnits(blue, teleport);
        this.game.northTeam.placeUnits(red, teleport);    
    }
}