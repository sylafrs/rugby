using UnityEngine;
using System;
using System.Collections;

[AddComponentMenu("Scripts/MiniGames/Tackle")]
public class TackleManager: myMonoBehaviour {

    public Unit tackled { get; set; }
    public Unit tackler { get; set; }

    private float remainingTime;
    public Game game { get; set; }
    	
    public enum RESULT
    {
        NONE,
        QTE,
        CRITIC,
        PASS,
        NORMAL
    }

    public Action<RESULT> callback;

    public float tempsPlaquage = 1; // Seconds
    public float ralentiPlaquage = 1; // [0 .. 1]

    private RESULT result = RESULT.NONE;

    public void Tackle()
    {
        result = RESULT.NONE;

        if (tackled == null || tackler == null)
        {
            throw new UnityException("Manque tackled ou tackler");
        }

    	if (this.IsCrit())
		{            
            result = RESULT.CRITIC;

            if (callback != null)
                callback(result);              
		}
		else
		{
            remainingTime = tempsPlaquage;
            result = RESULT.QTE;

            tackled.ShowButton("A");


            // Le plaqu� a une dur�e avant de tomber			    => time.timeScale (attention cam�ra !)
            // Pendant la tomb�e : QTE => Cut sc�ne peut-�tre		=> code reusable
            // UI : bouton A (pos tweakable)				        => �

            // QTE :
            // * Si pas appui sur A : Plaquage - M�l�e		=> voil� quoi ^^ 
            // * Sinon : Passe




			//TODO : Launch CutScene
			/*	
                if (System.range(0,1) > 0.5f)
				{
					//ball.Owner = tackler;
					//ball.transform.parent = tackler;
				}
            */
		}		
    }

    public void Update()
    {
        if (result == RESULT.QTE)
        {
            /* Effect fall */
            //float ratio = remainingTime / tempsPlaquage;
            //float angle = 90 - (ratio * 90);

            //Vector3 rot = Vector3.zero;

            //if (tackled.Model)
            //{
            //    rot = tackled.Model.transform.localRotation.eulerAngles;
            //    rot.x = angle;
            //    tackled.Model.transform.localRotation = Quaternion.Euler(rot);
            //}

            /* Pass On Tackle */
            if (
                tackled.Team.Player != null && 
                (
					
                    Input.GetKeyDown(game.settings.Inputs.tackle.keyboard(tackled.Team)) || 
                    tackled.Team.Player.XboxController.GetButtonDown(game.settings.Inputs.tackle.xbox)
                    
                )
            ){
                result = RESULT.PASS;
                if (callback != null)
                {
                    tackled.HideButton();
                    callback(result);
                }
                
                return;
            }

            remainingTime -= Time.deltaTime;            
            if (remainingTime <= 0)
            {
                result = RESULT.NORMAL;
                if (callback != null)
                {
                    tackled.HideButton();
                    callback(result);
                }

                return;
            }
        }
    }

    public void OnGUI()
    {
        if (result == RESULT.QTE)
        {
            GUILayout.Label("Temps pour passer la balle : " + remainingTime.ToString("F2"));
        }
    }
	
	private bool IsCrit()
	{
        //float angle = Vector3.Angle(tackled.transform.position - tackler.transform.position, tackler.transform.forward);
        //bool supporte = tackled.getNearAlliesNumber() > 0;

		//return angle <= tackler.Team.AngleOfFovTackleCrit && !supporte;

        bool superTackler = tackler.Team.Super.SuperActive;
        if (superTackler)
        {
            return tackled.Team.Super.SuperActive;
        }

        return false;
    }
}
