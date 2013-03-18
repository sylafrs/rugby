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

    public override void Entered(Triggered t)
    {
        Ball b = t.GetComponent<Ball>();
        if (b != null)
        {
            Debug.Log(Owner.name + " viens de se prendre un but dans sa face");
            Owner.nbPoints += b.Game.settings.score.points_drop;
            b.setPosition(Vector3.zero);
        }
    }
}
