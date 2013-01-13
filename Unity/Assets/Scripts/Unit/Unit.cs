using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

/**
 * @class Unit
 * @brief Une unité
 * @author Sylvain Lafon
 */
[System.Serializable, AddComponentMenu("Scripts/Models/Unit"), RequireComponent(typeof(NavMeshAgent))]
public class Unit : TriggeringTriggered, Debugable
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

    public override void Inside(Triggered o, Trigger t)
    {
        Unit other = o.GetComponent<Unit>();
        if (other != null)
        {
            if (t.GetType() == typeof(NearUnit))
            {
                if (other.Team != this.Team)
                    ;// Debug.Log(this.name + " : " + other.name + " est dans mon champs d'action !");
            }
        }
    }

    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        Order o = this.GetOrder();
        EditorGUILayout.LabelField("Ordre : " + o.type.ToString());
        switch (o.type)
        {
            case Order.TYPE.DEPLACER:
                EditorGUILayout.LabelField("Point : " + o.point.ToString());
                break;

            case Order.TYPE.SUIVRE:
                EditorGUILayout.LabelField("Cible : " + o.target.name);               
                break;
        }        
#endif
    }
}

