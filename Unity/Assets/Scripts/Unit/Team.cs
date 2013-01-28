using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * @class Team
 * @brief Une equipe (unite)
 * @author Sylvain Lafon
 */
[System.Serializable, AddComponentMenu("Scripts/Models/Team")]
public class Team : MonoBehaviour {

    public Team opponent;

    public Game Game;
    public Color Color;
    public string Name;
    public bool right;

    public But But;
    public Zone Zone;

    public int nbPoints = 0;

    private Unit [] units;

    public Unit this[int index]
    {
        get
        {
            if (index < 0 || index >= units.Length)
                return null;

            return units[index];
        }

        private set
        {

        }
    }

    public GameObject Prefab_model;
    
    public int nbUnits;

    public void Start()
    {
        But.Owner = this;
        Zone.Owner = this;
    }

    public void CreateUnits()
    {
        units = new Unit[nbUnits];
        for (int i = 0; i < nbUnits; i++)
        {
            Vector3 pos = this.transform.position + new Vector3((i - (nbUnits / 2.0f)) * 2, 0, 0);

            GameObject o = GameObject.Instantiate(Prefab_model, pos, Quaternion.identity) as GameObject;
            units[i] = o.GetComponent<Unit>();
            units[i].name = Name + " " + (i+1).ToString("D2");
            units[i].transform.parent = this.transform;
            units[i].Team = this;
            units[i].Game = Game;
            //units[i].renderer.material.color = Color;           
        }
    }

    public void initPos()
    {
        for (int i = 0; i < nbUnits; i++)
        {
            Vector3 pos = this.transform.position + new Vector3((i - (nbUnits / 2.0f)) * 2, 0, 0);
            units[i].transform.position = pos;     
        }
    }

    public bool Contains(Unit unit)
    {
        bool trouve = false;
        int i = 0;

        while (!trouve && i < nbUnits)
        {
            trouve = (units[i++] == unit);
        }

        return trouve;
    }

    public void OwnerChanged()
    {
        if (Game.disableIA)
        {
            return;
        }

        if (Game.Ball.Owner == null)
        {
            if (Game.Ball.PreviousOwner.Team != this)
            {
                OwnerChangedBallFree();
            }
            else if (Game.Ball.PreviousOwner.Team == Game.right)
            {
                OwnerChangedOurs();
            }
            else
            {
                OwnerChangedBallFree();
            }

        }
        else if (Game.Ball.Owner.Team == this)
        {
            OwnerChangedOurs();
        }
        else
        {
            OwnerChangedOpponents();
        }
    }

    void OwnerChangedBallFree()
    {
        foreach (Unit u in units)
        {
            if (u != Game.p1.controlled && (Game.p2 == null || u != Game.p2.controlled))
            {
                u.Order = Order.OrderFollowBall();
            }
        }
    }

    public static int sortLeft(Unit a, Unit b)
    {
        if (a.transform.position.x > b.transform.position.x)
        {
            return -1;
        }

        return 1;
    }

    public static int sortRight(Unit a, Unit b)
    {
        if (a.transform.position.x > b.transform.position.x)
        {
            return 1;
        }

        return -1;
    }

    public List<Unit> GetRight(Unit unit)
    {
        List<Unit> right = new List<Unit>();
        foreach (Unit u in units)
        {
            if (u != unit)
            {
                if ((u.transform.position.x - unit.transform.position.x) > 0)
                {
                    right.Add(u);
                }
            }
        }

        right.Sort(sortRight);
        return right;
    }

    public List<Unit> GetLeft(Unit unit)
    {
        List<Unit> left = new List<Unit>();
        foreach (Unit u in units)
        {
            if (u != unit)
            {
                if ((u.transform.position.x - unit.transform.position.x) <= 0)
                {
                    left.Add(u);
                }
            }
        }

        left.Sort(sortLeft);
        return left;
    }

    public int GetLineNumber(Unit unit, Unit target)
    {
        if (unit == target) return 0;

        List<Unit> right = GetRight(target);
        List<Unit> left = GetLeft(target);

        int i;

        if (left.Contains(unit))
        {
            i = -left.IndexOf(unit) - 1;
        }
        else
        {
            i = right.IndexOf(unit) + 1;
        }

        return i;
    }

    void OwnerChangedOurs()
    {
        Unit owner = Game.Ball.Owner;
        if (owner == null)
        {
            owner = Game.Ball.PreviousOwner;
        }

        if (owner != Game.p1.controlled && (Game.p2 == null || owner != Game.p2.controlled))
        {
            Zone z = Game.opponent(this).Zone;
            owner.Order = Order.OrderMove(new Vector3(owner.transform.position.x, 0, z.transform.position.z), Order.TYPE_DEPLACEMENT.SPRINT);
        }
        else
        {
            owner.Order = Order.OrderNothing();
        }
       
        foreach (Unit u in units)
        {
            // FIX. (TODO)
            if (u.Order.type != Order.TYPE.CHERCHER)
            {
                if (u != owner)
                {
                    u.Order = Order.OrderSupport(owner, new Vector3(Game.settings.Vheight, 0, Game.settings.Vwidth), right);
                }
            }            
        } 
    }

    void OwnerChangedOpponents()
    {
        Unit a;
        if (Game.p1.controlled != null && Game.p1.controlled.Team == this)
        {
            a = Game.p1.controlled;
        }
        else if (Game.p2 != null && Game.p2.controlled.Team == this)
        {
            a = Game.p2.controlled;
        }
        else
        {
            a = units[0];
            float d = Vector3.Distance(a.transform.position, Game.Ball.Owner.transform.position);
            for (int i = 0; i < units.Length; i++)
            {
                float d2 = Vector3.Distance(units[i].transform.position, Game.Ball.Owner.transform.position);
                if (d > d2)
                {
                    a = units[i];
                    d = d2;
                }
            }

            a.Order = Order.OrderFollow(Game.Ball.Owner, Order.TYPE_DEPLACEMENT.COURSE);
        } 

        foreach (Unit u in units)
        {
            if (u != a)
            {
                u.Order = Order.OrderAttack(a, Game.settings.LineSpace, right);
            }
        }
    }
}
