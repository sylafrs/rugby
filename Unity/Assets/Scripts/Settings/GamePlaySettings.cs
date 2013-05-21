using UnityEngine;
using System.Collections;

/**
 * @class GamePlaySettings
 * @brief Classe de reglages
 * @author Sylvain Lafon
 */
[System.Serializable]
public class GamePlaySettings {
	
	public GlobalSettings 		Global;
	public GameStatesSettings 	GameStates;
	public InputSettings 		Inputs;
	public UISettings 			UI;

    public bool ToucheRemiseAuCentre = false;
    public bool TransfoRemiseAuCentre = false;
}

