using UnityEngine;
using System.Collections;

/**
 * @class Ball
 * @brief Composant faisant de l'objet, une balle de rugby utilisable.
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[AddComponentMenu("Scripts/Game/Ball"), RequireComponent(typeof(Rigidbody))]
public class Ball : TriggeringTriggered {
    public Game Game;
	public Vector3 multiplierDrop = new Vector3(50.0f, 70.0f, 0.0f);
	public Vector3 multiplierPass = new Vector3(20.0f, 70.0f, 20.0f);

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
	 */
    public void Drop()
    {              
        this.transform.parent = null;
        this.rigidbody.useGravity = true;
        this.rigidbody.AddForce(Owner.transform.forward * multiplierDrop.x + Owner.transform.up * multiplierDrop.y + Owner.transform.right * multiplierDrop.z);
        Owner = null;
    }

	//Passe
	public void Pass(Vector3 direction, float pressionCapture = 1.0f)
	{
		Debug.Log("On Pass pression : " + pressionCapture + " direction : " + direction);
		this.transform.parent = null;
		this.rigidbody.useGravity = true;
		this.rigidbody.AddForce(new Vector3(direction.x * multiplierPass.x * pressionCapture, direction.y * multiplierPass.y * pressionCapture, direction.z * multiplierPass.z * pressionCapture));
		Owner = null;
	}

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
