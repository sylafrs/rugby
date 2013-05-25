using UnityEngine;
using System.Collections;

public enum SuperList{
	superNull,
	superTackle,
	superDash
};

[AddComponentMenu("Scripts/Supers/Controller")]
public class superController : myMonoBehaviour {
	
	public SuperList OffensiveSuper;
	public SuperList DefensiveSuper;
	
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
	
	//super color
	public Color superTackleColor;
	public Color superDashColor;
	
	//time management for supers
	private float OffensiveSuperTimeAmount;
	//private float DefensiveSuperTimeAmount;
	private float SuperTimeLeft;
	
	
	void Start () {
		game 	        = Game.instance;
		team			= gameObject.GetComponent<Team>();
		currentSuper    = SuperList.superNull;
		
		OffensiveSuperTimeAmount = game.settings.Global.Super.OffensiveSuperDurationTime;
		//DefensiveSuperTimeAmount = game.settings.Global.Super.DefensiveSuperDurationTime;
		
		SuperTimeLeft = 0f;
	}
	
	void Update () {
		updateSuperValue();
		updateSuperStatus();
	}
	
	void updateSuperValue(){
		if( (Random.Range(1,20) == 1) && (currentSuper == SuperList.superNull) ){
			team.increaseSuperGauge(0);
		}
	}
		
	void updateSuperStatus(){
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
		stopDashAttackFeedback();
		stopTackleAttackFeedback();
	}

    public void launchSuper()
    {
        if (this.team.SuperGaugeValue == game.settings.Global.Super.superGaugeOffensiveLimitBreak)
        {
            MyDebug.Log("Offensive Super attack !");
            this.launchSuper(this.OffensiveSuper, this.OffensiveSuperTimeAmount);
            this.team.SuperGaugeValue -= game.settings.Global.Super.superGaugeOffensiveLimitBreak;
            this.game.OnSuper(team, SuperList.superDash);
        }
        else
        {
            MyDebug.Log("Need more Power to lauch the offensive super");
            MyDebug.Log("Current Power : " + team.SuperGaugeValue);
            MyDebug.Log("Needed  Power : " + game.settings.Global.Super.superGaugeOffensiveLimitBreak);
        }        
    }
	
	void launchSuper(SuperList super, float duration){
		colorSave = team.GetPlayerColor();
		SuperTimeLeft = duration;
		currentSuper  = super;
		switch (super){
			case SuperList.superDash:
				MyDebug.Log("Dash Super attack !");
				launchDashAttackFeedback();
				team.speedFactor = game.settings.Global.Super.superSpeedScale;
                team.setSpeed();
			    break;
			
			case SuperList.superTackle:
				MyDebug.Log("Tackle Super attack !");
				launchTackleAttackFeedback();
				team.tackleFactor = game.settings.Global.Super.superTackleBoxScale;
                team.setSpeed();
			    break;
					
			default:
				break;			
		}
	}
	
	void launchDashAttackFeedback(){		
		team.ChangePlayersColor(superDashColor);
		team.PlaySuperParticleSystem(SuperList.superDash, true);
	}
	
	void launchTackleAttackFeedback(){
		team.ChangePlayersColor(superTackleColor);
		team.PlaySuperParticleSystem(SuperList.superTackle, true);
	}
	
	void stopDashAttackFeedback(){
		team.ChangePlayersColor(colorSave);
		team.PlaySuperParticleSystem(SuperList.superDash, false);
	}
	
	void stopTackleAttackFeedback(){
		team.ChangePlayersColor(colorSave);
		team.PlaySuperParticleSystem(SuperList.superTackle, false);
	}
}