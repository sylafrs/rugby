using UnityEngine;
using System.Collections;

public class Limites : MonoBehaviour {
    void OnTriggerEnter(Collider other)
    {
        Ball b = other.GetComponent<Ball>();
        if (b != null)
        {
            // La balle à dépassé les bornes !
            Debug.Log("Hors limites : [Replace au centre]");

            b.transform.position = Vector3.zero;
            b.transform.rotation = Quaternion.identity;
            b.rigidbody.velocity = Vector3.zero;
            b.Owner = null;
            b.transform.parent = null;
            b.rigidbody.useGravity = true;
        }
    }
}
