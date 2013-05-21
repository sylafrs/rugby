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
		game 	        = Game.instance;
		team			= gameObject.GetComponent<Team>();
		currentSuper    = SuperList.superNull;
		
		OffensiveSuperTimeAmount = game.settings.super.OffensiveSuperDurationTime;
		DefensiveSuperTimeAmount = game.settings.super.DefensiveSuperDurationTime;
		
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
		
		InputTouch superOff = game.settings.Inputs.superOff;
		//InputTouch superDef = game.settings.Inputs.superDef;
		
		//if(game.state == Game.State.PLAYING) {
		
			//offense
			if(team.Player.XboxController != null){
				if(Input.GetKeyDown(superOff.keyboard(team)) || team.Player.XboxController.GetButtonDown(superOff.xbox)){
					if(team.SuperGaugeValue == game.settings.super.superGaugeOffensiveLimitBreak){
						
						launchSuper(OffensiveSuper, OffensiveSuperTimeAmount);
						team.SuperGaugeValue -= game.settings.super.superGaugeOffensiveLimitBreak;
                        game.OnSuper(team, SuperList.superDash);
					}else{
						
						
						
					}
				}
				
				//defense
				/*
				if(Input.GetKeyDown(superDef.keyboard) || team.Player.XboxController.GetButtonDown(superDef.xbox)){
					if(team.SuperGaugeValue == game.settings.super.superGaugeDefensiveLimitBreak){
						
							launchSuper(DefensiveSuper, DefensiveSuperTimeAmount);
							team.SuperGaugeValue -= game.settings.super.superGaugeDefensiveLimitBreak;
                            game.OnSuper(team, SuperList.superWall);
					}else{
						
						
						
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
			//
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
				
				launchDashAttackFeedback();
				team.speedFactor = game.settings.super.superSpeedScale;
				//dash
			break;
			}
			case SuperList.superTackle:{
				
				launchTackleAttackFeedback();
				team.tackleFactor = game.settings.super.superTackleBoxScale;
				//tackle
			break;
			}
			case SuperList.superWall:{
				
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