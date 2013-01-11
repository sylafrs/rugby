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
        if (t.GetComponent<Ball>())
        {
            Debug.Log(Owner.name + " viens de se prendre un but dans sa face");
            Owner.nbPoints += GameSettings.settings.score.points_drop;
        }
    }
}
