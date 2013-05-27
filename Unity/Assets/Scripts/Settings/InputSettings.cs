using System.Collections;

/**
 * @class InputSettings
 * @brief Reglages des entrées pour un Gamer
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[System.Serializable]
public class InputSettings
{    
    public InputTouch 	
					/* pass */
					shortPass, longPass, 
					/* abilities */
					dropUpAndUnder, dropKick, tackle, changePlayer, put, superOff, conversionTouch,
					/* misc */
					reset, enableIA, skipIntro,
					/* scrum */
					smashButton, superButton;	
	/* touche */
	public InputTouch[] touch;
	
	public InputDirection move, dodge;
}
