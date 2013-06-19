using UnityEngine;
using System.Collections;

/**
 * @class But
 * @brief Trigger entre les deux potaux
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Game/But")]
public class But : TriggeringTrigger
{
	[HideInInspector]
    public Team Owner;
	
	public Transform transformationPoint;
    public Renderer model;

    public override void Left(Triggered t)
    {
        Ball b = t.GetComponent<Ball>();
        if (b != null){			
			b.Game.OnConversion(this);
        }
    }
}
