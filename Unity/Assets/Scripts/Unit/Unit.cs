using UnityEngine;
using System.Collections;

[System.Serializable, AddComponentMenu("Scripts/Models/Unit"), RequireComponent(typeof(NavMeshAgent))]
public class Unit : TriggeringTriggered
{
    public StateMachine sm;
    public GameObject BallPlaceHolder; 
    private NavMeshAgent nma;
    private Order currentOrder;
    private Team team;

    public Team Team {
        get
        {
            return GetTeam();
        }
        set {
            if (team == null) team = value;
        }
    }

    public Order Order
    {
        get
        {
            return GetOrder();    
        }
        set
        {
            ChangeOrder(value);
        }
    }

	public override void Start () 
    {
        nma = this.GetComponent<NavMeshAgent>();
        sm.SetFirstState(new MainState(sm, this));
        base.Start();
	}

    public Team GetTeam()
    {
        return team;
    }

    public NavMeshAgent GetNMA()
    {
        return nma;
    }

    public void ChangeOrder(Order o)
    {
        ChangeOrderSilency(o);
        sm.event_neworder();
    }

    public void ChangeOrderSilency(Order o)
    {
        this.currentOrder = o;
    }

    public Order GetOrder()
    {
        return currentOrder;
    }
}

