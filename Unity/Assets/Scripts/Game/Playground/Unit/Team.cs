using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * @class Team
 * @brief Une equipe (unite)
 * @author Sylvain Lafon
 */
[System.Serializable, AddComponentMenu("Scripts/Models/Team")]
public class Team : myMonoBehaviour, IEnumerable {

    public Team opponent {get; set;}
    public Gamer Player {get; set;}
    public Game Game {get; set;}
	public float AngleOfFovTackleCrit = 0.0f;
	public Color ConeTackle = new Color(1f, 1f, 1f, 0.33f);
	public Color DiscTackle = new Color(0f, 0f, 1f, 0.33f);
    public Color Color;
	public Color PlaqueColor;
    public string Name;
    public bool right;
	
	public bool useColors = false;
	
    public But But;
    public Zone Zone;
	
	private bool _fixUnits = false;
	public bool fixUnits {
		get {
			return _fixUnits;
		}
		set {
			_fixUnits = value;
			setSpeed();	
		}		
	}

    public int nbPoints = 0;
	public Transform StartPlacement;

    private Unit [] units;
	
	public float speedFactor;
	public float tackleFactor;
		
	public float unitSpeed;
	public float handicapSpeed = 1;
	public float unitTackleRange;
	
    public Unit this[int index]
    {
        get
        {
			if (units == null || index < 0 || index >= units.Length)
                return null;

            return units[index];
        }

        private set
        {

        }
    }
    public GameObject Prefab_model;
	
	//maxens dubois
	public int SuperGaugeValue;
    
    public int nbUnits;

    public void Start()
    {
        But.Owner = this;
        Zone.Owner = this;
		
		speedFactor  = 1f;
		tackleFactor = 1f;
    }
	
	public void setSpeed() {
		foreach(var u in units) {
            if (u.nma)
            {
                //Debug.Log("a " + u.nma.speed);
                u.nma.speed = fixUnits ? 0 : unitSpeed * speedFactor;
                //Debug.Log("b " + u.nma.speed);
                u.nma.acceleration = (u.nma.speed == 0) ? 10000 : 100; // Valeur "à l'arrache" TODO
            }
            else
            {
                Debug.Log("WAT ?");
            }
		}
	}
	
	public void setHandicapSpeed() {
		foreach(var u in units) {
            if (u.nma)
            {
                //Debug.Log("a " + u.nma.speed);
                u.nma.speed = fixUnits ? 0 : (unitSpeed - handicapSpeed) * speedFactor;
                //Debug.Log("b " + u.nma.speed);
                u.nma.acceleration = (u.nma.speed == 0) ? 10000 : 100; // Valeur "à l'arrache" TODO
            }
            else
            {
                Debug.Log("WAT ?");
            }
		}
	}
	
	//maxens dubois
	public void increaseSuperGauge(int value){
		if((SuperGaugeValue += value) > Game.settings.super.superGaugeMaximum) SuperGaugeValue = Game.settings.super.superGaugeMaximum;
	}
	
    public void CreateUnits()
    {
        units = new Unit[nbUnits];
        for (int i = 0; i < nbUnits; i++)
        {
            GameObject o = GameObject.Instantiate(Prefab_model) as GameObject;
            units[i] = o.GetComponent<Unit>();			
            units[i].Game = Game;
            units[i].name = Name + " " + (i+1).ToString("D2");
            units[i].transform.parent = this.transform;
            units[i].Team = this;
            //units[i].renderer.material.color = Color;           
        }

		initPos();
		setSpeed();
    }

