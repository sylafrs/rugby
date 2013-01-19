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
    public Game Game;
   
    public Team Team {
        get
        {
            return team;
        }
        set {
            if (team == null) team = value;
        }
    }

    public Order Order
    {
        get
        {
            return currentOrder;    
        }
        set
        {
            ChangeOrderSilency(value);
            sm.event_neworder();
        }
    }

	public override void Start () 
    {
        nma = this.GetComponent<NavMeshAgent>();
        sm.SetFirstState(new MainState(sm, this));
        base.Start();
	}

    public NavMeshAgent GetNMA()
    {
        return nma;
    }
    
    public void ChangeOrderSilency(Order o)
    {
        this.currentOrder = o;
    }
    
    public override void Inside(Triggered o, Trigger t)
    {
        Unit other = o.GetComponent<Unit>();
        if (other != null)
        {
            if (t.GetType() == typeof(NearUnit))
            {
                if (other.Team != this.Team)  
                    this.sm.event_NearUnit(other);// Debug.Log(this.name + " : " + other.name + " est dans mon champs d'action !");
            }
        }
    }

    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        Order o = this.Order;
        EditorGUILayout.LabelField("Ordre : " + o.type.ToString());
        switch (o.type)
        {
            case Order.TYPE.DEPLACER:
                EditorGUILayout.LabelField("Point : " + o.point.ToString());
                break;

            case Order.TYPE.SUIVRE:
                EditorGUILayout.LabelField("Cible : " + o.target.name);               
                break;

            case Order.TYPE.LIGNE:
                EditorGUILayout.LabelField("Repere : " + o.target.name);
                EditorGUILayout.LabelField("Espace : " + o.power);
                break;

            case Order.TYPE.TRIANGLE:
                EditorGUILayout.LabelField("Sommet : " + o.target.name);
                EditorGUILayout.LabelField("Espace : " + o.point.ToString());
                break;

            case Order.TYPE.CHERCHER:
                EditorGUILayout.LabelField("Cours sur la balle");
                break;

            case Order.TYPE.PLAQUER:
                EditorGUILayout.LabelField("Plaque " + o.target.name);
                break;

        }        
#endif
    }
}

