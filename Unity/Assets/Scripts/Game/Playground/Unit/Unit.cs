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
	public GameObject Model;
    
    public GameObject BallPlaceHolderRight;
    public GameObject BallPlaceHolderLeft;
	public GameObject BallPlaceHolderTransformation;
	public GameObject BallPlaceHolderDrop;
	
	public TextureCollectionner buttonIndicator;

    private NavMeshAgent _nma;
    public NavMeshAgent nma
    {
        get
        {
            if (_nma == null)
            {
                _nma = this.GetComponent<NavMeshAgent>();
            }

            return _nma;
        }
    }
    private Order currentOrder;
    private Team team;
	
    public Game Game {get; set;}	
    public GameObject[] selectedIndicators;

    public bool isTackled { get; set; }
		
	public Unit() {
		NearUnits = new List<Unit>();	
	}
	
	//particles sytems
	public ParticleSystem superDashParticles;
	public ParticleSystem superTackleParticles;
	
	public NearUnit triggerTackle {get; set;}

  	public bool canCatchTheBall = true;
	private float timeNoCatch = 0f;
    	
	//maxens : c'est très bourrin xD
	void Update() {
		if(team == null)
			return;
				
		if(triggerTackle)
			triggerTackle.collider.radius = team.unitTackleRange * team.tackleFactor;


		if (!canCatchTheBall)
		{
			if (timeNoCatch < 2f)
			{
				timeNoCatch += Time.deltaTime;
			}
			else
			{
				canCatchTheBall = true;
				timeNoCatch = 0f;
			}
		}
		
	}

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
        sm.SetFirstState(new MainState(sm, this));
        base.Start();
	}
    
    public void ChangeOrderSilency(Order o)
    {
        this.currentOrder = o;
    }

    public List<Unit> NearUnits {get; set;}

    public int getNearAlliesNumber()
    {
        int res = 0;
        foreach (var u in NearUnits)
        {
            if (u.Team == this.Team)
                res++;
        }
        return res;
    }

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
                    this.sm.event_NearUnit(other);// MyDebug.Log(this.name + " : " + other.name + " est dans mon champs d'action !");
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

    public Unit GetNearestAlly()
    {
        float d;
        return GetNearestAlly(out d);
    }

    public Unit GetNearestAlly(out float dMin)
    {
        Unit nearestAlly = null;
        dMin = -1;

        foreach (Unit u in this.Team)
        {
            if (u != this)
            {
                float d = Vector3.Distance(this.transform.position, u.transform.position);
                if (nearestAlly == null || d < dMin)
                {
                    nearestAlly = u;
                    dMin = d;
                }
            }
        }

        return nearestAlly;
    }

/*
    public void ShowTouch(InputTouch touch)
    {

    }
*/
    public void ForDebugWindow()
    {
#if UNITY_EDITOR
        Order o = this.Order;
        EditorGUILayout.LabelField("Ordre : " + o.type.ToString());
        switch (o.type)
        {
            case Order.TYPE.MOVE:
                EditorGUILayout.LabelField("Point : " + o.point.ToString());
                break;

            case Order.TYPE.FOLLOW:
                EditorGUILayout.LabelField("Cible : " + o.target.name);               
                break;

            case Order.TYPE.LANE:
                EditorGUILayout.LabelField("Repere : " + o.target.name);
                EditorGUILayout.LabelField("Espace : " + o.power);
                break;

            case Order.TYPE.TRIANGLE:
                EditorGUILayout.LabelField("Sommet : " + o.target.name);
                EditorGUILayout.LabelField("Espace : " + o.point.ToString());
                break;

            case Order.TYPE.SEARCH:
                EditorGUILayout.LabelField("Cours sur la balle");
                break;

            case Order.TYPE.TACKLE:
                EditorGUILayout.LabelField("Plaque " + o.target.name);
                break;

        }        
#endif
    }
	
	public bool ButtonVisible {
		get {
			return this.buttonIndicator.target.renderer.enabled;
		}
		set {
			this.buttonIndicator.target.renderer.enabled = value;
		}		
	}
	
	public string CurrentButton {
		get {
			return this.buttonIndicator.target.renderer.material.mainTexture.name;	
		}
		set {
			this.buttonIndicator.ApplyTexture(value);	
		}
	}
	
	public void HideButton() {
		ButtonVisible = false;
	}
	
	public void ShowButton(string str) {
		CurrentButton = str;
		ButtonVisible = true;
	}
}

