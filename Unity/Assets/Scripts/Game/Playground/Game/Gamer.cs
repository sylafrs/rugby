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
	private Unit unitTo;
	private InputDirection.Direction stickDirection;

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

        if (XboxController == null)
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

    //List<Unit> unitsSide;

    void Update()
    {
        if(XboxController == null)
            XboxController = Game.xboxInputs.controllers[id];

        if (Inputs == null) 
            return;

        if (UpdateRESET())
            return;
		
		/*
		if (Game.state != Game.State.PLAYING) 
            return;
           */

		UpdateStickDirection();	
        UpdateMOVE();
        UpdateTACKLE();
        UpdatePASS();
        UpdateDROP();
		UpdateESSAI();
		UpdatePLAYER();
        
    }

    bool UpdateRESET()
    {
        if (Input.GetKeyUp(Inputs.reset.keyboardP1) || Input.GetKeyUp(Inputs.reset.keyboardP1) || XboxController.GetButtonUp(Inputs.reset.xbox))
        {
            Game.Reset();
            return true;
        }

        return false;
    }

	//maxens dubois
	//get the 2d vector
	void UpdateStickDirection()
	{
		if (XboxController.IsConnected)
		{
			stickDirection = XboxController.GetDirection(Inputs.move.xbox);
		}
		else
		{
			stickDirection = Inputs.move.keyboard.GetDirection();
		}
	}

    void UpdatePASS()
    {
        if (Game.Ball.Owner == Controlled && Game.Ball.inZone == null)  
        {

            int side = 0;

            if (stickDirection.x > 0.1f)
            {
                side = 1;
            }
            else if (stickDirection.x < 0.1f)
            {
                side = -1;
            }

            if (Game.cameraManager.TeamLooked == Game.left)
            {
                side *= -1;
            }

			if (Input.GetKeyDown(Inputs.shortPass.keyboard(Controlled.Team)) || XboxController.GetButtonDown(Inputs.shortPass.xbox))
            {
				if (side > 0)
				{
					if (Controlled.Team.GetRight(Controlled).Count > 0)
					{
						unitTo = Controlled.Team.GetRight(Controlled)[0];
					}
					else
					{
						unitTo = null;
						return;
					}

				}
				else if (side < 0)
				{
					if (Controlled.Team.GetLeft(Controlled).Count > 0)
					{
						unitTo = Controlled.Team.GetLeft(Controlled)[0];
					}
					else
					{
						unitTo = null;
						return;
					}
				}

            }
			else if (Input.GetKeyDown(Inputs.longPass.keyboard(Controlled.Team)) || XboxController.GetButtonDown(Inputs.longPass.xbox))
            {
				if (side > 0)
				{
                    List<Unit> rightAllies = Controlled.Team.GetRight(Controlled);
                    if (rightAllies.Count > 1)
					{
                        unitTo = rightAllies[1];
					}
					else
					{
						unitTo = null;
						return;
					}


				}
				else if (side < 0)
				{
                    List<Unit> leftAllies = Controlled.Team.GetLeft(Controlled);
                    if (leftAllies.Count > 1)
					{
                        unitTo = leftAllies[1];
					}
					else
					{
						unitTo = null;
						return;
					}
				}
            }
				
            else if (
                Input.GetKeyUp(Inputs.shortPass.keyboard(Controlled.Team)) ||
				Input.GetKeyUp(Inputs.longPass.keyboard(Controlled.Team)) ||
				XboxController.GetButtonUp(Inputs.shortPass.xbox) ||
				XboxController.GetButtonUp(Inputs.longPass.xbox))
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
				//unitsSide = Controlled.Team.GetRight(Controlled);
			}
			else
			{
				PassDirection = -this.transform.right;
				Game.Ball.transform.position = Controlled.BallPlaceHolderLeft.transform.position;
				//unitsSide = Controlled.Team.GetLeft(Controlled);
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
			/*
            if (unitsSide.Count != 0)
            {
                int unit = Mathf.FloorToInt(unitsSide.Count * timeOnActionCapture / Game.settings.maxTimeHoldingPassButton);
                MyDebug.Log(unit);

                if (unit == unitsSide.Count) unit--;
                Unit u = unitsSide[unit];

                Controlled.Order = Order.OrderPass(u);
                PassDirection = Vector3.zero;
            }*/
			if ( unitTo != null && unitTo != Game.Ball.Owner )
				Controlled.Order = Order.OrderPass(unitTo);
			//PassDirection = Vector3.zero;
        }
        else
        {
            // Changer de perso.
        }
    }

    void UpdateTACKLE()
    {
        if (Input.GetKeyDown(Inputs.tackle.keyboard(this.Game.Ball.Owner.Team)) || XboxController.GetButtonDown(Inputs.tackle.xbox))
        {
            Unit owner = this.Game.Ball.Owner;
            if (owner != null && owner.Team != this.Team && Controlled.NearUnits.Contains(owner))
            {
                Controlled.Order = Order.OrderPlaquer(owner);
            }
        }
    }
	
	void UpdatePLAYER()
	{
		if (Game.Ball.Owner == null && ((Game.Ball.PreviousOwner.Team == Controlled.Team && Game.Ball.PreviousOwner.isTackled)
										|| Controlled.isTackled))
		{
            Controlled.Order = Order.OrderNothing();

			Controlled.IndicateSelected(false);
			Controlled = GetUnitNear();
			Controlled.IndicateSelected(true);
		}

		if ( Controlled != null && Controlled.Team != null && Game != null && Game.Ball != null)
		{	
			if ((Game.Ball.Owner == null || Game.Ball.Owner.Team != Team) &&
                (Input.GetKeyDown(Inputs.changePlayer.keyboard(Controlled.Team)) || XboxController.GetButtonDown(Inputs.changePlayer.xbox)))
	        {
				Controlled.IndicateSelected(false);
				Controlled = GetUnitNear();
                Controlled.Order = Order.OrderNothing();
				Controlled.IndicateSelected(true);
				
				//MyDebug.Log("joueur controllé " + Controlled);
	        }
			
			Order.TYPE_POSITION typePosition = Team.PositionInMap( Controlled );
			//MyDebug.Log("pos in map : " + typePosition);
			
            if (Game.Ball.Owner == null || Game.Ball.Owner.Team == Team)
			{				
				//offensiveside
				foreach (Unit u in Controlled.Team)
			    {
			    	if (u != Controlled)
			        {
						u.Order = Order.OrderOffensiveSide(Controlled, new Vector3(Game.settings.Vheight, 0, Game.settings.Vwidth/1.5f), Controlled.Team.right, typePosition);
			        }
				}
			}
			else
			{
				//defensiveside
				foreach (Unit u in Controlled.Team)
			    {
			    	if (u != Controlled)
			        {
						u.Order = Order.OrderDefensiveSide(Controlled, new Vector3(Game.settings.Vheight, 0, Game.settings.Vwidth/1.5f), Controlled.Team.right, typePosition);
			        }
				}
			}
             
		}
	}
	
	public Unit GetUnitNear()
	{
		float dist;
		float min = Vector3.SqrMagnitude(Game.Ball.transform.position - Controlled.Team[0].transform.position);
		Unit near = (Controlled.Team[0].isTackled ? Controlled.Team[1] : Controlled.Team[0]);
		
		foreach( Unit u in Controlled.Team )
		{
			dist = Vector3.SqrMagnitude(Game.Ball.transform.position - u.transform.position);
			
			if ( dist < min && !u.isTackled )
			{
				near = u;
				min = dist;
			}
		}
		return near;
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
		if (Input.GetKeyDown(Inputs.dropUpAndUnder.keyboard(Controlled.Team)) || XboxController.GetButtonDown(Inputs.dropUpAndUnder.xbox))
        {
            Controlled.Order = Order.OrderDropUpAndUnder(Game.left[0]);
        }
		else if (Input.GetKeyDown(Inputs.dropKick.keyboard(Controlled.Team)) || XboxController.GetButtonDown(Inputs.dropKick.xbox))
		{
			Controlled.Order = Order.OrderDropKick(Game.left[0]);
		}
    }
	
	void UpdateESSAI() {
		if(Input.GetKeyDown(Inputs.put.keyboard(Controlled.Team)) || XboxController.GetButtonDown(Inputs.put.xbox)) {
            if (this.Game.Ball.Owner == this.Controlled)
            {
				if(this.Game.Ball.inZone == this.Team.opponent.Zone) {
					this.Game.OnEssai();
				}			
			}			
		}
	}
}
