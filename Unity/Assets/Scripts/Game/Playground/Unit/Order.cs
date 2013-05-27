using UnityEngine;
using System.Collections;

/**
 * @class Order
 * @brief Un ordre (unite)
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[System.Serializable]
public struct Order  {
    public enum TYPE
    {
        NOTHING,
        MOVE,
        PASS,
        DROPKICK,
		DROPUPANDUNDER,
        TACKLE,
        FOLLOW,
        TRIANGLE,   // ASSISTER
		DEFENSIVE_SIDE,
        LANE,      // PRESSER
        SEARCH,
        DODGE,

        LENGTH
    }
  
	public enum TYPE_POSITION
	{
		EXTRA_LEFT,
		LEFT,
		MIDDLE_LEFT,
		MIDDLE,
		MIDDLE_RIGHT,
		RIGHT,
		EXTRA_RIGHT
	}

    public TYPE type;
	public TYPE_POSITION position;
	public float pressionCapture;
    public bool passR;
    public Unit target;
    public Transform targetT;
    public float power;
    public Vector3 point;

    public static Order OrderNothing() {
        Order o = new Order();
        o.type = TYPE.NOTHING;
        return o;
    }

    public static Order OrderDodge(Vector3 direction)
    {
        Order o = new Order();
        o.type = TYPE.DODGE;
        o.point = direction;
        return o;
    }

    public static Order OrderMove(Vector3 point)
    {
        Order o = new Order();
        o.type = TYPE.MOVE;
        o.point = point;
        return o;
    }

    public static Order OrderFollow(Transform unit)
    {
        Order o = new Order();
        o.type = TYPE.FOLLOW;
        o.targetT = unit;
        return o;
    }
	
	public static Order OrderDropKick(Unit unit)
    {
		return OrderDropKick(unit, 1.0f);
	}

    public static Order OrderDropKick(Unit unit, float pressionCapture)
    {
        Order o = new Order();
        o.type = TYPE.DROPKICK;
        o.target = unit;
		o.pressionCapture = pressionCapture;
        return o;
    }

	public static Order OrderDropUpAndUnder(Unit unit)
	{
		return OrderDropUpAndUnder(unit, 1.0f);
	}

	public static Order OrderDropUpAndUnder(Unit unit, float pressionCapture)
	{
		Order o = new Order();
		o.type = TYPE.DROPUPANDUNDER;
		o.target = unit;
		o.pressionCapture = pressionCapture;
		return o;
	}

	public static Order OrderPass(Unit unit, bool right)
	{
		Order o = new Order();
		o.type = TYPE.PASS;
		o.target = unit;
        o.passR = right;
		return o;
	}

    public static Order OrderSupport(Unit unit, Vector3 distance, bool right)
    {
        Order o = new Order();
        o.type = TYPE.TRIANGLE;
        o.target = unit;
        o.point = new Vector3(distance.x, 0, distance.z * (right ? -1 : 1));
        return o;
    }

	public static Order OrderDefensiveSide(Unit unit, Vector3 distance, bool right, TYPE_POSITION type)
	{
		Order o = new Order();
		o.type = TYPE.DEFENSIVE_SIDE;
		o.target = unit;
		o.position = type;
		o.point = new Vector3(distance.x, 0, distance.z * (right ? -1 : 1));
		return o;
	}
	
	public static Order OrderOffensiveSide(Unit unit, Vector3 distance, bool right, TYPE_POSITION type)
	{
		Order o = new Order();
		o.type = TYPE.DEFENSIVE_SIDE;
		o.target = unit;
		o.position = type;
		o.point = new Vector3(distance.x, 0, distance.z * (right ? -1 : 1));
		return o;
	}

    public static Order OrderAttack(Unit unit, float distance, bool right)
    {
        Order o = new Order();
        o.type = TYPE.LANE;
        o.target = unit;
        o.power = distance;
        return o;
    }

    public static Order OrderFollowBall()
    {
        Order o = new Order();
        o.type = TYPE.SEARCH;
        return o;
    }

    public static Order OrderPlaquer(Unit u)
    {          
        Order o = new Order();
        o.type = TYPE.TACKLE;        
        o.target = u;
        
        return o;
    }
}
