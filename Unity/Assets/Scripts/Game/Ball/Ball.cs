using UnityEngine;
using System.Collections;

/**
 * @class Ball
 * @brief Composant faisant de l'objet, une balle de rugby utilisable.
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/Game/Ball"), RequireComponent(typeof(Rigidbody))]
public class Ball : TriggeringTriggered {
    public Game Game;

    private Unit _owner;
    public Unit Owner
    {
        get
        {
            return _owner;
        }
        set
        {
            if (_owner != value)
            {
                _owner = value;
                Game.OwnerChanged(_owner, value);
            }
            else
            {
                _owner = value;
            }
        }
    }
   
    public void Update()
    {
        if (Owner != null)
        {
            this.transform.position = Owner.BallPlaceHolder.transform.position;
            this.transform.localRotation = Quaternion.identity;
        }       
    }
  
    public void ShootTarget(Unit u)
    {        
        //Vector3 pos = u.transform.position;        

        this.transform.parent = null;
        this.rigidbody.useGravity = true;
        this.rigidbody.isKinematic = false;

        Unit prev = Owner;
        Owner = null;

        this.rigidbody.AddForce(prev.transform.forward * 50 + prev.transform.up * 70);
    }

    public void OwnerPlaque()
    {
        setPosition(this.transform.position);    
    }

    public void Taken(Unit u)
    {
        this.rigidbody.useGravity = false;        
        this.rigidbody.velocity = Vector3.zero;
        this.rigidbody.isKinematic = true;
        this.transform.parent = u.BallPlaceHolder.transform;
        this.transform.localPosition = Vector3.zero;

        if (Owner != u)
        {
            Owner = u;            
        }
    }

    public void setPosition(Vector3 v)
    {
        this.transform.parent = null;
        this.transform.position = v;
        this.rigidbody.useGravity = true;
        this.rigidbody.isKinematic = false;
        this.rigidbody.velocity = Vector3.zero;       
        this.transform.rotation = Quaternion.identity;
        this.Owner = null;         
    }

    public override void Entered(Triggered o, Trigger t)
    {
        Unit u = o.GetComponent<Unit>();
        if (u != null)
        {
            u.sm.event_NearBall();
        }
    }
}
