using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamFormation {
		List<Unit> OffensiveFormation;
		List<Unit> DefensiveFormation;
		
		Gamer g;
		public TeamFormation()
		{
			OffensiveFormation = new List<Unit>();
			DefensiveFormation = new List<Unit>();
		}
		
		public void AddUnit(List<Unit> l, Unit u)
		{
			if (l.Contains(u))
				return;
			l.Add(u);
		}
		
		public void RemoveUnit(List<Unit> l, Unit u)
		{
			if (!l.Contains(u))
				return;
			l.Remove(u);
		}
		
		public void InitFormation(Team t)
		{
			InitOffensiveFormation(t);
			InitDefensiveFormation(t);
		}
		
		public void InitOffensiveFormation(Team t)
		{
			g = t.GetComponent<Gamer>();
			List<Unit> near = g.GetListUnitNear(g.Controlled);
			AddUnit(OffensiveFormation, g.Controlled);
			foreach(Unit u in near)
			{
				AddUnit(OffensiveFormation, u);
			}
		}
		
		public void InitDefensiveFormation(Team t)
		{
			foreach(Unit u in t)
			{
				if (!OffensiveFormation.Contains(u))
					AddUnit(DefensiveFormation, u);
			}
		}
		
		/*
		 * Cette fonction va me permettre de modifier les différentes unités de la liste en fonction de certains cas
		 * 		- le controllé est un défenseur : retirer le controllé de la liste des défenseurs, l'ajouter dans la liste des attaquants
		 * 			et réorganisé comme il faut les formations
		 * 
		 **/
		public void UpdateTeamFormation(Team t)
		{
			if (OffensiveFormation.Contains(g.Controlled))
				return;
			else
			{
				DefensiveFormation.Remove(g.Controlled);
				List<Unit> near = g.GetListUnitNear(g.Controlled);
				AddUnit(OffensiveFormation, g.Controlled);
				foreach(Unit u in near)
				{
					AddUnit(OffensiveFormation, u);
				}
			
				foreach(Unit u in t)
				{
					if (!OffensiveFormation.Contains(u))
						AddUnit(DefensiveFormation, u);
				}
			}
		}
		
		public override string ToString ()
		{
			string s="OffensiveFormation : ";
			uint uTmp = 0;
			
			foreach(Unit u in OffensiveFormation)
			{
				s += u + ", ";
			}
			s+="DefensiveFormation : ";
			
			foreach(Unit u in DefensiveFormation)
			{
				if (uTmp == DefensiveFormation.Count - 1)
				{
					s+= u;
				}
				else
				{
					s+= u + ", ";
				}
				uTmp++;
			}

			return s;
		}
}
