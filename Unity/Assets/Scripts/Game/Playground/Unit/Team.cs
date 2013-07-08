using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TeamNationality{
	MAORI,
	JAPANESE
};

/**
 * @class Team
 * @brief Une equipe (unite)
 * @author Sylvain Lafon
 */
[System.Serializable, AddComponentMenu("Scripts/Models/Team")]
public class Team : myMonoBehaviour, IEnumerable
{
	public TeamNationality nationality;
	
	public Team opponent { get; set; }
	public Gamer Player { get; set; }
	public Game game { get; set; }

	public float AngleOfFovTackleCrit = 0.0f;
	public Color ConeTackle = new Color(1f, 1f, 1f, 0.33f);
	public Color DiscTackle = new Color(0f, 0f, 1f, 0.33f);
	public Color Color;
	public Color PlaqueColor;
	public string Name;
	public bool south;

	public bool useColors = false;

	public But But;
	public Zone Zone;

	private bool _fixUnits = false;
	public bool fixUnits
	{
		get
		{
			return _fixUnits;
		}
		set
		{
			_fixUnits = value;
			setSpeed();
		}
	}

	public int nbPoints = 0;

	private Unit[] units;

	public float speedFactor;
	public float tackleFactor;
	
	public DodgingStateSettings settings;
	
	public float unitSpeed;
	public float handicapSpeed = 1;
	public float unitTackleRange;

	public int nbOffensivePlayer;

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

    public Unit captain;
	
    public GameObject Prefab_model;
    public GameObject Prefab_capitaine;

	private superController _super;
	public  superController Super
	{
		get
		{
			if (_super == null)
			{
				_super = this.GetComponent<superController>();
			}

			return _super;
		}
	}

	//maxens dubois
	public int SuperGaugeValue;

	public int nbUnits;

	public void Start()
	{
		But.Owner = this;
		Zone.Owner = this;
		this.settings = Game.instance.settings.GameStates.MainState.PlayingState.MainGameState.RunningState.BallHandlingState.DodgingState;

		speedFactor = 1f;
		tackleFactor = 1f;
	}

	public void setSpeed()
	{
		foreach (var u in units)
		{
			if (u.nma)
			{
				if (u.Dodge)
				{
					u.nma.speed = fixUnits ? 0 : unitSpeed * this.settings.unitDodgeSpeedFactor;
				}
				//else if (game.Ball.Owner != null && game.Ball.Owner.Team == this)
				else if (game.Ball.Team == this)
				{
					u.nma.speed = fixUnits ? 0 : (unitSpeed - handicapSpeed) * speedFactor;
				}
				else
				{
					u.nma.speed = fixUnits ? 0 : unitSpeed * speedFactor;
				}
				u.nma.acceleration = (u.nma.speed == 0) ? 10000 : 100; // Valeur "à l'arrache" TODO
			}
		}
	}

	public void StunEverybodyForSeconds(float stunDuration)
	{
		foreach (Unit u in units)
		{
            if (u == game.Ball.Owner)
            {
                game.Ball.Owner = null;
                game.Ball.TeleportOnGround();
            }

			u.sm.event_stun(stunDuration);
		}
	}

	public void CreateUnits()
	{
		units = new Unit[nbUnits];
		for (int i = 0; i < nbUnits; i++)
		{
            GameObject prefab = Prefab_model;
            if (i == 2 && Prefab_capitaine != null)
                prefab = Prefab_capitaine;

			GameObject o = GameObject.Instantiate(prefab) as GameObject;
			units[i] = o.GetComponent<Unit>();
			units[i].game = game;
			units[i].name = Name + " " + (i + 1).ToString("D2");
			units[i].transform.parent = this.transform;
			units[i].Team = this;
            units[i].index = i;

            if (i == 2)
            {
                captain = units[i];
            }

			//units[i].renderer.material.color = Color;
		}

		setSpeed();
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
		if (game.disableIA)
		{
			return;
		}

		if (game.Ball.Owner == null)
		{
			if (game.Ball.PreviousOwner.Team != this && game.Ball.NextOwner != null && game.Ball.NextOwner.Team != this)
			{
				setSpeed();
				OwnerChangedBallFree();
			}
			else if (game.Ball.PreviousOwner.Team == game.southTeam)
			{
				setSpeed();
				OwnerChangedOurs();
			}
			else if (game.Ball.NextOwner != null && game.Ball.NextOwner.Team != this)
			{
				OwnerChangedOurs();
			}

		}
		else
		{
			setSpeed();
			OwnerChangedOpponents();
		}
	}

