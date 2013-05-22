using UnityEngine;
using System.Collections;

/*
 * @class DefensiveSide
 * @brief Etat se place en mode de support défensif selon la position du porteur sur le terrain :
 * @author Florian Guilleminot
 */
public class DefensiveSide : UnitState
{
    public DefensiveSide(StateMachine sm, Unit unit) : base(sm, unit) { }

    Order o;

    public override void OnEnter()
    {
        o = unit.Order;
    }

    public override void OnUpdate()
	{
		Order.TYPE_POSITION typePosition = o.target.Team.PositionInMap( o.target );
		switch ( typePosition )
		{
			case Order.TYPE_POSITION.EXTRA_LEFT:
			{
				PositionExtraLeftSide();
				break;
			}
			case Order.TYPE_POSITION.LEFT:
			{
				PositionLeftSide();
				break;
			}
			case Order.TYPE_POSITION.RIGHT:
			{
				PositionRightSide();
				break;
			}
			case Order.TYPE_POSITION.EXTRA_RIGHT:
			{
				PositionExtraRightSide();
				break;
			}
			case Order.TYPE_POSITION.MIDDLE_LEFT:
			case Order.TYPE_POSITION.MIDDLE_RIGHT:
			case Order.TYPE_POSITION.MIDDLE:
			{
				PositionMiddleSide();
				break;
			}
			default: break;
		}
	}

	public void PositionExtraLeftSide()
	{
		Vector3 tPos = o.target.transform.position;

		int dif = unit.Team.GetLineNumber(unit, o.target);
		float x;
		float z;

		x = o.point.x * Mathf.Abs(dif);
		z = o.point.z * Mathf.Abs(dif);


		unit.nma.stoppingDistance = 0;
		unit.nma.SetDestination(new Vector3(tPos.x + x, 0, tPos.z + z));
	}

	public void PositionLeftSide()
	{
		Vector3 tPos = o.target.transform.position;

		int dif = unit.Team.GetLineNumber(unit, o.target);
		float x;
		float z;
		
		if (o.point.z < 0)
		{
			x = o.point.x * dif;
			z = o.point.z * Mathf.Abs(dif);
		}
		else
		{
			if (dif <= -2)
			{
				x = o.point.x * Mathf.Abs(dif);
				z = o.point.z * dif;
			}
			else
			{
				x = o.point.x * dif;
				z = o.point.z * Mathf.Abs(dif);
			}
		}

		unit.nma.stoppingDistance = 0;
		unit.nma.SetDestination(new Vector3(tPos.x + x, 0, tPos.z + z));
	}
	
	public void PositionExtraRightSide()
	{
		Vector3 tPos = o.target.transform.position;

		int dif = unit.Team.GetLineNumber(unit, o.target);
		float x;
		float z;

		x = o.point.x * Mathf.Abs(dif);
		z = o.point.z * Mathf.Abs(dif);

		unit.nma.stoppingDistance = 0;
		//
		unit.nma.SetDestination(new Vector3(tPos.x - x, 0, tPos.z + z));
	}

	public void PositionRightSide()
	{
		Vector3 tPos = o.target.transform.position;

		int dif = unit.Team.GetLineNumber(unit, o.target);
		float x;
		float z;

		//right side
		if (o.point.z > 0)
		{
			x = o.point.x * dif;
			z = o.point.z * Mathf.Abs(dif);
		}
		else
		{
			if (dif >= 2)
			{
				x = o.point.x * Mathf.Abs(dif);
				z = o.point.z * dif;
			}
			else
			{
				x = o.point.x * dif;
				z = o.point.z * Mathf.Abs(dif);
			}
		}

		unit.nma.stoppingDistance = 0;
		unit.nma.SetDestination(new Vector3(tPos.x + x, 0, tPos.z + z));
	}

	public void PositionMiddleSide()
	{
		Vector3 tPos = o.target.transform.position;

		int dif = unit.Team.GetLineNumber(unit, o.target);
		float x = o.point.x * dif;
		float z = o.point.z * Mathf.Abs(dif);

		unit.nma.stoppingDistance = 0;
		unit.nma.SetDestination(new Vector3(tPos.x + x, 0, tPos.z + z));
	}

}

