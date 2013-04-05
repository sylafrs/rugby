using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure;

/**
 * @class Gamer
 * @brief Représente un Joueur
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[AddComponentMenu("Scripts/Game/Gamer")]
public class Gamer : myMonoBehaviour
{
    private static int NextGamerId = 0;
    private int id;

    private Team _team;
    public Team Team
    {
        get
        {
            return _team;
        }
        set
        {
            _team = value;
            _team.Player = this;
        }
    }

    public Unit Controlled;
    public Game Game;
    public Vector3 PassDirection;

    public InputSettings Inputs;

    private bool onActionCapture = false;
    private float timeOnActionCapture = 0.0f;

    private bool canMove;
    public PlayerIndex playerIndex;

    public XboxInputs.Controller XboxController;

    public static void initGamerId()
    {
        NextGamerId = 0;
    }

    void Start()
    {
        canMove = true;

        id = NextGamerId;
        NextGamerId++;
        playerIndex = (PlayerIndex)id;

        //Debug.Log(playerIndex.ToString());		
		XboxController = Game.xboxInputs.controllers[id];
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	public void stopMove(){
		canMove = false;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	public void enableMove(){
		canMove = true;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	bool getMoveStatus(){
		return canMove;
	}

    List<Unit> unitsSide;

    void Update()
    {

        if (Inputs == null) return;
		if (Game.state != Game.State.PLAYING) return;

        UpdateMOVE();
        UpdateTACKLE();
        UpdatePASS();
        UpdateDROP();
		UpdateESSAI();
    }

    void UpdatePASS()
    {
        if (Game.Ball.Owner == Controlled)
        {
            if (Input.GetKeyDown(Inputs.passRight.keyboard) || XboxController.GetButtonDown(Inputs.passRight.xbox))
            {
                UpdatePASS_OnPress(true);
            }
            else if (Input.GetKeyDown(Inputs.passLeft.keyboard) || XboxController.GetButtonDown(Inputs.passLeft.xbox))
            {
                UpdatePASS_OnPress(false);
            }
            else if (
                Input.GetKeyUp(Inputs.passRight.keyboard) ||
                Input.GetKeyUp(Inputs.passLeft.keyboard) ||
                XboxController.GetButtonUp(Inputs.passLeft.xbox) ||
                XboxController.GetButtonUp(Inputs.passRight.xbox))
            {
                UpdatePASS_OnRelease();
            }
        }

        if (onActionCapture)
        {
            timeOnActionCapture += Time.deltaTime;
        }
        else
        {
            timeOnActionCapture = 0.0f;
        }
    }

    void UpdatePASS_OnPress(bool right)
    {
        onActionCapture = true;

        if (right)
        {
            PassDirection = this.transform.right;
            Game.Ball.transform.position = Controlled.BallPlaceHolderRight.transform.position;
            unitsSide = Controlled.Team.GetRight(Controlled);
        }
        else
        {
            PassDirection = -this.transform.right;
            Game.Ball.transform.position = Controlled.BallPlaceHolderLeft.transform.position;
            unitsSide = Controlled.Team.GetLeft(Controlled);
        }
    }

    void UpdatePASS_OnRelease()
    {
        if (Controlled == Game.Ball.Owner)
        {
            onActionCapture = false;
            if (timeOnActionCapture > Game.settings.maxTimeHoldingPassButton)
                timeOnActionCapture = Game.settings.maxTimeHoldingPassButton;
            //Debug.DrawRay(this.transform.position, passDirection, Color.red);

            if (unitsSide.Count != 0)
            {
                int unit = Mathf.FloorToInt(unitsSide.Count * timeOnActionCapture / Game.settings.maxTimeHoldingPassButton);
                Debug.Log(unit);

                if (unit == unitsSide.Count) unit--;
                Unit u = unitsSide[unit];

                Controlled.Order = Order.OrderPass(u);
                PassDirection = Vector3.zero;
            }
        }
        else
        {
            // Changer de perso.
        }
    }

    void UpdateTACKLE()
    {
        if (Input.GetKeyDown(Inputs.tackle.keyboard) || XboxController.GetButtonDown(Inputs.tackle.xbox))
        {
            if (Controlled.NearUnits.Count > 0)
            {
                Controlled.Order = Order.OrderPlaquer(Controlled.NearUnits[0]);
            }
        }
    }

    void UpdateMOVE()
    {
        if (!canMove) return;
        Vector3 direction = Vector3.zero;

        InputDirection.Direction d;
        if (XboxController.IsConnected)
        {
            d = XboxController.GetDirection(Inputs.move.xbox);
        }
        else
        {
            d = Inputs.move.keyboard.GetDirection();
        }

        direction += Camera.main.transform.forward * d.y;
        direction += Camera.main.transform.right * d.x;

        if (direction != Vector3.zero)
        {
            Controlled.Order = Order.OrderMove(Controlled.transform.position + direction.normalized, Order.TYPE_DEPLACEMENT.COURSE);
        }
    }

    void UpdateDROP()
    {
        if (Input.GetKeyDown(Inputs.drop.keyboard) || XboxController.GetButtonDown(Inputs.drop.xbox))
        {
            Controlled.Order = Order.OrderDrop(Game.left[0]);
        }
    }
	
	void UpdateESSAI() {
		if(Input.GetKeyDown(Inputs.put.keyboard) || XboxController.GetButtonDown(Inputs.put.xbox)) {
			if(this.Game.Ball.Owner == this.Controlled) {
				if(this.Game.Ball.inZone == this.Team.opponent.Zone) {
					this.Game.OnEssai();
				}
				else {
					Debug.Log ("Pas la bonne zone !");	
				}
			}
			else {
				// Debug inutile si la touche est utilisée autre part ^^
				Debug.Log ("Sans la balle c'est chaud ^^");	
			}
		}
	}
}
