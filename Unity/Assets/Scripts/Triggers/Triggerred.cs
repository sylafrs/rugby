using UnityEngine;
using System.Collections;

public abstract class Triggered : MonoBehaviour
{
    public virtual void Start()
    {
        if (this.collider == null)
        {
            throw new UnityException("Pas de collider pour cet objet");
        }

        if (this.collider.isTrigger)
        {
            throw new UnityException("Cet objet n'est pourtant pas un trigger !");
        }

        if (this.rigidbody == null)
        {
            // IsKinematic = true && UseGravity = false permet de laisser l'objet 'libre'
            throw new UnityException("Cet objet a besoin d'un rigidbody..");
        }
    }
}

public abstract class TriggeringTriggered : Triggered, Triggering
{

    public virtual void Entered(Triggered o, Trigger t)
    {

    }

    public virtual void Inside(Triggered o, Trigger t)
    {

    }

    public virtual void Left(Triggered o, Trigger t)
    {

    }
}
