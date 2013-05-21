using UnityEngine;
using System.Collections;

public enum SuperList{
	//null is the base status
	superNull,
	superTackle,
	superDash,
	superWall
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
	private float DefensiveSuperTimeAmount;
	private float SuperTimeLeft;
	
	
	void Start () {
		game 	        = GameObject.Find("GameDesign").GetComponent<Game>();
		team			= gameObject.GetComponent<Team>();
		currentSuper    = SuperList.superNull;
		
		OffensiveSuperTimeAmount = game.settings.Global.Super.OffensiveSuperDurationTime;
		DefensiveSuperTimeAmount = game.settings.Global.Super.DefensiveSuperDurationTime;
		
		SuperTimeLeft = 0f;
	}
	
	void Update () {
		/*
        if (this.game.state == Game.State.INTRODUCTION)
        {
            return;
        }
        */

		updateSuperValue();
        updateSuperInput();
		updateSuperStatus();
	}
	
	void updateSuperValue(){
		if( (Random.Range(1,20) == 1) && (currentSuper == SuperList.superNull) ){
			team.increaseSuperGauge(0);
		}
	}
	
	void updateSuperInput(){
		
		//InputTouch superOff = game.settings.inputs.superOff;
		//InputTouch superDef = game.settings.inputs.superDef;
		
		//if(game.state == Game.State.PLAYING) {
		
			//offense
			if(team.Player.XboxController != null){
				if(Input.GetKeyDown(game.settings.Inputs.superOff.keyboard(team)) || team.Player.XboxController.GetButtonDown(game.settings.Inputs.superOff.xbox)){
					if(team.SuperGaugeValue == game.settings.Global.Super.superGaugeOffensiveLimitBreak){
						MyDebug.Log("Offensive Super attack !");
						launchSuper(OffensiveSuper, OffensiveSuperTimeAmount);
						team.SuperGaugeValue -= game.settings.Global.Super.superGaugeOffensiveLimitBreak;
                        game.OnSuper(team, SuperList.superDash);
					}else{
						MyDebug.Log("Need more Power to lauch the offensive super");
						MyDebug.Log("Current Power : "+team.SuperGaugeValue);
						MyDebug.Log("Needed  Power : "+game.settings.Global.Super.superGaugeOffensiveLimitBreak);
					}
				}
				
				//defense
				/*
				if(Input.GetKeyDown(superDef.keyboard) || team.Player.XboxController.GetButtonDown(superDef.xbox)){
					if(team.SuperGaugeValue == game.settings.super.superGaugeDefensiveLimitBreak){
						MyDebug.Log("Defensive Super attack !");
							launchSuper(DefensiveSuper, DefensiveSuperTimeAmount);
							team.SuperGaugeValue -= game.settings.super.superGaugeDefensiveLimitBreak;
                            game.OnSuper(team, SuperList.superWall);
					}else{
						MyDebug.Log("Need more Power to lauch the defensive super");
						MyDebug.Log("Current Power : "+team.SuperGaugeValue);
						MyDebug.Log("Needed  Power : "+game.settings.super.superGaugeDefensiveLimitBreak);
					}
				}
				*/
			}
		//}
	}
	
	void updateSuperStatus(){
		if(currentSuper != SuperList.superNull){
			//maj super time
			SuperTimeLeft -= Time.deltaTime;
			//MyDebug.Log("Super Time left  : "+SuperTimeLeft);
			/*if(SuperTimeLeft > 0f){
				switch(currentSuper){
					case SuperList.superDash:{
						break;
					}
					case SuperList.superTackle:{
						break;
					}
					case SuperList.superWall:{
						break;
					}
					default:{
						break;
					}
				}
			}else{
				endSuper();
			}*/

            if (SuperTimeLeft <= 0) {
                endSuper();
            }
		}
	}
	
	void endSuper(){
		MyDebug.Log("Super Over");
		currentSuper = SuperList.superNull;
		team.speedFactor 	= 1f;
		team.tackleFactor 	= 1f;
		team.ChangePlayersColor(colorSave);
		stopDashAttackFeedback();
		stopTackleAttackFeedback();
	}
	
	void launchSuper(SuperList super, float duration){
		colorSave = team.GetPlayerColor();
		SuperTimeLeft = duration;
		currentSuper  = super;
		switch (super){
			case SuperList.superDash:{
				MyDebug.Log("Dash Super attack !");
				launchDashAttackFeedback();
				team.speedFactor = game.settings.Global.Super.superSpeedScale;
				//dash
			break;
			}
			case SuperList.superTackle:{
				MyDebug.Log("Tackle Super attack !");
				launchTackleAttackFeedback();
				team.tackleFactor = game.settings.Global.Super.superTackleBoxScale;
				//tackle
			break;
			}
			case SuperList.superWall:{
				MyDebug.Log("Wall Super attack !");
				//wall
			break;
			}
			default:{
				break;
			}
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