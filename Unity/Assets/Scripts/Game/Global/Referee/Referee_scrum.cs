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

    private Vector3 PlaceScrumBloc()
    {
        Vector3 pos = this.game.Ball.transform.position;

        Transform ne = this.game.refs.positions.scrumFieldNE;
        Transform sw = this.game.refs.positions.scrumFieldSW;

        if (ne && sw)
        {
            float e = Mathf.Min(ne.position.x, sw.position.x);
            float w = Mathf.Max(ne.position.x, sw.position.x);
            float n = Mathf.Max(ne.position.z, sw.position.z);
            float s = Mathf.Min(ne.position.z, sw.position.z);

            if (pos.x > w)
            {
                pos.x = w;
            }

            if (pos.x < e)
            {
                pos.x = e;
            }

            if (pos.z > n)
            {
                pos.z = n;
            }

            if (pos.z < s)
            {
                pos.z = s;
            }
        }

        return pos;
    }

    public void ScrumSwitchToBloc()
    {
        ScrumBloc bloc = this.game.refs.gameObjects.ScrumBloc;
        bloc.transform.position = this.PlaceScrumBloc();

        this.game.southTeam.ShowPlayers(false);
        this.game.northTeam.ShowPlayers(false);
        this.game.Ball.Model.enabled = false;
        bloc.gameObject.SetActive(true);
    }

    private void ScrumSwitchToPlayers()
    {
        ScrumBloc bloc = this.game.refs.gameObjects.ScrumBloc;
        
        this.game.Ball.Model.enabled = true;
        this.game.southTeam.ShowPlayers(true);
        this.game.northTeam.ShowPlayers(true);
        bloc.gameObject.SetActive(false);
    }

    private Team scrumWinners;
    private Vector3 scrumEndPos;
    public float FreezeAfterScrum = 5;
    public void NowScrum()
    {
        ScrumBloc bloc = this.game.refs.gameObjects.ScrumBloc;

        ScrumManager sc = this.game.refs.managers.scrum;
        sc.InitialPosition = this.PlaceScrumBloc();
        sc.ScrumBloc = bloc.transform;

        sc.callback = (Team t, Vector3 endPos) =>
        {
            scrumWinners = t;
            scrumEndPos = endPos;

            this.ScrumGiveSuperPoints(t);

            game.OnResumeSignal(FreezeAfterScrum);
        };

        sc.enabled = true;
    }

    private void ScrumGiveSuperPoints(Team winner)
    {
        SuperSettings settings = this.game.settings.Global.Super;

        this.IncreaseSuper(settings.scrumWinSuperPoints, winner);
        this.IncreaseSuper(settings.scrumLooseSuperPoints, winner.opponent);
    }

    public void ScrumAfter()
    {
        game.Ball.Owner = scrumWinners[0];

        ScrumSwitchToPlayers();
        ScrumEndPlacement(scrumWinners, scrumEndPos);  
    }

    private void ScrumEndPlacement(Team t, Vector3 endPos)
    {
        Transform placement = this.game.refs.placeHolders.scrumPlacement.FindChild("EndPlacement");

        Transform winners = placement.FindChild("WinnerTeam");
        Transform loosers = placement.FindChild("LooserTeam");

        if (t == game.northTeam)
        {
            Vector3 pos = winners.position;
            winners.position = loosers.position;
            loosers.position = pos;

            Quaternion q = winners.rotation;
            winners.rotation = loosers.rotation;
            loosers.rotation = q;
        }

        t.placeUnits(winners, true);
        t.opponent.placeUnits(loosers, true);

        if (t == game.northTeam)
        {
            Vector3 pos = winners.position;
            winners.position = loosers.position;
            loosers.position = pos;

            Quaternion q = winners.rotation;
            winners.rotation = loosers.rotation;
            loosers.rotation = q;

        }
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