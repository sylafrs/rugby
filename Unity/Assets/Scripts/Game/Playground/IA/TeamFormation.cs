using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamFormation
{
	List<Unit> OffensiveFormation;
	List<Unit> DefensiveFormation;
	
	Gamer oGamer;
	
	System.Random rand = new System.Random();
	
	public TeamFormation ()
	{
		OffensiveFormation = new List<Unit> ();
		DefensiveFormation = new List<Unit> ();
	}
		
	public void AddUnit (List<Unit> l, Unit u)
	{
		if (l.Contains (u))
			return;
		l.Add (u);
	}
		
	public void RemoveUnit (List<Unit> l, Unit u)
	{
		if (!l.Contains (u))
			return;
		l.Remove (u);
	}
		
	public void InitFormation (Team t)
	{
		InitOffensiveFormation (t);
		InitDefensiveFormation (t);
	}
		
	public void InitOffensiveFormation (Team t)
	{
		oGamer = t.GetComponent<Gamer> ();
		List<Unit> near = oGamer.GetListUnitNear (oGamer.Controlled);
		AddUnit (OffensiveFormation, oGamer.Controlled);
		foreach (Unit u in near) {
			AddUnit (OffensiveFormation, u);
		}
	}
		
	public void InitDefensiveFormation (Team t)
	{
		foreach (Unit u in t) {
			if (!OffensiveFormation.Contains (u))
				AddUnit (DefensiveFormation, u);
		}
	}
		
	/*
		 * Cette fonction va me permettre de modifier les différentes unités de la liste en fonction de certains cas
		 * 		- le controllé est un défenseur : retirer le controllé de la liste des défenseurs, l'ajouter dans la liste des attaquants
		 * 			et réorganisé comme il faut les formations
		 * 
		 **/
	public void UpdateTeamFormation (Team t)
	{
		if (OffensiveFormation.Contains (oGamer.Controlled))
			return;
		else {
			DefensiveFormation.Remove (oGamer.Controlled);
			List<Unit> near = oGamer.GetListUnitNear (oGamer.Controlled);
			AddUnit (OffensiveFormation, oGamer.Controlled);
			foreach (Unit u in near) {
				AddUnit (OffensiveFormation, u);
			}
			
			foreach (Unit u in t) {
				if (!OffensiveFormation.Contains (u))
					AddUnit (DefensiveFormation, u);
			}
		}
	}
		
	public override string ToString ()
	{
		string s = "OffensiveFormation : ";
		uint uTmp = 0;
			
		foreach (Unit u in OffensiveFormation) {
			s += u + ", ";
		}
		s += "DefensiveFormation : ";
			
		foreach (Unit u in DefensiveFormation) {
			if (uTmp == DefensiveFormation.Count - 1) {
				s += u;
			} else {
				s += u + ", ";
			}
			uTmp++;
		}

		return s;
	}
	
	//Compare toutes les coordonnées
	public float distanceBetweenFormation(out Unit OffNear, out Unit DefNear)
	{
		float dist;
		OffNear = OffensiveFormation[0];
		DefNear = DefensiveFormation[0];
		float min = Vector3.SqrMagnitude(OffNear.transform.position - DefNear.transform.position);

		foreach (Unit uOffensive in OffensiveFormation)
		{
			foreach (Unit uDefensive in DefensiveFormation)
			{
				dist = Vector3.SqrMagnitude(uOffensive.transform.position - uDefensive.transform.position);
				if( Others.nearlyEqual( dist, min, 0.00001f) )
				{
					min = dist;
					OffNear = uOffensive;
					DefNear = uDefensive;
				}
			}
		}
		return min;
	}
	
	//Calcule la distance entre les lignes attaques défenses (uniquement z)
	public float distanceBetweenLanes()
	{
		float dist;
		float min = Mathf.Abs(OffensiveFormation[0].transform.position.z - DefensiveFormation[0].transform.position.z);

		foreach (Unit uOffensive in OffensiveFormation)
		{
			foreach (Unit uDefensive in DefensiveFormation)
			{
				dist = Mathf.Abs(uOffensive.transform.position.z - uDefensive.transform.position.z);
				if( Others.nearlyEqual( dist, min, 0.00001f) )
				{
					min = dist;
				}
			}
		}
		return min;
	}
	
	public void FormationGo(List<Unit> formation, float ecart)
	{
		Vector3 newPosition;
		Order.TYPE_POSITION type;
		
		foreach(var u in formation)
		{
			newPosition = u.transform.position;
			type = u.Team.PositionInMap(u);
			
			//if (oGamer.Controlled.transform.position.z + ecart < oGamer.Controlled.transform.position.z)
			newPosition.z += ecart;
			
			if (type == Order.TYPE_POSITION.MIDDLE_LEFT)
				newPosition.x += (float)rand.NextDouble() * -5f;
			else if (type == Order.TYPE_POSITION.MIDDLE_RIGHT)
				newPosition.x += (float)rand.NextDouble() * 5f;
			else if (type == Order.TYPE_POSITION.MIDDLE)
				newPosition.x += (float)rand.NextDouble();
			else if (type == Order.TYPE_POSITION.LEFT || type == Order.TYPE_POSITION.EXTRA_LEFT)
				newPosition.x += (float)rand.NextDouble() * -10f;
			else if (type == Order.TYPE_POSITION.RIGHT || type == Order.TYPE_POSITION.EXTRA_RIGHT)
				newPosition.x += (float)rand.NextDouble() * 10f;
			
			//Debug.DrawLine(u.transform.position, newPosition, Color.green,10f);
			Debug.Log("anciennePosition : " + u.transform.position + " newPos : " + newPosition);
			
			u.Order = Order.OrderMove(newPosition, Order.TYPE_DEPLACEMENT.COURSE);
		}
	}
	
	public void ManageDefenseUnit (Unit tooNear, Unit referent, float ray)
	{
		Order.TYPE_POSITION type = tooNear.Team.PositionInMap(tooNear);
		Vector3 newPosition;
		
		//Permet de définir une fourchette angulaire pour la future position de "tooNear"
		//180° pour toute notre moitié d'équipe, soit 3*60°
		int fRandAngle = Random.Range(0,60);
		float angle;
		if (type == Order.TYPE_POSITION.EXTRA_LEFT || type == Order.TYPE_POSITION.LEFT)
		{
			
			angle = Mathf.Deg2Rad * (120 + fRandAngle);
			
		}
		else if (type == Order.TYPE_POSITION.EXTRA_RIGHT || type == Order.TYPE_POSITION.RIGHT)
		{
			angle = Mathf.Deg2Rad * (0 + fRandAngle);
		}
		else
		{
			angle = Mathf.Deg2Rad * (60 + fRandAngle);
		}
		
		newPosition = new Vector3(referent.transform.position.x + ray*Mathf.Cos(angle),
				0 ,
				referent.transform.position.z + ray*Mathf.Sin(angle));
		
		tooNear.Order = Order.OrderMove(newPosition, Order.TYPE_DEPLACEMENT.COURSE);
	}
	
	public List<Unit> GetOffensiveFormation()
	{
		return OffensiveFormation;
	}
	
	public List<Unit> GetDefensiveFormation()
	{
		return DefensiveFormation;
	}
	
	public Team GetTeam()
	{
		if (OffensiveFormation.Count > 0)
			return OffensiveFormation[0].Team;
		else
			return DefensiveFormation[0].Team;
	}
}
