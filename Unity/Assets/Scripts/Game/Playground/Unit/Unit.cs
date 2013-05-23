using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using System;

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
	
    public Game game {get; set;}	
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
	
	public enum TYPEOFPLAYER
	{
		DEFENSE,
		OFFENSIVE,
		NONE
	};
	
	public TYPEOFPLAYER typeOfPlayer;
	public bool ballZone = false;
	
	//maxens : c'est très bourrin xD
	void Update() {
		if(team == null)
			return;

        if (remainingTimeDodging > 0)
        {
            remainingTimeDodging -= Time.deltaTime;
            if (remainingTimeDodging <= 0)
            {
                this.game.OnDodgeFinished(this);
            }
        }
        else if (cooldownDodge > 0)
        {
            cooldownDodge -= Time.deltaTime;
        }
				
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
	
	public void UpdateTypeOfPlay()
	{
		if (this.Team.Player.Controlled == this)
		{
			Debug.Log(this);
			this.typeOfPlayer = TYPEOFPLAYER.OFFENSIVE;
		}
		else
		{
			//trouvons les joueurs les plus proche jusqu'à ce que l'on ait 1 + Game.settings.nbOffensivePlayer
			List<Unit> newList = new List<Unit>();
			foreach (Unit u in this.Team)
			{
				if (u != this.Team.Player.Controlled)
					newList.Add(u);
			}

			float dMin = 0f;
			String s;

			for (int i = 0; i < this.game.settings.Global.Team.nbOffensivePlayer; ++i)
			{
				Unit u = this.Team.Player.Controlled.GetNearestAlly(newList, out dMin, true);
				s = "";
				foreach(Unit c in newList)
				{
					s += c + " ";
				}
				//Debug.Log(s);
				if (u != null)
				{
					u.typeOfPlayer = TYPEOFPLAYER.OFFENSIVE;
					this.Team.nbOffensivePlayer++;
					newList.Remove(u);
				}
			}

			foreach (Unit u in newList)
			{
				u.typeOfPlayer = TYPEOFPLAYER.DEFENSE;
			}
		}
		
	}

	public void UpdatePlacement()
	{

		switch (typeOfPlayer)
		{
			case TYPEOFPLAYER.DEFENSE:
				{
					UpdateDefensePlacement();
					break;
				}
			case TYPEOFPLAYER.OFFENSIVE:
				{
					UpdateOffensivePlacement();
					break;
				}
			case TYPEOFPLAYER.NONE:
			default: break;
		}
	}

	void UpdateOffensivePlacement()
	{
		if (this == this.team.Player.Controlled)
		{
			return;
		}

		//Variables
		Vector3 pos = this.transform.position;
		Vector3 oldPos = pos;
		float distZ = Mathf.Abs(this.team.Player.Controlled.transform.position.z - this.transform.position.z); //distance entre moi et le controllé
		float distX = 0f;

		if (this.game.Ball.passManager!= null && this.game.Ball.passManager.oPassState == PassSystem.passState.ONPASS)
		{
			Debug.Log("pass en cours");
		}
	}
	
	void UpdateDefensePlacement()
	{
        //return;

		//Variables
		Vector3 pos = this.transform.position;
		Vector3 oldPos = pos;
		float distZ = Mathf.Abs(this.Team.Player.Controlled.transform.position.z - this.transform.position.z); //distance entre moi et le controllé
		float distX = 0f;

		//Check si je suis dans le même couloir que la balle
		if (this.game.compareZoneInMap(this.game.Ball.gameObject, this.gameObject) == 0)
		{
			//je suis dans la même zone que la balle et donc je ne bouge pas sur X sauf si un autre joueur est déjà dans le couloir
			foreach (Unit u in this.Team)
			{
				if (u != this && u.typeOfPlayer == TYPEOFPLAYER.DEFENSE)
				{
					if (u.ballZone)
					{
						this.ballZone = false;
						break;
					}
					else
						this.ballZone = true;
				}
			}
		}
		else // Check si un autre défenseur est dans le couloir de la balle
		{
			//Si personne alors je décide d'y aller
			bool otherDefenseGo = false;
			foreach (Unit u in this.Team)
			{
				if (u != this && u.typeOfPlayer == TYPEOFPLAYER.DEFENSE)
				{
					if (u.ballZone)
					{
						otherDefenseGo = true;
						break;
					}
				}
			}

			if (!otherDefenseGo)
			{
				this.ballZone = true;
				//Déplacement sur X
				pos.x += this.game.Ball.transform.position.x - this.transform.position.x;
				this.Order = Order.OrderMove(pos);
			}
		}

		if (!this.ballZone)
		{
			//Contrainte sur X
			foreach (Unit u in this.Team)
			{
				//Seuls les défenseurs m'intéressent
				if (u != this && u.typeOfPlayer == TYPEOFPLAYER.DEFENSE)
				{
					distX = Mathf.Abs(u.transform.position.x - this.transform.position.x);
					if (distX > this.game.settings.Global.Team.distanceMaxBetweenDefensePlayer)
					{
						if (u.transform.position.x > this.transform.position.x)
						{
							pos.x = u.transform.position.x - this.game.settings.Global.Team.distanceMaxBetweenDefensePlayer + this.game.rand.Next(5);
						}
						else
						{
							pos.x = u.transform.position.x + this.game.settings.Global.Team.distanceMaxBetweenDefensePlayer - this.game.rand.Next(5);
						}
					}
					else if (distX < this.game.settings.Global.Team.distanceMinBetweenDefensePlayer)
					{
						if (u.transform.position.x > this.transform.position.x)
						{
							pos.x = u.transform.position.x - (this.game.settings.Global.Team.distanceMinBetweenDefensePlayer + this.game.rand.Next(5));
						}
						else
						{
							pos.x = u.transform.position.x + (this.game.settings.Global.Team.distanceMinBetweenDefensePlayer + this.game.rand.Next(5));
						}
					}
					else // cas normal
					{
						if (this.game.rand.Next(100) > 50)
							pos.x += this.game.rand.Next(5);
						else
							pos.x -= this.game.rand.Next(5);
					}
				}
				if (pos.x != oldPos.x)
				{
					this.Order = Order.OrderMove(pos);
					oldPos = pos;
				}
			}
		}
		else
		{
			foreach (Unit u in this.team)
			{
				//seuls les défenseurs différent de moi m'intéresse
				if (u != this && u.typeOfPlayer == TYPEOFPLAYER.DEFENSE)
				{
					// le joueur est à ma droite
					Order.TYPE_POSITION posThis = this.PositionInMap();
					Order.TYPE_POSITION posBall = game.PositionInMap(game.Ball.gameObject);
					//Debug.Log(posThis + " et " + posBall + "donne" + (posThis > posBall));

					//l'autre défenseur est à ma droite
					if (u.PositionInMap() > posThis)
					{
						//si la balle est > milieu alors il devient le défenseur de la balle
						if (posBall > Order.TYPE_POSITION.MIDDLE)
						{
							this.ballZone = false;
							u.ballZone = true;
						}
					}
					else
					{
						//si la balle est < milieu alors il devient le défenseur de la balle
						if (posBall < Order.TYPE_POSITION.MIDDLE)
						{
							this.ballZone = false;
							u.ballZone = true;
						}
					}
				}
			}
		}

		//Contrainte sur Z
		if (distZ > this.game.settings.Global.Team.distanceMaxBetweenOffensiveAndDefensePlayer)
		{
			//Debug.Log("too far : pos controllé : " + this.Team.Player.Controlled.transform.position.z + " autre pos : " + this.transform.position.z);
			if (this.Team.Player.Controlled.transform.position.z > this.transform.position.z)
			{
				pos.z = this.team.Player.Controlled.transform.position.z - 
					(this.game.settings.Global.Team.distanceMaxBetweenOffensiveAndDefensePlayer + (float)this.game.rand.NextDouble());
			}
			else
				pos.z = pos.z = this.team.Player.Controlled.transform.position.z +
					(this.game.settings.Global.Team.distanceMaxBetweenOffensiveAndDefensePlayer + (float)this.game.rand.NextDouble());
		}
		else if (distZ < this.game.settings.Global.Team.distanceMinBetweenOffensiveAndDefensePlayer)
		{
			//Debug.Log("too near : " + this);
			if (this.Team.Player.Controlled.transform.position.z > this.transform.position.z)
				pos.z -= (this.game.settings.Global.Team.distanceMinBetweenOffensiveAndDefensePlayer + (float)this.game.rand.NextDouble()) - distZ;
			else
				pos.z += (this.game.settings.Global.Team.distanceMinBetweenOffensiveAndDefensePlayer + (float)this.game.rand.NextDouble()) - distZ;
		}/*
		else // cas normal
		{
			if (this.game.rand.Next(100) > 50)
				pos.z += (float)this.game.rand.Next(5);
			else
				pos.z -= (float)this.game.rand.Next(5);
		}*/

		if (pos.z != oldPos.z)
		{
			this.Order = Order.OrderMove(pos);
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

    private float remainingTimeDodging = -1;
    private float cooldownDodge = -1;

    public Vector3 dodgeDirection { get; set; }
    public bool Dodge
    {
        get
        {
            return (remainingTimeDodging > 0);
        }
        set
        {
            if (value == false)
            {
                remainingTimeDodging = -1;
            }
            else if(CanDodge)
            {
                remainingTimeDodging = this.team.unitDodgeDuration;
                cooldownDodge = this.team.unitDodgeCooldown;
                this.game.OnDodge(this);
            }
        }
    }

    public bool CanDodge
    {
        get
        {
            if (!game)
                return false;
            if (!game.Ball)
                return false;

            return !Dodge && cooldownDodge < 0 && this == game.Ball.Owner;
        }
    }

	public override void Start () 
    {        
        sm.SetFirstState(new MainUnitState(sm, this));
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
                    this.sm.event_NearUnit(other);// 
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

    public Unit GetNearestAlly(out float dMin, bool square = false)
    {
        Unit nearestAlly = null;
        dMin = -1;
		float d;
		
        foreach (Unit u in this.Team)
        {
            if (u != this)
            {
				if (!square)
                	d = Vector3.Distance(this.transform.position, u.transform.position);
				else
					d = Vector3.SqrMagnitude(this.transform.position - u.transform.position);
                if (nearestAlly == null || d < dMin)
                {
                    nearestAlly = u;
                    dMin = d;
                }
            }
        }

        return nearestAlly;
    }
	
	public Unit GetNearestAlly(List<Unit> l, out float dMin, bool square = false)
    {
        Unit nearestAlly = null;
        dMin = -1;
		float d;
		
        foreach (Unit u in l)
        {
            if (u != this)
            {
				if (!square)
                	d = Vector3.Distance(this.transform.position, u.transform.position);
				else
					d = Vector3.SqrMagnitude(this.transform.position - u.transform.position);
                if (nearestAlly == null || d < dMin)
                {
                    nearestAlly = u;
                    dMin = d;
                }
            }
        }

        return nearestAlly;
    }

    public void ShowPlayer(bool active)
    {
        if (!active)
        {
            this.IndicateSelected(false);
        }
        else
        {
            Gamer g = this.team.Player;
            if (g != null)
            {
                this.IndicateSelected(this == g.Controlled);
            }
        }

        this.Model.SetActive(active);        
    }

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
                
        if (this.CanDodge)
        {
            EditorGUILayout.LabelField("Can Dodge");
        }
        else
        {
            if (this.Dodge)
            {
                EditorGUILayout.LabelField("Dodging : " + (int)this.remainingTimeDodging);
            }
            else
            {
                EditorGUILayout.LabelField("Dodge cooldown : " + (int)this.cooldownDodge);
            }
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
	
	public Order.TYPE_POSITION PositionInMap()
	{

		if (this.transform.position.x >= this.game.xSO && this.transform.position.x < this.game.xSO + this.game.section)
			return Order.TYPE_POSITION.EXTRA_LEFT;
		else if (this.transform.position.x >= this.game.xSO + this.game.section && this.transform.position.x < this.game.xSO + 2*this.game.section)
			return Order.TYPE_POSITION.LEFT;
		else if (this.transform.position.x <= this.game.xNE && this.transform.position.x > this.game.xNE - this.game.section)
			return Order.TYPE_POSITION.EXTRA_RIGHT;
		else if (this.transform.position.x <= this.game.xNE - this.game.section && this.transform.position.x > this.game.xNE - 2*this.game.section)
			return Order.TYPE_POSITION.RIGHT;
		else if (this.transform.position.x >= this.game.xSO + 2 * this.game.section && this.transform.position.x < this.game.xSO + 3*this.game.section)
			return Order.TYPE_POSITION.MIDDLE_LEFT;
		else if (this.transform.position.x <= this.game.xNE - 2 * this.game.section && this.transform.position.x > this.game.xNE - 3*this.game.section)
			return Order.TYPE_POSITION.MIDDLE_RIGHT;
		return Order.TYPE_POSITION.MIDDLE;
	}
}

