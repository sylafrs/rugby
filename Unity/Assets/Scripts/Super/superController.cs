using UnityEngine;
using System.Collections;

public class superController : MonoBehaviour {
	
	
	public KeyCode OffensiveSuperButton;
	public KeyCode DefensiveSuperButton;
	
	private Game _game;
	
	void Start () {
		_game 	= gameObject.GetComponent<Game>();
	}
	
	void Update () {
		updateBlueValue();
		updateRedValue();
		checkSuperInput();
	}
	
	void updateBlueValue(){
		if(Random.Range(1,20) == 1){
			_game.right.increaseSuperGauge(1);
		}
	}
	
	void updateRedValue(){
		if(Random.Range(1,20) == 1){
			_game.left.increaseSuperGauge(1);
		}
	}
	
	void checkSuperInput(){
		//si le bouton est appuyé
		//si on a assez
		if(_game.right.SuperGaugeValue == _game.settings.super.superGaugeLimitBreak){
			if(Input.GetKeyDown(OffensiveSuperButton)){
				Debug.Log("Offensive Super attack !");
				_game.right.SuperGaugeValue = 0;
			}else{
				if(Input.GetKeyDown(OffensiveSuperButton)){
					Debug.Log("Offensive Super attack !");
					_game.right.SuperGaugeValue = 0;
				}
			}
		}
	}
}
