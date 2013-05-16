using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/IAManager")]
public class IAManager : MonoBehaviour {
	
	/*Variable à set dans unity*/
	public Team[] teamTweak;
	public uint DistanceMinBetweenFormation; //Distance entre la formation offensive et la formation défensive à ne jamais restreindre
	public uint nbReplacementPerFrame; //Tricks pour ne pas tout mettre à jour à chaque frame = ressemble à un temps de réaction de l'IA
	
	const uint NBTEAMTOMANAGE = 2;

	
	public TeamFormation[] TeamManager;
	
	// Use this for initialization
	void Start () {
		TeamManager = new TeamFormation[NBTEAMTOMANAGE];
		
		for(uint u = 0; u < NBTEAMTOMANAGE; ++u)
		{
			TeamManager[u] = new TeamFormation();
		}
		
		if (teamTweak[0] != null)
		{
			TeamManager[0].InitFormation(teamTweak[0]);
			//Debug.Log(TeamManager[0].ToString());
		}
		
		if (teamTweak[1] != null)
		{
			TeamManager[1].InitFormation(teamTweak[1]);
			//Debug.Log(TeamManager[1].ToString());
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		UpdateTeamManager();
		
		for (uint uTeam = 0; uTeam < NBTEAMTOMANAGE; ++uTeam)
		{
			for (uint uReplacementProcess = 0; uReplacementProcess < nbReplacementPerFrame; ++uReplacementProcess)
			{
				UpdateTeamPlacement(TeamManager[uTeam]);
			}
		}
	}
	
	/*
	 * Me sert à update mes listes (intéractions entre elle)
	 * Si un défenseur récupère la balle:
	 *.		-il devient automatiquement un attaquant
	 * 		-les listes sont mises à jours en conséquences
	 * 		-le plus lointain de la situation devient défenseur
	 **/
	void UpdateTeamManager()
	{
		for (uint uTeam = 0; uTeam < NBTEAMTOMANAGE; ++uTeam)
		{
			TeamManager[uTeam].UpdateTeamFormation(teamTweak[uTeam]);
			//Debug.Log(TeamManager[uTeam].ToString());
		}
	}
	
	
	/*
	 * Me sert à update le placement des joueurs dans les listes
	 * 		- Pour la team qui n'a pas la balle, le replacement des défenseurs est prioritaire
	 * 		- Pour la team ayant le ballon, le replacement des attaquants est prioritaire
	 **/
	void UpdateTeamPlacement(TeamFormation oToUpdate)
	{
	}
}
