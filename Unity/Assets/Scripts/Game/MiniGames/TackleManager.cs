using UnityEngine;
using System;
using System.Collections;

[AddComponentMenu("Scripts/MiniGames/Tackle")]
public class TackleManager: MonoBehaviour {

    public Unit tackled { get; set; }
    public Unit tackler { get; set; }

    private float remainingTime;
    	
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
    public InputTouch touchPassOnTackle;

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


            // Le plaqué a une durée avant de tomber			    => time.timeScale (attention caméra !)
            // Pendant la tombée : QTE => Cut scène peut-être		=> code reusable
            // UI : bouton A (pos tweakable)				        => î

            // QTE :
            // * Si pas appui sur A : Plaquage - Mêlée		=> voilà quoi ^^ 
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
            float ratio = remainingTime / tempsPlaquage;
            float angle = 90 - (ratio * 90);

            Vector3 rot = Vector3.zero;

            if (tackled.Model)
            {
                rot = tackled.Model.transform.localRotation.eulerAngles;
                rot.x = angle;
                tackled.Model.transform.localRotation = Quaternion.Euler(rot);
            }

            /* Pass On Tackle */
            if (
                tackled.Team.Player != null && 
                (
                    Input.GetKeyDown(touchPassOnTackle.keyboard) || 
                    tackled.Team.Player.XboxController.GetButtonDown(touchPassOnTackle.xbox)
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
