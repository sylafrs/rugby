using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/IAManager")]
public class IAManager : MonoBehaviour {
	
//	/*Variable à set dans unity*/
//	public Team[] teamTweak;
//	public float DistanceMinBetweenFormation = 10f; //Distance entre la formation offensive et la formation défensive à ne jamais restreindre
//	public float DistanceMaxBetweenFormation = 15f; //Distance entre la formation offensive et la formation défensive à ne jamais dépasser
//	public int ZoneMinBetweenDefense = 1;
//	public int ZoneMaxBetweenDefense = 2;
//	public float brainlagIATimePrioritary = 0.1f;
//	public float brainlagIATimeSecondary = 0.2f;
	
//	private const uint NBTEAMTOMANAGE = 2;
//	private Team BallOwner;
//	private float timeToManage = 0f;
	
//	private TeamFormation[] TeamManager;
	
//	// Use this for initialization
//	void Start () {
//		TeamManager = new TeamFormation[NBTEAMTOMANAGE];
		
//		for(uint u = 0; u < NBTEAMTOMANAGE; ++u)
//		{
//			TeamManager[u] = new TeamFormation();
//		}
		
//		if (teamTweak[0] != null)
//		{
//			TeamManager[0].InitFormation(teamTweak[0]);
//			//Debug.Log(TeamManager[0].ToString());
//		}
		
//		if (teamTweak[1] != null)
//		{
//			TeamManager[1].InitFormation(teamTweak[1]);
//			//Debug.Log(TeamManager[1].ToString());
			
//		}
//	}
	
//	// Update is called once per frame
//	/*void Update () {
//		if (teamTweak[0].Game.state ==Game.State.PLAYING)
//		{
//			UpdateTeamManager();
			
//			if (teamTweak[0].Game.state == Game.State.PLAYING && timeToManage <= Mathf.Max (brainlagIATimePrioritary, brainlagIATimeSecondary))
//				timeToManage += Time.deltaTime;
//			else
//				timeToManage = 0f;
			
//			if (BallOwner != teamTweak[0].Game.Ball.Owner.Team)
//				BallOwner = teamTweak[0].Game.Ball.Owner.Team;
			
//			if (DistanceMinBetweenFormation < 0f)
//				DistanceMinBetweenFormation = 0f;
			
//			for (int iTeam = 0; iTeam < NBTEAMTOMANAGE; ++iTeam)
//			{
//				UpdateTeamPlacement(iTeam);
//			}
//		}
//	}*/
	
//	/*
//	 * Me sert à update mes listes (intéractions entre elle)
//	 * Si un défenseur récupère la balle:
//	 *.		-il devient automatiquement un attaquant
//	 * 		-les listes sont mises à jours en conséquences
//	 * 		-le plus lointain de la situation devient défenseur
//	 **/
//	void UpdateTeamManager()
//	{
//		for (int uTeam = 0; uTeam < NBTEAMTOMANAGE; ++uTeam)
//		{
//			TeamManager[uTeam].UpdateTeamFormation(teamTweak[uTeam]);
//			//Debug.Log(TeamManager[uTeam].ToString());
//		}
//	}
	
	
//	/*
//	 * Me sert à update le placement des joueurs dans les listes
//	 * 		- Pour la team qui n'a pas la balle, le replacement des défenseurs est prioritaire
//	 * 		- Pour la team ayant le ballon, le replacement des attaquants est prioritaire
//	 * 		- les brainlagX[1] et brainlagX[2] correspondent toujours au temps de réaction des joueurs prioritaires
//	 **/
//	void UpdateTeamPlacement(int indexTeam)
//	{
//		float distanceMinSquare = DistanceMinBetweenFormation*DistanceMinBetweenFormation;
		
//		if ( BallOwner == teamTweak[indexTeam] )
//		{
//			//prio = offensive
//		}
//		else
//		{
//			//prio = defensive
//			if ( timeToManage != 0f && timeToManage < brainlagIATimePrioritary )
//			{
//				float ecart = TeamManager[indexTeam].distanceBetweenLanes();
				
//				//Defenseur trop proche des avants
//				if (ecart < DistanceMinBetweenFormation)
//				{
//					//Debug.Log("too near : " + ecart);
//					TeamManager[indexTeam].FormationGoZ(TeamManager[indexTeam].GetDefensiveFormation(),( indexTeam == 0 ? -ecart : ecart ));
//				}
				
//				//Defenseur trop loin des avants
//				else if (ecart > DistanceMaxBetweenFormation)
//				{
//					//Debug.Log("too far : " + ecart);
//					TeamManager[indexTeam].FormationGoZ(TeamManager[indexTeam].GetDefensiveFormation(),( indexTeam == 0 ? ecart : -ecart ));
//				}
				
//				TeamManager[indexTeam].FormationDefenseGoX();
//				/*
//				foreach(var u in TeamManager[indexTeam].GetDefensiveFormation())
//				{
					
//					TeamManager[indexTeam].ManageDefenseUnit(u, 
//															teamTweak[indexTeam].GetComponent<Gamer>().Controlled, 
//															DistanceMinBetweenFormation);
//				}*/
//			}
//		}
		
//		/*
//		Unit offensiveNear = oToUpdate.getOffensiveFormation()[0];
//		Unit defensiveNear = oToUpdate.getDefensiveFormation()[0];
//		float dist = oToUpdate.distanceBetweenFormation(out offensiveNear, out defensiveNear);
		
//			//il faut déplacer l'unité la plus proche des joueurs si la distance entre leslignes n'est pas la bonne
//		if ( dist < distanceMinSquare )
//			oToUpdate.ManageDefenseUnit(defensiveNear, offensiveNear, DistanceMinBetweenFormation);
//		*/
//	}
}
