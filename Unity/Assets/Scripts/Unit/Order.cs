using UnityEngine;
using System.Collections;

/**
 * @class Order
 * @brief Un ordre (unite)
 * @author Sylvain Lafon
 */
[System.Serializable]
public struct Order  {
    public enum TYPE
    {
        RIEN,
        DEPLACER,
        PASSER,
        SHOOTER,
        PLAQUER,
        SUIVRE,
        TRIANGLE,   // ASSISTER
        LIGNE,      // PRESSER
        CHERCHER
    }
    public enum TYPE_DEPLACEMENT
    {
        MARCHE,
        COURSE,
        SPRINT
    }

    public TYPE type;
    public TYPE_DEPLACEMENT deplacement;
    public Unit target;
    public float power;
    public Vector3 point;

    public static Order OrderNothing() {
        Order o = new Order();
        o.type = TYPE.RIEN;
        return o;
    }

    public static Order OrderMove(Vector3 point, TYPE_DEPLACEMENT type)
    {
        Order o = new Order();
        o.type = TYPE.DEPLACER;
        o.point = point;
        o.deplacement = type;
        return o;
    }

    public static Order OrderFollow(Unit unit, TYPE_DEPLACEMENT type)
    {
        Order o = new Order();
        o.type = TYPE.SUIVRE;
        o.target = unit;
        o.deplacement = type;
        return o;
    }

    public static Order OrderGiveBall(Unit unit)
    {
        Order o = new Order();
        o.type = TYPE.PASSER;
        o.target = unit;
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

    public static Order OrderAttack(Unit unit, float distance, bool right)
    {
        Order o = new Order();
        o.type = TYPE.LIGNE;
        o.target = unit;
        o.power = distance;
        return o;
    }

    public static Order OrderFollowBall()
    {
        Order o = new Order();
        o.type = TYPE.CHERCHER;
        return o;
    }

    public static Order OrderPlaquer(Unit u)
    {
        Order o = new Order();
        o.type = TYPE.PLAQUER;
        o.target = u;
        return o;
    }
}
