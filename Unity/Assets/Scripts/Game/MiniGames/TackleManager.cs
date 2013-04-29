using UnityEngine;
using System;
using System.Collections;

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

    	if (IsCrit())
		{            
            result = RESULT.CRITIC;

            if (callback != null)
                callback(result);              
		}
		else
		{
            remainingTime = tempsPlaquage;
            result = RESULT.QTE;




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
            if (
                tackled.Team.Player != null && 
                (
                    Input.GetKeyDown(touchPassOnTackle.keyboard) || 
                    tackled.Team.Player.XboxController.GetButtonDown(touchPassOnTackle.xbox)
                )
            ){

                /* Effect fall */
                float ratio = remainingTime / tempsPlaquage;
                float angle = (90 - (ratio * 90)) * Mathf.Deg2Rad;

                Vector3 rot = tackled.transform.localRotation.eulerAngles;
                rot.x = angle;
                tackled.transform.localRotation = Quaternion.Euler(rot);

                result = RESULT.PASS;
                if (callback != null)
                {
                    rot.x = 0;
                    tackled.transform.localRotation = Quaternion.Euler(rot);

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
                    if (tackled)
                    {
                        Vector3 rot = tackled.transform.localRotation.eulerAngles;
                        rot.x = 0;
                        tackled.transform.localRotation = Quaternion.Euler(rot);
                    }

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
        float angle = Vector3.Angle(tackled.transform.position - tackler.transform.position, tackler.transform.forward);
        bool supporte = tackled.getNearAlliesNumber() > 0;

		return angle <= tackler.Team.AngleOfFovTackleCrit && !supporte;
	}
}
