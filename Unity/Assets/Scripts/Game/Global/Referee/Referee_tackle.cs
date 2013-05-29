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
            switch (res)
            {
                // Plaquage critique, le plaqueur recupère la balle, le plaqué est knockout
                case TackleManager.RESULT.CRITIC:
                    this.game.Ball.Owner = tackler;
                    break;

                // Passe : les deux sont knock-out mais la balle a pu être donnée à un allié
                case TackleManager.RESULT.PASS:
                    Unit target = tackled.GetNearestAlly();
                    if (tackled.unitAnimator)
                    {
                        tackled.unitAnimator.OnTacklePass();
                    }
                    game.Ball.Pass(target);

                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    break;

                // Normal : les deux sont knock-out et la balle est par terre 
                // /!\ Mêlée possible /!\
                case TackleManager.RESULT.NORMAL:

                    //super			
                    game.Ball.TeleportOnGround();
                    IncreaseSuper(game.settings.Global.Super.tackleWinSuperPoints, tackler.Team);
                    tackled.sm.event_Tackle();
                    tackler.sm.event_Tackle();
                    break;
            }

            LastTackle = Time.time;
        };

        tm.Tackle();
    }
	
    public void UpdateTackle()
    {
        if (LastTackle != -1)
        {
            // TODO cte : 2 -> temps pour checker
            if (Time.time - LastTackle > game.settings.GameStates.MainState.PlayingState.GameActionState.ScrumingState.timeToGetOutTackleAreaBeforeScrum)
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

                // TODO cte : 3 --> nb de joueurs de chaque equipe qui doivent etre dans la zone
                if (right >= game.settings.GameStates.MainState.PlayingState.GameActionState.ScrumingState.minPlayersEachTeamToTriggerScrum &&
                    left >= game.settings.GameStates.MainState.PlayingState.GameActionState.ScrumingState.minPlayersEachTeamToTriggerScrum)
                {
                    game.OnScrum();
                    //goScrum = true;
                    //
                }
            }
        }
    }
}