using UnityEngine;
using System.Collections;

public class TouchUI{
	
	private Game game;
	private TouchManager manager;
	
	public TouchUI(Game _game)
	{
		game = _game;
        manager = _game.refs.managers.touch;
	}
	
	public void DrawUI()
	{
		Color c = GUI.color;
		
		GUILayout.Space(300);
				
		if(manager.timeLeft > 0 && !manager.infiniteTime)
			GUILayout.Label("Choisissez une touche, il vous reste : " + ((int)manager.timeLeft) + " secondes (minimum)");
		else
			GUILayout.Label("Choisissez une touche");
			
		GUILayout.BeginHorizontal();
			GUILayout.Label("J1 : ");
		
			for(int i = 0; i < manager.n; i++) {
				GUI.color = (manager.choixTouche == i+1) ? Color.red : c;
				if(GUILayout.Button (game.settings.Inputs.touch[i].xbox.ToString(), GUILayout.MinWidth(100))) {
					manager.choixTouche = i+1;
				}
			}
		GUILayout.EndHorizontal();
		
		GUI.color = c;
		
		GUILayout.BeginHorizontal();
			GUILayout.Label("J2 : ");
		
			for(int i = 0; i < manager.n; i++) {
				GUI.color = (manager.choixInter == i+1) ? Color.red : c;
				if(GUILayout.Button (game.settings.Inputs.interception[i].xbox.ToString(), GUILayout.MinWidth(100))) {
					manager.choixInter = i+1;
				}
			}
		GUILayout.EndHorizontal();
		
		GUI.color = c;
	}
}
