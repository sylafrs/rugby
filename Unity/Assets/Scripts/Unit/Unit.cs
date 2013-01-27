using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

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
    public GameObject[] selectedIndicators;
    public GameObject[] plaqueIndicators;

    public void IndicateSelected(bool enabled)
    {
        for (int i = 0; i < selectedIndicators.Length; i++)
        {
            selectedIndicators[i].renderer.enabled = enabled;
        }
    }

    public Team Team
    {
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

    public List<Unit> NearUnits;

    public override void Entered(Triggered o, Trigger t)
    {
        Unit other = o.GetComponent<Unit>();
        if (other != null)
        {
            if (t.GetType() == typeof(NearUnit))
            {
                if (other.Team != this.Team)
                {
                    if (!NearUnits.Contains(other))
                    {
                        NearUnits.Add(other);
                    }
                }
            }
        }
    }

    public override void Left(Triggered o, Trigger t)
    {
        Unit other = o.GetComponent<Unit>();
        if (other != null)
        {
            if (NearUnits.Contains(other))
            {
                NearUnits.Remove(other);
            }
        }
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

    public Unit ClosestAlly()
    {
        if (Team.nbUnits < 2)
            return null;

        int i = 0;
        Unit u = Team[i];
        if(u == this) {
            i++;
            u = Team[i];
        }

        float minDist = Vector3.Distance(this.transform.position, u.transform.position);
        while(i < Team.nbUnits)
        {
            if (Team[i] != this)
            {
                float thisDist = Vector3.Distance(this.transform.position, Team[i].transform.position);
                if(thisDist < minDist) {
                    minDist = thisDist;
                    u = Team[i];
                }              
            }

            i++;
        }
        
        return u;
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

