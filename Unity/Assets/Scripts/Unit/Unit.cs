using UnityEngine;
using System.Collections;

[System.Serializable, AddComponentMenu("Scripts/Models/Unit"), RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour {

    public StateMachine sm;
    private NavMeshAgent nma;
    private Order currentOrder;

	void Start () 
    {
        nma = this.GetComponent<NavMeshAgent>();
        sm.SetFirstState(new MainState(sm, this));
	}

    void Update()
    {

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
