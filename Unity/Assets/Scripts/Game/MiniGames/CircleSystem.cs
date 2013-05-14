using UnityEngine;
using System.Collections;

public class CircleSystem : MonoBehaviour {
	
	public Team left;
	public Team right;
	public float raySquare;
	public float YToDecide = 3f;
	public Ball ball;
	
	public System.Collections.Generic.List<Unit> unitInCircle;
	
	// Use this for initialization
	void Start () {
		unitInCircle = new System.Collections.Generic.List<Unit>();
	}
	
	// Update is called once per frame
	void Update () {

		if (ball.transform.position.y < YToDecide && ball.PreviousOwner.canCatchTheBall)
		{
			ball.Owner = GetFirstUnit();
		}
		else{
			unitIn(left);
			unitIn(right);
		}
	}
	
	void OnEnable(){
		unitInCircle.Clear();
	}
	
	Unit GetFirstUnit(){
		if (unitInCircle.Count != 0)
		{
			Debug.Log(unitInCircle[0]);
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
					unitInCircle.Add(u);
			}
			else if (unitInCircle.Contains(u))
			{
				unitInCircle.Remove(u);
			}
		}
	}
}
