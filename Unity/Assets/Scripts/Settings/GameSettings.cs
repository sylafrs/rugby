using UnityEngine;
using XInputDotNetPure; 
using System.Collections;

/**
 * @class ScoreSettings
 * @brief Reglages des points
 * @author Sylvain Lafon
 */
[System.Serializable]
public class ScoreSettings
{
    public int points_drop = 5;
    public int points_essai = 5;
}

/**
 * @class SuperSettings
 * @brief Variab les for settings
 * @author Maxens Dubois
 */
[System.Serializable]
public class SuperSettings
{
    public int superGaugeOffensiveLimitBreak   	= 200;
	public int superGaugeDefensiveLimitBreak   	= 200;
	public int superGaugeMaximum				= 200;
	//for the effect
	//Duration in s
	public float OffensiveSuperDurationTime	= 10f;
	public float DefensiveSuperDurationTime	= 10f;
	public float superSpeedScale            = 1.5f;
	public bool  superUnblockable           = true;
	//passe
    public int passWinSuperPoints  		= 10;
	public int passLooseSuperPoints  	= 5;
	//drop
	public int dropWinSuperPoints  		= 20;
	public int dropLooseSuperPoints  	= 10;
	//try = essais
	public int tryWinSuperPoints  		= 30;
	public int tryLooseSuperPoints  	= 20;
	//conversion = transformation
	public int conversionWinSuperPoints    = 10;
	public int conversionLooseSuperPoints  = 5;
	//touche = touch
	public int touchWinSuperPoints  	= 20;
	public int touchLooseSuperPoints  	= 10;
	//tackle = plaquage
	public int tackleWinSuperPoints    = 20;
	public int tackleLooseSuperPoints  = 5;
	//scrum = melee
	public int scrumWinSuperPoints  	= 30;
	public int scrumLooseSuperPoints  	= 10;
}

/**
 * @class InputSettings
 * @brief Reglages des entrées pour un Gamer
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[System.Serializable]
public class InputSettings
{
    public KeyCode up = KeyCode.UpArrow;
    public KeyCode down = KeyCode.DownArrow;
    public KeyCode left = KeyCode.RightArrow;
    public KeyCode right = KeyCode.LeftArrow;
	public KeyCode passLeft = KeyCode.Alpha1;
    public KeyCode drop = KeyCode.Alpha2;
	public KeyCode plaquer = KeyCode.Alpha3;
    public KeyCode passRight = KeyCode.Alpha4;
}

/** 
 * @class InputSettingsXBOX
 * @brief Reglages des entrées pour un Gamer (manette xBox)
 * @author LAFON Sylvain
 */
[System.Serializable]
public class InputSettingsXBOX
{
    public XBOX_DIRECTION move ;
    public XBOX_BUTTONS passLeft;
    public XBOX_BUTTONS passRight;
    public XBOX_BUTTONS drop;
    public XBOX_BUTTONS plaquer;
	public XBOX_BUTTONS reset;
	public XBOX_BUTTONS enableIA;
	public XBOX_BUTTONS scrumNormal;
	public XBOX_BUTTONS scrumExtra;

    public struct Direction
    {
        public float x, y;
        public Direction(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public static Direction getDirection(XBOX_DIRECTION direction, GamePadState pad)
    {
        Direction d = new Direction(0, 0);
        switch (direction)
        {
            case XBOX_DIRECTION.Pad:
                if (pad.DPad.Down == ButtonState.Pressed)
                    d.y--;
                if (pad.DPad.Up == ButtonState.Pressed)
                    d.y++;
                if (pad.DPad.Right == ButtonState.Pressed)
                    d.x++;
                if (pad.DPad.Left == ButtonState.Pressed)
                    d.x--;
                break;
            case XBOX_DIRECTION.StickLeft:
                d.x = pad.ThumbSticks.Left.X;
                d.y = pad.ThumbSticks.Left.Y;
                break;
            case XBOX_DIRECTION.StickRight:
                d.x = pad.ThumbSticks.Right.X;
                d.y = pad.ThumbSticks.Right.Y;
                break;
        }

        return d;
    }

    public static bool GetButton(XBOX_BUTTONS button, GamePadState pad)
    {
        switch (button)
        {
            case XBOX_BUTTONS.A:
                return pad.Buttons.A == ButtonState.Pressed;
            case XBOX_BUTTONS.B:
                return pad.Buttons.B == ButtonState.Pressed;
            case XBOX_BUTTONS.Back:
                return pad.Buttons.Back == ButtonState.Pressed;
            case XBOX_BUTTONS.LeftShoulder:
                return pad.Buttons.LeftShoulder == ButtonState.Pressed;
            case XBOX_BUTTONS.LeftStick:
                return pad.Buttons.LeftStick == ButtonState.Pressed;
            case XBOX_BUTTONS.RightShoulder:
                return pad.Buttons.RightShoulder == ButtonState.Pressed;
            case XBOX_BUTTONS.RightStick:
                return pad.Buttons.RightStick == ButtonState.Pressed;
            case XBOX_BUTTONS.Start:
                return pad.Buttons.Start == ButtonState.Pressed;
            case XBOX_BUTTONS.X:
                return pad.Buttons.X == ButtonState.Pressed;
            case XBOX_BUTTONS.Y:
                return pad.Buttons.Y == ButtonState.Pressed;
            case XBOX_BUTTONS.TriggerL:
                return (pad.Triggers.Left > 0.5f);
            case XBOX_BUTTONS.TriggerR:
                return (pad.Triggers.Right > 0.5f);
            case XBOX_BUTTONS.PAD_up:
                return pad.DPad.Up == ButtonState.Pressed;
            case XBOX_BUTTONS.PAD_down:
                return pad.DPad.Down == ButtonState.Pressed;
            case XBOX_BUTTONS.PAD_left:
                return pad.DPad.Left == ButtonState.Pressed;
            case XBOX_BUTTONS.PAD_right:
                return pad.DPad.Right == ButtonState.Pressed;
        }

        return false;
    }
}

/**
 * @class GameSettings
 * @brief Classe de reglages
 * @author Sylvain Lafon
 */
[AddComponentMenu("Settings/GameSettings")]
public class GameSettings : myMonoBehaviour {

    public ScoreSettings score;     // Attribution des Points
	public SuperSettings super;     // for super settings
    public InputSettings inputs;    // Controles J1
    public InputSettings inputs2;   // Controles J2

    public InputSettingsXBOX XboxController;

    public float timePlaque = 3;

    public float Vheight = 1;
    public float Vwidth = 1;
    public float LineSpace = 1;

    public float maxTimeHoldingPassButton = 3; // Seconds
}
