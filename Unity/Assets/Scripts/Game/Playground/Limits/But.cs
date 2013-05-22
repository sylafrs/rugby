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
    private Team _Owner;
    public Team Owner {
        get {
            return _Owner;
        }
        set {
            _Owner = value;
        }
    }
	
	public Transform transformationPoint;

    public override void Left(Triggered t)
    {
        Ball b = t.GetComponent<Ball>();
        if (b != null)
        {			
			b.Game.OnConversion(this);
        }
    }
}
