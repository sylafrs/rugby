using UnityEngine;
using System.Collections;

/**
 * @class GameSettings
 * @brief Classe de reglages
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/Settings/GameSettings")]
public class GamePlaySettings : myMonoBehaviour {
	
	public GlobalSettings 		Global;
	public GameStatesSettings 	GameStates;
	public InputSettings 		Inputs;
	public UISettings 			UI;
}