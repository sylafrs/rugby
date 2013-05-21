using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamFormation : IAManager
{
	List<Unit> OffensiveFormation;
	List<Unit> DefensiveFormation;
	
	Gamer oGamer;
	Game oGame;
	
	float largeurTerrain;
	float section;
	float xNE;
	float xSO;
	
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
		
		xNE = oGame.limiteTerrainNordEst.transform.position.x;
		xSO = oGame.limiteTerrainSudOuest.transform.position.x;
		largeurTerrain = Mathf.Abs(xNE - xSO);
		section = largeurTerrain / 7f;
	}
		
	public void InitOffensiveFormation (Team t)
	{
		oGamer = t.Player;
		oGame = t.Game;
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
	
	public void FormationDefenseGoX()
	{
		Vector3 newPosition;
		Order.TYPE_POSITION posUnit;
		Order.TYPE_POSITION posBall;
		List<Unit> same;
		int ecartUnitBall;
		float xOffset;
		List<Unit> tmp = new List<Unit>(this.DefensiveFormation);
		
		//represente l'unite qui a le droit d'etre dans la meme zone que la balle
		Unit unitBallZoneAuthorized = unitAndBallSameZone(this.DefensiveFormation);
		
		//Pas d'unite dans la même zone que la balle, on la trouve, et la deplace dans la zone de la balle
		if (unitBallZoneAuthorized == null)
		{
			int? dist;
			Unit toMove = tooNearOfBall(this.DefensiveFormation, out dist);
			
			if (toMove != null && dist != null)
			{
				newPosition = toMove.transform.position;
				xOffset = (float)dist * section;
				newPosition.x += xOffset * (float)rand.NextDouble();
				
				toMove.Order = Order.OrderMove(newPosition, Order.TYPE_DEPLACEMENT.COURSE);
				Debug.Log("Move an unit in ballZone : " + toMove + " ecart de zone " + (float)dist);
				tmp.Remove(toMove);
			}
		}
		else
		{
			tmp.Remove(unitBallZoneAuthorized);
		}
		
		List<Unit> toto = new List<Unit>();
		toto.Add (unitBallZoneAuthorized);
		MoveUnit(toto, tmp);
		/*
		//Bouge les unites qui sont dans la meme zone, 2 unites ne peuvent être dans la même zone
		while (unitsInSameZone(tmp, out same))
		{
			
			foreach(var u in same)
			{
				newPosition = u.transform.position;
				xOffset = (float)rand.NextDouble() * section;
				if (rand.NextDouble() < 0.5)
				{
					xOffset *= -1f;
				}
				newPosition.x += xOffset;
				u.Order = Order.OrderMove(newPosition, Order.TYPE_DEPLACEMENT.COURSE);
				Debug.Log("same zone : " + u + " to : " + newPosition);
				tmp.Remove(u);
			}
			
			same.Clear();
		}
		
		
		foreach (var u in tmp)
		{
			newPosition = u.transform.position;
			
			newPosition.x += (float)rand.NextDouble() * section;
				
			u.Order = Order.OrderMove(newPosition, Order.TYPE_DEPLACEMENT.COURSE);
		}
		*/
	}
	
	/*
	 * Bouge un groupe d'unite par rapport à une liste de referent qui ne bougent pas
	 **/
	public void MoveUnit(List<Unit> referent, List<Unit> others)
	{
		Vector3 newPosition;
		int dist;
		List<Unit> refCopy = new List<Unit>(referent);
		
		if (ZoneMinBetweenDefense < 0 || ZoneMaxBetweenDefense < 0 || ZoneMinBetweenDefense >= ZoneMaxBetweenDefense)
			return;
		Unit uReferent;
		for (int i = 0; i < refCopy.Count; ++i)
		{
			uReferent = refCopy[i];
			if (uReferent != null)
			foreach(Unit uOther in others)
			{
				newPosition = uOther.transform.position;
				dist = compareZoneInMap(uReferent.gameObject, uOther.gameObject);
				
				if (Mathf.Abs(dist) < ZoneMinBetweenDefense)
				{
					if (rand.NextDouble() > 0.5)
						newPosition.x += ZoneMinBetweenDefense * section;
					else
						newPosition.x -= ZoneMinBetweenDefense * section;
				}
				else if (Mathf.Abs(dist) > ZoneMaxBetweenDefense)
				{
					if (dist > 0)
						newPosition.x -= (dist - ZoneMaxBetweenDefense) * section;
					else
						newPosition.x += (dist + ZoneMaxBetweenDefense) * section;
				}
				
				uOther.Order = Order.OrderMove(newPosition, Order.TYPE_DEPLACEMENT.SPRINT);
				refCopy.Add(uOther);
				others.Remove(uOther);
			}
		}
	}
	
	public Unit tooNearOfBall(List<Unit> l, out int? dist)
	{
		if (l.Count ==0)
		{
			dist = null;
			return null;
		}
		
		dist = compareZoneInMap(oGame.Ball.gameObject, l[0].gameObject);
		Unit res = l[0];
		int diff;
		
		foreach(var u in l)
		{
			diff = compareZoneInMap(oGame.Ball.gameObject, u.gameObject);
			
			if (diff < dist)
			{
				dist = diff;
				res = u;
			}
		}
		
		return res;
	}
	
	public Unit unitAndBallSameZone(List<Unit> l)
	{
		foreach(var u in l)
		{
			if (compareZoneInMap(u.gameObject, oGame.Ball.gameObject) == 0)
				return u;
		}
		return null;
	}
	
	public bool unitsInSameZone(List<Unit> toCheck, out List<Unit> same)
	{
		List<Unit> tmp = new List<Unit>(toCheck);
		same = new List<Unit>();
		
		if (toCheck.Count == 0)
			return false;
		
		foreach(var u in toCheck)
		{
			//tmp = toCheck;
			tmp.Remove(u);
			foreach(var c in tmp)
			{
				if (compareZoneInMap(u.gameObject, c.gameObject) == 0)
				{
					if (!same.Contains(u))
					{
						same.Add(u);
					}
					if(!same.Contains(c))
					{
						same.Add(c);
					}
				}
			}
		}
		return same.Count > 0 ? true : false;
	}
	
	public void FormationGoZ(List<Unit> formation, float ecart)
	{
		Vector3 newPosition;
		
		
		foreach(var u in formation)
		{
			newPosition = u.transform.position;
			
			//if (oGamer.Controlled.transform.position.z + ecart < oGamer.Controlled.transform.position.z)
			newPosition.z += ecart;
			
			
			//Debug.DrawLine(u.transform.position, newPosition, Color.green,10f);
			//Debug.Log("anciennePosition : " + u.transform.position + " newPos : " + newPosition);
			
			u.Order = Order.OrderMove(newPosition, Order.TYPE_DEPLACEMENT.COURSE);
		}
	}
	
	/*
	 * Cette fonction me retourne le nombre de zone d'écart entre deux positions d'objets.
	 * Si le retour est négatif, alors "other" est à gauche de "referent"
	 * Si le retour est positif, alors "other" est à droite de "referent"
	 **/
	public int compareZoneInMap( Order.TYPE_POSITION referent, Order.TYPE_POSITION other )
	{
		return (int)referent - (int)other;
	}
	
	public int compareZoneInMap(GameObject referent, GameObject other)
	{
		return (int)PositionInMap(referent) - (int)PositionInMap(other);
	}
	
	public int compareZoneInMap(float referent, float other)
	{
		return (int)PositionInMap(referent) - (int)PositionInMap(other);
	}
	
	public Order.TYPE_POSITION PositionInMap(float obj)
	{

		if (obj >= xSO && obj < xSO + section)
			return Order.TYPE_POSITION.EXTRA_LEFT;
		else if (obj >= xSO + section && obj < xSO + 2*section)
			return Order.TYPE_POSITION.LEFT;
		else if (obj <= xNE && obj > xNE - section)
			return Order.TYPE_POSITION.EXTRA_RIGHT;
		else if (obj <= xNE - section && obj > xNE - 2*section)
			return Order.TYPE_POSITION.RIGHT;
		else if (obj >= xSO + 2 * section && obj < xSO + 3*section)
			return Order.TYPE_POSITION.MIDDLE_LEFT;
		else if (obj <= xNE - 2 * section && obj > xNE - 3*section)
			return Order.TYPE_POSITION.MIDDLE_RIGHT;
		return Order.TYPE_POSITION.MIDDLE;
	}
	
	public Order.TYPE_POSITION PositionInMap(GameObject obj)
	{

		if (obj.transform.position.x >= xSO && obj.transform.position.x < xSO + section)
			return Order.TYPE_POSITION.EXTRA_LEFT;
		else if (obj.transform.position.x >= xSO + section && obj.transform.position.x < xSO + 2*section)
			return Order.TYPE_POSITION.LEFT;
		else if (obj.transform.position.x <= xNE && obj.transform.position.x > xNE - section)
			return Order.TYPE_POSITION.EXTRA_RIGHT;
		else if (obj.transform.position.x <= xNE - section && obj.transform.position.x > xNE - 2*section)
			return Order.TYPE_POSITION.RIGHT;
		else if (obj.transform.position.x >= xSO + 2 * section && obj.transform.position.x < xSO + 3*section)
			return Order.TYPE_POSITION.MIDDLE_LEFT;
		else if (obj.transform.position.x <= xNE - 2 * section && obj.transform.position.x > xNE - 3*section)
			return Order.TYPE_POSITION.MIDDLE_RIGHT;
		return Order.TYPE_POSITION.MIDDLE;
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
