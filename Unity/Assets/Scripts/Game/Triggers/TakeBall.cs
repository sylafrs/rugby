using UnityEngine;
using System.Collections;

public class TakeBall : MonoBehaviour {

    public Ball ball;
    void OnTriggerEnter(Collider other)
    {
        // Balle libre
        if (ball.Owner == null)
        {
            // Un joueur passe à côté
            Unit unit = other.GetComponent<Unit>();
            if (unit != null)
            {
                // Il prend (automatiquement) la balle
                Debug.Log(unit.name + " prend la balle");
                ball.Taken(unit);
            }
        }
    }
}
