using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Ball"), RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour {
    public Unit Owner;

  
    public void ShootTarget(Unit u)
    {
        Unit shooter = Owner;

        Vector3 pos = u.transform.position;
      //  Owner = null;

        this.transform.parent = null;

        this.rigidbody.useGravity = true;
        this.rigidbody.AddForce(shooter.transform.forward * 50 + shooter.transform.up * 70);

        //Camera.mainCamera.GetComponent<rotateMe>().rotate(new Vector3(0, 1, 0), 180);
    }
}
