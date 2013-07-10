using System.Collections.Generic;
using UnityEngine;

public partial class Referee
{
    public void OnTackle(Unit tackler, Unit tackled)
    {
        if (tackler != null && tackled == null)
        {
            tackler.sm.event_Tackle();
            game.OnDodgeSuccess();
            return;
        }

        TackleManager tm = this.game.refs.managers.tackle;
        if (tm == null)
            throw new UnityException("Game needs a TackleManager !");

        if (tackler == null || tackled == null || tackler.Team == tackled.Team)
            throw new UnityException("Error : " + tackler + " cannot tackle " + tackled + " !");

        tm.game = this.game;
        tm.tackler = tackler;
        tm.tackled = tackled;

        // End of a tackle, according to the result
        tm.callback = (TackleManager.RESULT res) =>
        {
            TacklePlaceUnitsAtEnd(tackler, tackled);
            //
            //switch (res)
            //{
            //    // Plaquage critique, le plaqueur recupère la balle, le plaqué est knockout
            //    case TackleManager.RESULT.CRITIC:
            //        this.game.Ball.Owner = tackler;
            //        tackled.sm.event_Tackle();
            //        break;
            //
            //    // Passe : les deux sont knock-out mais la balle a pu être donnée à un allié
            //    case TackleManager.RESULT.PASS:
            //
            //        Unit unitTo = this.ComputeUnitToPassOnTackle(tackled);                                       
            //
            //        if (unitTo != null && unitTo != game.Ball.Owner)
            //        {
            //            if (tackled.unitAnimator)
            //            {
            //                tackled.unitAnimator.OnTacklePass();
            //            }
            //            game.Ball.Pass(unitTo);
            //        }
            //
            //        tackled.sm.event_Tackle();
            //        tackler.sm.event_Tackle();
            //        LastTackle = Time.time;
            //        break;
            //
            //    // Normal : les deux sont knock-out et la balle est par terre 
            //    // /!\ Mêlée possible /!\
            //    case TackleManager.RESULT.NORMAL:
            //
            //        //super			
                    IncreaseSuper(game.settings.Global.Super.tackleWinSuperPoints, tackler.Team);
                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    game.Ball.TeleportOnGround();
                    LastTackle = Time.time;
            //        break;
            //}
            //
            tackler.Team.Player.UpdateControlled();
            tackled.Team.Player.UpdateControlled();            
        };

        this.TacklePlaceUnitsAtStart(tackler, tackled);
        tm.atUpdate = TacklePlaceUnitsAtUpdate;
        tm.Tackle();
    }

    private Unit ComputeUnitToPassOnTackle(Unit tackled)
    {
        List<Unit> listToCheck = tackled.Team.GetRight(tackled);
        Unit unitTo = null;                    
        
        if (listToCheck.Count == 0)
        {
            listToCheck = tackled.Team.GetLeft(tackled);
        }

        if (listToCheck.Count == 0)
        {
            return null;
        }

        var e = listToCheck.GetEnumerator();
        while (unitTo == null && e.MoveNext())
        {
            Unit u = e.Current;
            if (u.typeOfPlayer == Unit.TYPEOFPLAYER.OFFENSIVE)
            {
                if (tackled.Team == game.southTeam)
                {
                    if (u.transform.position.z < tackled.transform.position.z && u.canCatchTheBall)
                    {
                        unitTo = u;
                    }                    
                }
                else if (tackled.Team == game.northTeam)
                {
                    if (u.transform.position.z > tackled.transform.position.z && u.canCatchTheBall)
                    {
                        unitTo = u;
                    }                    
                }
            }
        }       

        return unitTo;
    }

    private void TacklePlaceUnitsAtStart(Unit tackler, Unit tackled)
    {
        tackler.transform.LookAt(tackled.transform, Vector3.up);
    }

    private void TacklePlaceUnitsAtUpdate(Unit tackler, Unit tackled)
    {
        tackler.transform.LookAt(tackled.transform, Vector3.up);
        tackler.transform.position = Vector3.Lerp(tackler.transform.position, tackled.transform.position, Time.deltaTime * 5);
    }

    private void TacklePlaceUnitsAtEnd(Unit tackler, Unit tackled)
    {
        tackler.transform.forward = Vector3.forward;
    }

    public void ResetScrumTimer()
    {
        LastTackle = -1;
    }

    public void UpdateTackle()
    {
        var settings = game.settings.GameStates.MainState.PlayingState.GameActionState.ScrumingState;
        if (LastTackle != -1)
        {
            if (Time.time - LastTackle > settings.timeToGetOutTackleAreaBeforeScrum)
            {
                LastTackle = -1;
                int right = 0, left = 0;
                for (int i = 0; i < this.game.Ball.scrumFieldUnits.Count; i++)
                {
                    if (this.game.Ball.scrumFieldUnits[i].Team == game.southTeam)
                        right++;
                    else
                        left++;
                }

                if (right >= settings.minPlayersEachTeamToTriggerScrum &&
                    left >= settings.minPlayersEachTeamToTriggerScrum)
                {
                    //if(InScrumZone())
                        game.OnScrum();
                }
            }
        }
    }

    public bool InScrumZone()
    {
        Vector3 pos = this.game.Ball.transform.position;

        Transform ne = this.game.refs.positions.scrumFieldNE;
        Transform sw = this.game.refs.positions.scrumFieldSW;

        float e = Mathf.Min(ne.position.x, sw.position.x);
        float w = Mathf.Max(ne.position.x, sw.position.x);
        float n = Mathf.Max(ne.position.z, sw.position.z);
        float s = Mathf.Min(ne.position.z, sw.position.z);

        Rect r = Rect.MinMaxRect(w, n, e, s);

        pos.y = pos.z;
        return r.Contains(pos);
    }
}