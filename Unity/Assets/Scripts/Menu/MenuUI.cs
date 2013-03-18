using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour {

	void OnGUI()
    {
		
		int offset		 = 100;
		
		//a box for the button to launch the game
		Rect buttonBox = gameUIManager.screenRelativeRect(50- 40/2, 
		50 + 10/2, 
		40, 10);
		
		//title
		Rect titleBox = gameUIManager.screenRelativeRect(50- 40/2, 
		0 + 10/2, 
		40, 10);
		
		//patch note
		Rect patchNoteBox = gameUIManager.screenRelativeRect(50- 40/2, 
		0 + 40/2, 
		40, 40);
		
		
		GUI.Label(titleBox,  "Roar Raging Rugby");
		GUI.Label(patchNoteBox,  "Notes :\n" +
			"-Super Available !\n" +
			"-Super Available !\n" +
			"-Super Available !\n" +
			"-Super Available !");
		
		if(GUI.Button(buttonBox,"Launch")){
				Application.LoadLevel("terrain");
		}
	}
}
