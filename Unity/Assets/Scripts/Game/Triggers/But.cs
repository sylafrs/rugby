using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class But : MonoBehaviour {
    private Team _Owner;
    public Team Owner {
        get {
            return _Owner;
        }
        set {
            _Owner = value;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(Owner.name + " viens de se prendre un but dans sa face");
        Owner.nbPoints += GameSettings.settings.score.points_drop;
    }
}
