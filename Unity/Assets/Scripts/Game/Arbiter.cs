using UnityEngine;
using System.Collections.Generic;

/**
  * @class Arbiter
  * @brief The arbiter watch the game and apply rules.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class Arbiter : MonoBehaviour {
		
	public Game Game {get;set;}
	
	public bool ToucheRemiseAuCentre = false;
	
	public void OnTouch(Touche t) {
		if(t == null) {
			return;	
		}		
		
		if (t.a == null || t.b == null)
        {
            Debug.Log("Touche : [Replace au centre]");
            Game.Ball.setPosition(Vector3.zero);
        }
        else
        {
            Debug.Log("Touche : [Replace au centre, sur la ligne]");

            Vector3 pos = Vector3.Project(Game.Ball.transform.position - t.a.position, t.b.position - t.a.position) + t.a.position;
            pos.x = ToucheRemiseAuCentre ? 0 : pos.y; // Au centre
            pos.y = 0; // A terre
            Game.Ball.setPosition(pos);
        }           
	}
	
	public void OnScrum() {
		
	}
	
	public void OnTransformation() {
		
	}
	
	public void OnTackle() {
		
	}
	
	public void OnBut() {
		
	}
}
