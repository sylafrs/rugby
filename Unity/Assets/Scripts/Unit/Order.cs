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
        ATTAQUER,
        PRESSER,
        ASSISTER
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
}
