using UnityEngine;
using System.Collections;

public enum SuperList{
	//null is the base status
	superNull,
	superTackle,
	superDash,
	superWall
};

public class superController : MonoBehaviour {
	
	public KeyCode OffensiveSuperButton;
	public KeyCode DefensiveSuperButton;
	
	public SuperList OffensiveSuper;
	public SuperList DefensiveSuper;
	
	private Game _game;
	private Team _team;
	private SuperList currentSuper;
	private Color colorSave;
	
	//super color
	public Color superTackleColor;
	public Color superDashColor;
	
	//time management for supers
	private float OffensiveSuperTimeAmount;
	private float DefensiveSuperTimeAmount;
	private float SuperTimeLeft;
	
	
	void Start () {
		_game 	        = GameObject.Find("Scene").GetComponent<Game>();
		_team			= gameObject.GetComponent<Team>();
		currentSuper    = SuperList.superNull;
		
		OffensiveSuperTimeAmount = _game.settings.super.OffensiveSuperDurationTime;
		DefensiveSuperTimeAmount = _game.settings.super.DefensiveSuperDurationTime;
		
		SuperTimeLeft = 0f;
	}
	
	void Update () {
		updateSuperValue();
		updateSuperInput();
		updateSuperStatus();
	}
	
	void updateSuperValue(){
		if( (Random.Range(1,20) == 1) && (currentSuper == SuperList.superNull) ){
			_team.increaseSuperGauge(5);
		}
	}
	
	void updateSuperInput(){
		
		//offense
		if(Input.GetKeyDown(OffensiveSuperButton)){
			if(_team.SuperGaugeValue == _game.settings.super.superGaugeOffensiveLimitBreak){
				Debug.Log("Offensive Super attack !");
				launchSuper(OffensiveSuper, OffensiveSuperTimeAmount);
				_team.SuperGaugeValue -= _game.settings.super.superGaugeOffensiveLimitBreak;
			}else{
				Debug.Log("Need more Power to lauch the offensive super");
				Debug.Log("Current Power : "+_team.SuperGaugeValue);
				Debug.Log("Needed  Power : "+_game.settings.super.superGaugeOffensiveLimitBreak);
			}
		}
		
		//defense
		if(Input.GetKeyDown(DefensiveSuperButton)){
			if(_team.SuperGaugeValue == _game.settings.super.superGaugeDefensiveLimitBreak){
				Debug.Log("Defensive Super attack !");
					launchSuper(DefensiveSuper, DefensiveSuperTimeAmount);
					_team.SuperGaugeValue -= _game.settings.super.superGaugeDefensiveLimitBreak;
			}else{
				Debug.Log("Need more Power to lauch the defensive super");
				Debug.Log("Current Power : "+_team.SuperGaugeValue);
				Debug.Log("Needed  Power : "+_game.settings.super.superGaugeDefensiveLimitBreak);
			}
		}
	}
	
	void updateSuperStatus(){
		if(currentSuper != SuperList.superNull){
			//maj super time
			SuperTimeLeft -= Time.deltaTime;
			//Debug.Log("Super Time left  : "+SuperTimeLeft);
			if(SuperTimeLeft > 0f){
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
			}
		}
	}
	
	void endSuper(){
		Debug.Log("Super Over");
		currentSuper = SuperList.superNull;
		_team.speedFactor 	= 1f;
		_team.tackleFactor 	= 1f;
		_team.ChangePlayersColor(colorSave);
		stopDashAttackFeedback();
		stopTackleAttackFeedback();
	}
	
	void launchSuper(SuperList super, float duration){
		colorSave = _team.GetPlayerColor();
		SuperTimeLeft = duration;
		currentSuper  = super;
		switch (super){
			case SuperList.superDash:{
				Debug.Log("Dash Super attack !");
				launchDashAttackFeedback();
				_team.speedFactor = _game.settings.super.superSpeedScale;
				//dash
			break;
			}
			case SuperList.superTackle:{
				Debug.Log("Tackle Super attack !");
				launchTackleAttackFeedback();
				_team.tackleFactor = _game.settings.super.superTackleBoxScale;
				//tackle
			break;
			}
			case SuperList.superWall:{
				Debug.Log("Wall Super attack !");
				//wall
			break;
			}
			default:{
				break;
			}
		}
	}
	
	
	
	void launchDashAttackFeedback(){		
		_team.ChangePlayersColor(superDashColor);
		_team.PlaySuperParticleSystem(SuperList.superDash, true);
	}
	
	void launchTackleAttackFeedback(){
		_team.ChangePlayersColor(superTackleColor);
		_team.PlaySuperParticleSystem(SuperList.superTackle, true);
	}
	
	void stopDashAttackFeedback(){
		_team.ChangePlayersColor(colorSave);
		_team.PlaySuperParticleSystem(SuperList.superDash, false);
	}
	
	void stopTackleAttackFeedback(){
		_team.ChangePlayersColor(colorSave);
		_team.PlaySuperParticleSystem(SuperList.superTackle, false);
	}
}