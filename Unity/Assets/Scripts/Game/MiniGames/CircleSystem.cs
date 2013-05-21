using UnityEngine;
using System.Collections;

public class CircleSystem : MonoBehaviour {

    public Game game;
	public float raySquare;
	public float YToDecide = 3f;
	
	public bool winnerDrop;
	
	public System.Collections.Generic.List<Unit> unitInCircle;
	
	// Use this for initialization
	void Start () {
		unitInCircle = new System.Collections.Generic.List<Unit>();
	}
	
	// Update is called once per frame
	void Update () {
		if (winnerDrop)
			return;
		
		if (game.Ball.transform.position.y < YToDecide && !winnerDrop)
		{
            game.Ball.Owner = GetFirstUnit();
		}
		else{
			unitIn(game.northTeam);
            unitIn(game.southTeam);
		}
	}
	
	void OnEnable(){
		unitInCircle.Clear();
		winnerDrop = false;
	}
	
	void OnDisable()
	{
		//
		//foreach(Unit u in unitInCircle)
		//	
	}
	
	Unit GetFirstUnit(){
		if (unitInCircle.Count != 0)
		{
			
			unitInCircle[0].canCatchTheBall = true;
			winnerDrop = true;
			return unitInCircle[0];
		}
		else
			return null;
	}
	
	/*
	 * Equation de cercle
	 * 
	 * Soit O centre du cercle et M un point du cercle tel que OM² = raySquare
	 * alors le cercle a pour équation :
	 * (Ym-Yo)² + (Xm-Xo)² = OM²
	 * 
	 * donc si le rayon entre le centre du cercle et notre unité a une distance <= raySquare alors l'unité est 
	 * dans le cercle
	 */
	void unitIn(Team t)
	{
		foreach(Unit u in t)
		{
			if ( (u.transform.position.x-this.transform.position.x)*(u.transform.position.x-this.transform.position.x) +
				(u.transform.position.z-this.transform.position.z)*(u.transform.position.z-this.transform.position.z) <= raySquare)
			{
				if (!unitInCircle.Contains(u))
				{
					unitInCircle.Add(u);
					u.canCatchTheBall = false;
				}
			}
			else if (unitInCircle.Contains(u))
			{
				unitInCircle.Remove(u);
				u.canCatchTheBall = true;
			}
		}
	}
	
}
