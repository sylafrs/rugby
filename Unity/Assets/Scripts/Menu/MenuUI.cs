using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Menu/UI")]
public class MenuUI : myMonoBehaviour {

	void OnGUI()
    {
		
		// int offset		 = 100;
		
		//a box for the button to launch the game
		Rect buttonBox = GameUIManager.screenRelativeRect(50- 40/2, 
		50 + 10/2, 
		40, 10);
		
		//title
		Rect titleBox = GameUIManager.screenRelativeRect(50- 40/2, 
		0 + 10/2, 
		40, 10);
		
		//patch note
		Rect patchNoteBox = GameUIManager.screenRelativeRect(50- 40/2, 
		0 + 40/2, 
		40, 40);
		
		
		GUI.Label(titleBox,  "Roar Raging Rugby");
		GUI.Label(patchNoteBox,  "Notes :\n" +
			"- Super Available !\n" +
			"- New In Game UI !\n" +
			"- New super particles !");
		
		if(GUI.Button(buttonBox,"Launch")){
				Application.LoadLevel("terrain");
		}
	}
}
