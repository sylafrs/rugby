using UnityEngine;
using System.Collections;

public enum SuperList{
	superNull,
	superTackle,
	superDash,
    superStun
};

[AddComponentMenu("Scripts/Supers/Controller")]
public class superController : myMonoBehaviour {
	
    public SuperList Super;
	
	private Game game;
	private Team team;
	private SuperList currentSuper;
	private Color colorSave;

    public bool SuperActive
    {
        get
        {
            return currentSuper != SuperList.superNull;
        }
    }

    private SuperSettings settings;
		
	//time management for supers
    private float SuperTimeLeft;	
	
	void Start () {
		game 	        = Game.instance;
		team			= gameObject.GetComponent<Team>();
		currentSuper    = SuperList.superNull;

        settings = this.game.settings.Global.Super;

        SuperTimeLeft = 0f;
	}
	
	void Update () {
        team.increaseSuperGauge(0); // clamp
	}
				
	public void updateSuperStatus(){
        if(currentSuper != SuperList.superNull){
			SuperTimeLeft -= Time.deltaTime;

            if (SuperTimeLeft <= 0) {
                endSuper();
            }
		}
	}
	
	public void endSuper(){
		MyDebug.Log("Super Over");
        SuperTimeLeft = 0;
		currentSuper = SuperList.superNull;
		team.speedFactor 	= 1f;
		team.tackleFactor 	= 1f;
		team.ChangePlayersColor(colorSave);

        StopFeedBack(SuperList.superDash);
        StopFeedBack(SuperList.superTackle);
	}

    public void launchSuper()
    {
        if (this.team.SuperGaugeValue == game.settings.Global.Super.superGaugeOffensiveLimitBreak)
        {
            MyDebug.Log("Offensive Super attack !");
            this.launchSuper(this.Super);
            this.team.SuperGaugeValue -= game.settings.Global.Super.superGaugeOffensiveLimitBreak;
            this.game.OnSuper(team, this.Super);
        }
        else
        {
            MyDebug.Log("Need more Power to lauch the offensive super");
            MyDebug.Log("Current Power : " + team.SuperGaugeValue);
            MyDebug.Log("Needed  Power : " + game.settings.Global.Super.superGaugeOffensiveLimitBreak);
        }        
    }
	
	void launchSuper(SuperList super){
		colorSave = team.GetPlayerColor();
		currentSuper  = super;
		switch (super){
			case SuperList.superDash:
				MyDebug.Log("Dash Super attack !");
				team.speedFactor = game.settings.Global.Super.superSpeedScale;
                team.setSpeed();

                this.SuperTimeLeft = settings.SuperDashDurationTime;
			    break;
			
			case SuperList.superTackle:
				MyDebug.Log("Tackle Super attack !");
				team.tackleFactor = game.settings.Global.Super.superTackleBoxScale;
                team.setSpeed();
			    break;

            case SuperList.superStun:
                MyDebug.Log("Stun Super attack !");
                team.opponent.StunEverybodyForSeconds(settings.SuperStunDurationTime);

                this.SuperTimeLeft = settings.SuperStunDurationTime;
                break;
					
			default:
				break;			
		}

        //LauchFeedBack(super);
	}

    public void LaunchFeedback()
    {
        LauchFeedBack(Super);
    }

    void LauchFeedBack(SuperList super)
    {
        team.PlaySuperParticleSystem(super, true);
    }

    void StopFeedBack(SuperList super)
    {
        team.PlaySuperParticleSystem(super, false);
    }
}