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

    public override void Entered(Triggered t)
    {
        Ball b = t.GetComponent<Ball>();
        if (b != null)
        {
			if(b.Game.state == Game.State.PLAYING) {
	            Debug.Log(Owner.name + " viens de se prendre un but dans sa face");
	            Owner.nbPoints += b.Game.settings.score.points_drop;
	            b.setPosition(Vector3.zero);
			}
			else if(b.Game.state == Game.State.TRANSFORMATION) {
				TransformationManager tm = b.Game.GetComponent<TransformationManager>();
				tm.But();
			}
        }
    }
}
