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
  
	//Drop
	/**
	 * TODO
	 * Passer un vector3 résultant du capteur de pression en paramètre
	 * et dotProduct avec Owner.transform
	 */
    public void Drop()
    {              
        this.transform.parent = null;
        this.rigidbody.useGravity = true;
        this.rigidbody.AddForce(Owner.transform.forward * 50 + Owner.transform.up * 70);
        Owner = null;
    }

	//Passe

	//Poser la balle
    public void Put()
    {
        setPosition(this.transform.position);    
    }

    public void Taken(Unit u)
    {
        this.rigidbody.useGravity = false;
        this.rigidbody.velocity = Vector3.zero;
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
