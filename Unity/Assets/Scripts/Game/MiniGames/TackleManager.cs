using UnityEngine;
using System;
using System.Collections;

public class TackleManager: MonoBehaviour {

    public Unit tackled { get; set; }
    public Unit tackler { get; set; }
    	
    public enum RESULT
    {
        NONE,
        QTE,
        CRITIC,
        PASS,
        NORMAL
    }

    public Action<RESULT> callback;

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
            // Le plaqueur récupère la balle instanéement => Cut Scène	
            result = RESULT.CRITIC;

            if (callback != null)
                callback(result);              
		}
		else
		{
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
        if (result != RESULT.QTE)
        {
            result = RESULT.NONE;
        }
    }
	
	private bool IsCrit()
	{
        float angle = Vector3.Angle(tackled.transform.position - tackler.transform.position, tackler.transform.forward);
        bool supporte = tackled.getNearAlliesNumber() > 0;

		return angle <= tackler.Team.AngleOfFovTackleCrit && !supporte;
	}
}