    public void initPos()
    {
		/*
        for (int i = 0; i < nbUnits; i++)
        {
            Vector3 pos = this.transform.position + new Vector3((i - (nbUnits / 2.0f)) * 2, 0, 0);
            units[i].transform.position = pos;     
        }*/
		this.placeUnits(StartPlacement);
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

    public void OnOwnerChanged()
    {
        if (Game.disableIA)
        {
            return;
        }

        if (Game.Ball.Owner == null)
        {
            if (Game.Ball.PreviousOwner.Team != this && Game.Ball.NextOwner != null && Game.Ball.NextOwner.Team != this)
            {
				setSpeed();
                OwnerChangedBallFree();
            }
            else if (Game.Ball.PreviousOwner.Team == Game.right)
            {
				setSpeed();
                OwnerChangedOurs();
            }
			else if (Game.Ball.NextOwner != null && Game.Ball.NextOwner.Team != this)
            {
                OwnerChangedOurs();
            }

        }
        else if (Game.Ball.Owner.Team == this)
        {
			setHandicapSpeed();
            OwnerChangedOurs();
        }
        else
        {
			setSpeed();
            OwnerChangedOpponents();
        }
    }

    void OwnerChangedBallFree()
    {
        foreach (Unit u in units)
        {
            if (u != Game.p1.Controlled && (Game.p2 == null || u != Game.p2.Controlled))
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

        if (owner != Game.p1.Controlled && (Game.p2 == null || owner != Game.p2.Controlled))
        {
            Zone z = Game.opponent(this).Zone;
            owner.Order = Order.OrderMove(new Vector3(owner.transform.position.x, 0, z.transform.position.z), Order.TYPE_DEPLACEMENT.SPRINT);
        }
        else
        {
            owner.Order = Order.OrderNothing();
        }

		/*
		Order.TYPE_POSITION typePosition = PositionInMap( owner );
		//Debug.Log("pos in map : " + typePosition);
        foreach (Unit u in units)
        {
            // FIX. (TODO)
            if (u.Order.type != Order.TYPE.SEARCH)
            {
                if (u != owner)
                {
					u.Order = Order.OrderOffensiveSide(owner, new Vector3(Game.settings.Vheight, 0, Game.settings.Vwidth/1.5f), right, typePosition);
					//u.Order = Order.OrderSupport(owner, new Vector3(Game.settings.Vheight, 0, Game.settings.Vwidth), right);
                }
            }
        }*/
    }

	public Order.TYPE_POSITION PositionInMap(Unit owner)
	{
		float largeurTerrain = Mathf.Abs(Game.limiteTerrainNordEst.transform.position.x - Game.limiteTerrainSudOuest.transform.position.x);
		float section = largeurTerrain / 5f;
		
		if (owner.transform.position.x < Game.limiteTerrainSudOuest.transform.position.x + section)
			return Order.TYPE_POSITION.EXTRA_LEFT;
		else if (owner.transform.position.x >= Game.limiteTerrainSudOuest.transform.position.x + section && owner.transform.position.x <= Game.limiteTerrainSudOuest.transform.position.x + 2*section)
			return Order.TYPE_POSITION.LEFT;
		else if (owner.transform.position.x > Game.limiteTerrainNordEst.transform.position.x - section)
			return Order.TYPE_POSITION.EXTRA_RIGHT;
		else if (owner.transform.position.x <= Game.limiteTerrainNordEst.transform.position.x - section && owner.transform.position.x >= Game.limiteTerrainNordEst.transform.position.x - 2*section)
			return Order.TYPE_POSITION.RIGHT;
		return Order.TYPE_POSITION.MIDDLE;
	}
	
    void OwnerChangedOpponents()
    {
        Unit a;
        if (Game.p1.Controlled != null && Game.p1.Controlled.Team == this)
        {
            a = Game.p1.Controlled;
        }
        else if (Game.p2 != null && Game.p2.Controlled.Team == this)
        {
            a = Game.p2.Controlled;
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
	
	public void switchPlaces(int a, int b) {
		if(a == b)
			return;
		
		if(a < 0 || b < 0 || a >= nbUnits || b >= nbUnits)
			return;
		
		Unit A, B;
		A = units[a];
		B = units[b];
		
		switchPlaces(A, B);
	}
	
	public static void switchPlaces(Unit A, Unit B) {
		if(!A || !B)
			return;
		
		Vector3 pos = A.transform.position;
		Quaternion rot = A.transform.rotation;
		
		A.transform.position = B.transform.position;
		A.transform.rotation = B.transform.rotation;
		
		B.transform.position = pos;
		B.transform.rotation = rot;
	}
		
	public void placeUnits(Transform configuration, string pattern, string filter, int from, int to) {
		
		int i = 0;
		Transform t = configuration.FindChild(pattern.Replace(filter, (i+1).ToString()));
		while(t != null && (i + from) < nbUnits && (i + from) < to) {
			
			this.placeUnit(t, i + from);
			
			i++;
			t = configuration.FindChild(pattern.Replace(filter, (i+1).ToString()));
		}			
	}
	
	public void placeUnits(Transform configuration, string pattern) {
		this.placeUnits(configuration, pattern, "#", 0, nbUnits);
	}
	
	public void placeUnits(Transform configuration, int from, int to) {
		this.placeUnits(configuration, "Player_#", "#", from, to);
	}
	
	public void placeUnits(Transform configuration, int from) {
		this.placeUnits(configuration, "Player_#", "#", from, nbUnits);
	}
	
	public void placeUnits(Transform configuration) {
		this.placeUnits(configuration, "Player_#", "#", 0, nbUnits);
	}
	
	public void placeUnit(Transform t, int index) {
		if( t == null || index < 0 || index >= nbUnits ) {
			throw new System.ArgumentException();
		}
		
		units[index].transform.position = t.position;
		units[index].transform.rotation = t.rotation;
	}
	
	//maxens dubois
	public void ChangePlayersColor(Color _color){
		foreach(Unit u in units){
			u.renderer.material.color = _color;
		}
	}
	
	public Color GetPlayerColor(){
		return units[0].renderer.material.color;
	}
	
	//maxens dubois
	public void PlaySuperParticleSystem(SuperList _super, bool play){
		switch (_super){
		case SuperList.superDash:
			foreach(Unit u in units){
				if(play == true){
					u.superDashParticles.Play();
				}else{
					u.superDashParticles.Stop();
				}
			}
			break;
		case SuperList.superTackle:
			foreach(Unit u in units){
				if(play == true){
					u.superTackleParticles.Play();
				}else{
					u.superTackleParticles.Stop();
				}
			}
			break;
		default:
			break;
		}
	}
	
	public class TeamUnitEnumerator : IEnumerator {
		Team t;
		int current;
		
		public TeamUnitEnumerator(Team t) {
			this.t = t;
			current = -1;
		}
		
		public bool MoveNext()
	    {
	        current++;
	        return (t.units != null && current < t.nbUnits);
	    }
	
	    public void Reset()
	    {
	        current = -1;
	    }
	
	    public object Current
	    {
	        get
	        {
	            try
	            {
	                return t[current];
	            }
	            catch (System.IndexOutOfRangeException)
	            {
	                throw new System.InvalidOperationException();
	            }
	        }
	    }
	}
	
	public IEnumerator GetEnumerator() {
		return new TeamUnitEnumerator(this);
	}
}
