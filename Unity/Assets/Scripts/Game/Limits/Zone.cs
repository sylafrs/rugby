using UnityEngine;
using System.Collections;

/**
 * @class Game
 * @brief Trigger des zones d'essai
 * @author Sylvain Lafon
 */
[AddComponentMenu("Triggers/Zone")]
public class Zone : TriggeringTrigger {

    private Team owner;
    public Team Owner
    {
        get
        {
            return owner;
        }
        set
        {
            owner = value;
        }
    }

    public override void Inside(Triggered o)
    {
        Ball b = o.GetComponent<Ball>();
        if (b != null)
        {
            if (b.Owner != null)
            {
                if (b.Owner.Team != this.Owner)
                {
                    Debug.Log("Essai de la part des " + b.Owner.Team.Name + " !");
                    b.Owner.Team.nbPoints += b.Game.settings.score.points_essai;
                    b.setPosition(Vector3.zero);

                    b.Game.right.initPos();
                    b.Game.left.initPos();
                }
            }
        }
    }

}