    public void increaseSuperGauge(int value)
    {
        int max = game.settings.Global.Super.superGaugeMaximum;
        if(SuperGaugeValue < max) {
            SuperGaugeValue += value;
            if (SuperGaugeValue >= max)
            {
                SuperGaugeValue = max;
                this.game.OnSuperLoaded(this);
            }
        }

    }

	//Retourne le nombre de joueur de type offensif sans le controllé
	public int GetNumberOffensivePlayer()
	{
		int nb = 0;

		foreach (Unit u in this)
		{
			if (u != Player.Controlled && u.typeOfPlayer== Unit.TYPEOFPLAYER.OFFENSIVE)
			{
				nb++;
			}
		}
		return nb;
	}

	void OwnerChangedBallFree()
	{
		foreach (Unit u in units)
		{
			if (u != game.northTeam.Player.Controlled && u != game.southTeam.Player.Controlled)
			{
				u.Order = Order.OrderNothing(); // Order.OrderFollowBall();
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
		Unit owner = game.Ball.Owner;
		if (owner == null)
		{
			owner = game.Ball.PreviousOwner;
		}

		if (owner != game.southTeam.Player.Controlled && (game.northTeam.Player == null || owner != game.northTeam.Player.Controlled))
		{
			Zone z = this.opponent.Zone;
			owner.Order = Order.OrderMove(new Vector3(owner.transform.position.x, 0, z.transform.position.z));
		}
		else
		{
			owner.Order = Order.OrderNothing();
		}

		/*
		Order.TYPE_POSITION typePosition = PositionInMap( owner );
		//MyDebug.Log("pos in map : " + typePosition);
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
		float largeurTerrain = Mathf.Abs(game.refs.positions.limiteTerrainNordEst.transform.position.x - game.refs.positions.limiteTerrainSudOuest.transform.position.x);
		float section = largeurTerrain / 7f;

		if (owner.transform.position.x < game.refs.positions.limiteTerrainSudOuest.transform.position.x + section)
			return Order.TYPE_POSITION.EXTRA_LEFT;
		else if (owner.transform.position.x >= game.refs.positions.limiteTerrainSudOuest.transform.position.x + section && owner.transform.position.x <= game.refs.positions.limiteTerrainSudOuest.transform.position.x + 2 * section)
			return Order.TYPE_POSITION.LEFT;
		else if (owner.transform.position.x > game.refs.positions.limiteTerrainNordEst.transform.position.x - section)
			return Order.TYPE_POSITION.EXTRA_RIGHT;
		else if (owner.transform.position.x <= game.refs.positions.limiteTerrainNordEst.transform.position.x - section && owner.transform.position.x >= game.refs.positions.limiteTerrainNordEst.transform.position.x - 2 * section)
			return Order.TYPE_POSITION.RIGHT;
		else if (owner.transform.position.x <= game.refs.positions.limiteTerrainSudOuest.transform.position.x + 2 * section && owner.transform.position.x <= game.refs.positions.limiteTerrainSudOuest.transform.position.x + 2 * section)
			return Order.TYPE_POSITION.MIDDLE_LEFT;
		else if (owner.transform.position.x <= game.refs.positions.limiteTerrainNordEst.transform.position.x - 2 * section && owner.transform.position.x >= game.refs.positions.limiteTerrainNordEst.transform.position.x - 3 * section)
			return Order.TYPE_POSITION.MIDDLE_RIGHT;
		return Order.TYPE_POSITION.MIDDLE;
	}

	void OwnerChangedOpponents()
	{
		Unit a;
		if (game.southTeam.Player.Controlled != null && game.southTeam.Player.Controlled.Team == this)
		{
			a = game.southTeam.Player.Controlled;
		}
		else if (game.northTeam.Player.Controlled != null && game.northTeam.Player.Controlled.Team == this)
		{
			a = game.northTeam.Player.Controlled;
		}
		else
		{
			a = units[0];
			float d = Vector3.Distance(a.transform.position, game.Ball.Owner.transform.position);
			for (int i = 0; i < units.Length; i++)
			{
				float d2 = Vector3.Distance(units[i].transform.position, game.Ball.Owner.transform.position);
				if (d > d2)
				{
					a = units[i];
					d = d2;
				}
			}

			a.Order = Order.OrderFollow(game.Ball.Owner.transform);
		}

		foreach (Unit u in units)
		{
			if (u != a)
			{
				//u.Order = Order.OrderAttack(a, game.settings.LineSpace, right);
				u.Order = Order.OrderNothing();
			}
		}
	}

	public void switchPlaces(int a, int b)
	{
		if (a == b)
			return;

		if (a < 0 || b < 0 || a >= nbUnits || b >= nbUnits)
			return;

		Unit A, B;
		A = units[a];
		B = units[b];

		switchPlaces(A, B);
	}

	public static void switchPlaces(Unit A, Unit B)
	{
		if (!A || !B)
			return;

		Vector3 pos = A.transform.position;
		Quaternion rot = A.transform.rotation;

		A.transform.position = B.transform.position;
		A.transform.rotation = B.transform.rotation;

		B.transform.position = pos;
		B.transform.rotation = rot;
	}


	public void placeUnits(Transform configuration, string pattern, string filter, int from, int to, bool teleport)
	{

		if (configuration == null)
		{
			throw new UnityException("placeUnits configuration is null");
		}

		int i = 0;
		Transform t = configuration.FindChild(pattern.Replace(filter, (i + 1).ToString()));
		while (t != null && (i + from) < nbUnits && (i + from) < to)
		{

			this.placeUnit(t, i + from, teleport);

			i++;
			t = configuration.FindChild(pattern.Replace(filter, (i + 1).ToString()));
		}
	}

	public void placeUnits(Transform configuration, string pattern, bool teleport)
	{
		this.placeUnits(configuration, pattern, "#", 0, nbUnits, teleport);
	}

	public void placeUnits(Transform configuration, int from, int to, bool teleport)
	{
		this.placeUnits(configuration, "Player_#", "#", from, to, teleport);
	}

	public void placeUnits(Transform configuration, int from, bool teleport)
	{
		this.placeUnits(configuration, "Player_#", "#", from, nbUnits, teleport);
	}

	public void placeUnits(Transform configuration, bool teleport)
	{
		this.placeUnits(configuration, "Player_#", "#", 0, nbUnits, teleport);
	}

	public void placeUnit(Transform dst, int index, bool teleport)
	{
		if (dst == null || index < 0 || index >= nbUnits)
		{
			throw new System.ArgumentException();
		}

		Unit unit = units[index];

		if (teleport)
		{
			unit.transform.position = dst.position;
			unit.transform.rotation = dst.rotation;
		}
		else
		{
			unit.nma.speed = 10;
			unit.Order = Order.OrderMove(dst.position);
			unit.transform.rotation = dst.rotation;
		}
	}

	//maxens dubois
	public void ChangePlayersColor(Color _color)
	{
		foreach (Unit u in units)
		{
			u.renderer.material.color = _color;
		}
	}

	public Color GetPlayerColor()
	{
		return units[0].renderer.material.color;
	}

	// TODO : faire un effet par super serait mieux.
	// Cette solution fonctionne mais est pwerk..
	public GameObject fxSuper;
	private GameObject[] myFxSuper;
	public GameObject lightSuper;
    public GameObject groundSuperPrefab;
    public static GameObject groundSuper;
    private bool prevFX = false;

	public void PlaySuperParticleSystem(SuperList super, bool play)
	{
		this.PlaySuperParticleSystem(play);
	}

    public void PlaySuperGroundEffect()
    {
        if (groundSuper)
            GameObject.Destroy(groundSuper);
        if(groundSuperPrefab) 
            groundSuper = GameObject.Instantiate(groundSuperPrefab) as GameObject;
    }

	public void PlaySuperParticleSystem(bool play)
	{
        if (prevFX == play)
            return;

        prevFX = play;

		if (play)
			myFxSuper = new GameObject[this.nbUnits];

		int i = 0;

		if (play && lightSuper)
			lightSuper.SetActive(true);

		if (myFxSuper != null)
		{
			foreach (Unit u in this.units)
			{
				if (play)
				{
					myFxSuper[i] = GameObject.Instantiate(fxSuper) as GameObject;
					myFxSuper[i].transform.parent = u.transform;
					myFxSuper[i].transform.localPosition = Vector3.zero;
				}
				else
				{
					if (myFxSuper[i])
						GameObject.Destroy(myFxSuper[i]);
				}

				i++;
			}
		}

		if (!play && lightSuper)
			lightSuper.SetActive(false);

		if (!play)
			myFxSuper = null;
	}

    public void OnTouch()
    {
        foreach (Unit u in units)
        {
            UnitAnimator a = u.unitAnimator;
            if (a != null)
                a.OnTouch();
        }
    }

    public void OnTouchAction()
    {
        foreach (Unit u in units)
        {
            UnitAnimator a = u.unitAnimator;
            if (a != null)
                a.OnTouchAction();
        }
    }

	public class TeamUnitEnumerator : IEnumerator
	{
		Team t;
		int current;

		public TeamUnitEnumerator(Team t)
		{
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

	public IEnumerator GetEnumerator()
	{
		return new TeamUnitEnumerator(this);
	}

	public void ShowPlayers(bool active)
	{
		for (int i = 0; i < this.units.Length; i++)
		{
			this.units[i].ShowPlayer(active);
		}
	}
}
