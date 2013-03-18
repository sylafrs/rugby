using UnityEngine;
using System.Collections;

/**
 *  @author Lafon Sylvain
 *  @class boom
 *  @brief Classe de test : ajoute une force à un objet à l'appui d'une touche t
 */
[AddComponentMenu("Test/forceOnKeyDown")]
public class boom : myMonoBehaviour {

    public Vector3 force = Vector3.zero;
    public KeyCode k;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(k))
        {
            this.rigidbody.AddForce(this.transform.forward * force.x + this.transform.up * force.y + this.transform.right * force.z);
        }
	}
}
