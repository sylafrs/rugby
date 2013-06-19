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
public class Gamer
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
	public Game game;
	private Unit unitTo;
	private int passSide;
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

	public Gamer(Team t)
	{
		this.game = t.game;

		canMove = true;

		id = NextGamerId;
		NextGamerId++;
		playerIndex = (PlayerIndex)id;

		game.refs.xboxInputs.NeedToCheck(id);

		if (XboxController == null)
			XboxController = this.game.refs.xboxInputs.controllers[id];

		this.Team = t;
		this.Controlled = t[t.nbUnits / 2];
		this.Controlled.IndicateSelected(true);
		this.Inputs = this.game.settings.Inputs;
	}

	/*
	 * @ author Maxens Dubois
	 */
	public void stopMove()
	{
		canMove = false;
	}

	/*
	 * @ author Maxens Dubois
	 */
	public void enableMove()
	{
		canMove = true;
	}

	/*
	 * @ author Maxens Dubois
	 */
	bool getMoveStatus()
	{
		return canMove;
	}

	//List<Unit> unitsSide;

	public void newFrame()
	{
		this.canTackle = true;
	}

	public void myUpdate()
	{
		if (XboxController == null)
			XboxController = game.refs.xboxInputs.controllers[id];

		if (Inputs == null)
			return;

		if (Controlled)
		{
			UpdateDODGE();

			if (!Controlled.Dodge && !Controlled.isTackled)
			{
				UpdateStickDirection();
				UpdateSUPER();
				UpdateMOVE();
				UpdateTACKLE();
				UpdatePASS();
				UpdateDROP();
				UpdateESSAI();
			}
		}

		UpdatePLAYER();
	}

	public void UpdateSUPER()
	{
		if (this.XboxController != null)
		{
			if (Input.GetKeyDown(game.settings.Inputs.superOff.keyboard(this.Team)) || this.XboxController.GetButtonDown(game.settings.Inputs.superOff.xbox))
			{
				this.Team.Super.launchSuper();
			}
		}
	}

	public bool UpdateRESET()
	{
		if (Input.GetKeyUp(Inputs.reset.keyboardP1) || Input.GetKeyUp(Inputs.reset.keyboardP1) || XboxController.GetButtonUp(Inputs.reset.xbox))
		{
			game.Reset();
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
			stickDirection = Inputs.move.keyboard(this.Team).GetDirection();
		}
	}

	void UpdatePASS()
	{
		if (game.Ball.Owner == Controlled && game.Ball.inZone == null)
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

			if (game.refs.managers.camera.TeamLooked == game.northTeam)
			{
				side *= -1;
			}

			if (Input.GetKeyDown(Inputs.shortPass.keyboard(this.Team)) || XboxController.GetButtonDown(Inputs.shortPass.xbox))
			{
				List<Unit> listToCheck = null;
				if (side > 0)
				{
					listToCheck = Controlled.Team.GetRight(Controlled);
				}
				else if (side < 0)
				{
					listToCheck = Controlled.Team.GetLeft(Controlled);
				}
				if (listToCheck.Count > 0)
				{
					foreach (Unit u in listToCheck)
					{
						if (u.typeOfPlayer == Unit.TYPEOFPLAYER.OFFENSIVE)
						{
							if (Controlled.Team == game.southTeam)
							{
								if (u.transform.position.z < Controlled.transform.position.z && u.canCatchTheBall)
								{
									//Debug.Log("try to pass to " + u);
									unitTo = u;
									passSide = 1;
									break;
								}
								else
									unitTo = null;
							}
							else if (Controlled.Team == game.northTeam)
							{
								if (u.transform.position.z > Controlled.transform.position.z && u.canCatchTheBall)
								{
									//Debug.Log("try to pass to " + u);
									u.canCatchTheBall = true;
									unitTo = u;
									passSide = -1;
									break;
								}
								else
									unitTo = null;
							}
						}
					}
				}
				else
				{
					unitTo = null;
					return;
				}

			}
			else if (Input.GetKeyDown(Inputs.longPass.keyboard(this.Team)) || XboxController.GetButtonDown(Inputs.longPass.xbox))
			{
				List<Unit> listToCheck = null;
				if (side > 0)
				{
					listToCheck = Controlled.Team.GetRight(Controlled);
				}
				else if (side < 0)
				{
					listToCheck = Controlled.Team.GetLeft(Controlled);
				}
				if (listToCheck.Count > 0)
				{
					foreach (Unit u in listToCheck)
					{
						if (u.typeOfPlayer == Unit.TYPEOFPLAYER.DEFENSE)
						{
							if (Controlled.Team == game.southTeam)
							{
								if (u.transform.position.z < Controlled.transform.position.z && u.canCatchTheBall)
								{
									//Debug.Log("try to pass to " + u);
									unitTo = u;
									break;
								}
								else
									unitTo = null;
							}
							else if (Controlled.Team == game.northTeam)
							{
								if (u.transform.position.z > Controlled.transform.position.z && u.canCatchTheBall)
								{
									//Debug.Log("try to pass to " + u);
									u.canCatchTheBall = true;
									unitTo = u;
									break;
								}
								else
									unitTo = null;
							}
						}
					}
				}
				else
				{
					unitTo = null;
					return;
				}
			}

			else if (
				Input.GetKeyUp(Inputs.shortPass.keyboard(this.Team)) ||
				Input.GetKeyUp(Inputs.longPass.keyboard(this.Team)) ||
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
			game.Ball.transform.position = Controlled.BallPlaceHolderRight.transform.position;
			//unitsSide = Controlled.Team.GetRight(Controlled);
		}
		else
		{
			game.Ball.transform.position = Controlled.BallPlaceHolderLeft.transform.position;
			//unitsSide = Controlled.Team.GetLeft(Controlled);
		}

	}

	void UpdatePASS_OnRelease()
	{
		if (Controlled == game.Ball.Owner)
		{
			onActionCapture = false;
			if (timeOnActionCapture > game.settings.GameStates.MainState.PlayingState.MainGameState.PassingState.maxTimeHoldingPassButton)
				timeOnActionCapture = game.settings.GameStates.MainState.PlayingState.MainGameState.PassingState.maxTimeHoldingPassButton;
			//Debug.DrawRay(this.transform.position, passDirection, Color.red);
			/*
            if (unitsSide.Count != 0)
            {
                int unit = Mathf.FloorToInt(unitsSide.Count * timeOnActionCapture / Game.settings.maxTimeHoldingPassButton);
                

                if (unit == unitsSide.Count) unit--;
                Unit u = unitsSide[unit];

                Controlled.Order = Order.OrderPass(u);
                PassDirection = Vector3.zero;
            }*/
			if (unitTo != null && unitTo != game.Ball.Owner)
			{
				if (Controlled.unitAnimator)
				{
					Controlled.unitAnimator.OnPass(passSide > 0);
				}
				Controlled.Order = Order.OrderPass(unitTo);
			}
			//PassDirection = Vector3.zero;
		}
		else
		{
			// Changer de perso.
		}
	}

	bool canTackle = true;

	void UpdateTACKLE()
	{
		Unit owner = this.game.Ball.Owner;

		if (canTackle && owner != null && owner.Team != this.Team && (Input.GetKeyDown(Inputs.tackle.keyboard(this.Team)) || XboxController.GetButtonDown(Inputs.tackle.xbox)))
		{
			canTackle = false;

			Unit tackled = owner;
			if (owner.Dodge && owner.Team.settings.unitInvincibleDodge)
				tackled = null;

            if (!Controlled.RangeUnits.Contains(owner))
                return;

			if (!Controlled.NearUnits.Contains(owner))
				tackled = null;
                //return;

			if (Controlled.unitAnimator)
				Controlled.unitAnimator.OnTackleStart(tackled != null);

            if (tackled && tackled.unitAnimator)            
                tackled.unitAnimator.OnBeingTackled();
            
			Controlled.Order = Order.OrderPlaquer(tackled);
			
			if (
			(this.game.Ball.Owner.Team == this.game.southTeam && this.game.Ball.transform.position.z >= this.game.northTeam.But.transform.position.z )
			||
			(this.game.Ball.Owner.Team == this.game.northTeam && this.game.Ball.transform.position.z <= this.game.southTeam.But.transform.position.z )
			)
			{
				canTackle = false;
				Debug.Log("cant tackle");
			}
			
		}
	}

    public void UpdateControlled()
    {
        this.Controlled.IndicateSelected(false);
        this.Controlled = null;
    }

	void UpdatePLAYER()
	{
		bool change = false;
        
		if (this.Controlled == null)
		{
			change = true;
		}
		//else if (this.Controlled.isTackled)
		//{
		//	change = true;
		//}
		else if (this.Controlled != this.game.Ball.Owner &&
				(Input.GetKeyDown(Inputs.changePlayer.keyboard(this.Team)) || XboxController.GetButtonDown(Inputs.changePlayer.xbox)))
		{
			change = true;
		}

		if (change)
		{
            //Debug.Log("CHANGE PLAYER");
			//foreach (Unit u in this.Team)
			//{
			//	u.UpdateTypeOfPlay();
			//}

            Unit nextControlled = null;
            if (BallSkipOurOffensive())
            {
                if (BallGoOurBut())
                {
                    nextControlled = GetDefensivePlayer();
                }
                else
                {
                    nextControlled = GetUnitNear(false);
                }
            }
            else
            {
                nextControlled = GetUnitNear(false);
            }

			if (Controlled)
			{
				Controlled.Order = Order.OrderNothing();
				Controlled.IndicateSelected(false);
			}
            
			if (nextControlled)
				Controlled = nextControlled;

			//Unit nearFromBall = GetUnitNear(false);
			//if (nearFromBall)
			//	Controlled = nearFromBall;

			if (Controlled)
			{
				Controlled.Order = Order.OrderNothing();
				Controlled.IndicateSelected(true);
			}
		}

		if (Controlled && !game.UseFlorianIA)
		{
			Order.TYPE_POSITION typePosition = Team.PositionInMap(Controlled);

			if (game.Ball.Owner == null || game.Ball.Owner.Team == Team)
			{
				//offensiveside
				foreach (Unit u in Controlled.Team)
				{
					if (u != Controlled)
					{
						u.Order = Order.OrderOffensiveSide(
							Controlled, new Vector3(game.settings.Global.Team.Vheight, 0, game.settings.Global.Team.Vwidth / 1.5f),
							Controlled.Team.south,
							typePosition
						);
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
						u.Order = Order.OrderDefensiveSide(
							Controlled,
							new Vector3(game.settings.Global.Team.Vheight, 0, game.settings.Global.Team.Vwidth / 1.5f),
							Controlled.Team.south,
							typePosition
						);
					}
				}
			}
		}
	}

	public bool BallSkipOurOffensive()
	{
		bool result = false;

		foreach (Unit u in this.Team)
		{
			if (u.typeOfPlayer == Unit.TYPEOFPLAYER.OFFENSIVE)
			{
				if (this.Team == this.game.southTeam)
				{
					if (this.game.Ball.transform.position.z < u.transform.position.z)
						result = true;
				}
				else
				{
					if (this.game.Ball.transform.position.z > u.transform.position.z)
						result = true;
				}
			}
		}
		return result;
	}

	public bool BallGoOurBut()
	{
		if (this.game.Ball.Owner != null)
		{
			if (Vector3.Dot(this.Team.But.transform.position - this.game.Ball.Owner.transform.position, this.game.Ball.Owner.transform.forward) < 0)
				return false;
		}
		else
		{
			if (this.game.Ball.isDroping)
			{
				if (Vector3.Dot(this.Team.But.transform.position - this.game.Ball.transform.position, this.game.Ball.drop.ownerDirection) < 0)
					return false;
			}
		}
		return true;
	}

	public Unit GetDefensivePlayer()
	{
		foreach (Unit u in this.Team)
		{
			if (u.typeOfPlayer == Unit.TYPEOFPLAYER.DEFENSE)
			{
				if (u.ballZone)
					return u;
			}
		}
		return null;
	}

	public Unit GetUnitNear(bool countControlled)
	{
		float dist;         //< Distance
		float min = 0;      //< Distance minimum trouvée
		Unit near = null;   //< Unité la plus proche trouvée

		// Pour chaque unité de notre équipe
		foreach (Unit u in this.Team)
		{

			// On récupère la distance unité <-> balle (au carré, pour économiser une racine)
			dist = Vector3.SqrMagnitude(game.Ball.transform.position - u.transform.position);

			// Si :
			// - Aucune unité matchée ou distance plus petite que les précédentes
			// - Unité pas plaquée
			// - On peut prendre le controllé ou il ne s'agit pas du contrôlé
			if ((near == null || dist < min) && !u.isTackled && (countControlled || u != Controlled))
			{
				near = u;   // On change le résultat
				min = dist; // La distance minimum est mise à jour
			}
		}

		// On retourne l'unité
		return near; // Peut être null si aucune trouvée
	}

	void UpdateMOVE()
	{
		if (!canMove) return;
		Vector3 direction = Vector3.zero;
		InputDirection.Direction d;

		direction = Vector3.zero;
		if (XboxController.IsConnected)
		{
			d = XboxController.GetDirection(Inputs.move.xbox);
		}
		else
		{
			d = Inputs.move.keyboard(this.Team).GetDirection();
		}

		direction += Camera.main.transform.forward * d.y;
		direction += Camera.main.transform.right * d.x;

        if (direction.magnitude > 0.8f)
		{            
			Controlled.Order = Order.OrderMove(Controlled.transform.position + direction.normalized);
		}
	}

	void UpdateDODGE()
	{

		if (!canMove) return;
		if (!Controlled) return;
		if (!Controlled.CanDodge) return;

		Vector3 direction = Vector3.zero;
		InputDirection.Direction d;

		direction = Vector3.zero;
		if (XboxController.IsConnected)
		{
			d = XboxController.GetDirection(Inputs.dodge.xbox);
		}
		else
		{
			d = Inputs.dodge.keyboard(Team).GetDirection();
		}

		//  if (d.y < 0)
		//  {
		//      direction += Camera.main.transform.forward * d.y;
		//  }

		direction += Camera.main.transform.right * d.x;

		if (direction.magnitude > 0.8f)
		{
			Controlled.Order = Order.OrderDodge(direction.normalized);
		}
	}

	void UpdateDROP()
	{
		if (Input.GetKeyDown(Inputs.dropUpAndUnder.keyboard(this.Team)) || XboxController.GetButtonDown(Inputs.dropUpAndUnder.xbox))
		{
			Controlled.Order = Order.OrderDropUpAndUnder(game.northTeam[0]);
		}
		else if (Input.GetKeyDown(Inputs.dropKick.keyboard(this.Team)) || XboxController.GetButtonDown(Inputs.dropKick.xbox))
		{
			Controlled.Order = Order.OrderDropKick(game.northTeam[0]);
		}
	}

	void UpdateESSAI()
	{
		if (Input.GetKeyDown(Inputs.put.keyboard(this.Team)) || XboxController.GetButtonDown(Inputs.put.xbox))
		{
			if (this.game.Ball.Owner == this.Controlled)
			{
				Zone z = this.game.Ball.inZone;
				//if(z != null)Debug.Log("La balle est dans la zone "+z.name);
				if (
					(this.game.Ball.Owner.Team == this.game.southTeam && this.game.Ball.transform.position.z >= this.game.northTeam.But.transform.position.z )
					||
					(this.game.Ball.Owner.Team == this.game.northTeam && this.game.Ball.transform.position.z <= this.game.southTeam.But.transform.position.z )
					)
				{
					if (this.Controlled.unitAnimator)
					{
						this.Controlled.unitAnimator.OnPut();
					}
					this.game.OnTry(this.game.Ball.Owner.Team.opponent.Zone);
				}
			}
		}
	}
}
