using UnityEngine;
using System.Collections;

public abstract class Trigger : MonoBehaviour
{
    public Triggering triggering;
    
    public virtual void Start()
    {
        if (this.triggering == null)
        {
            throw new UnityException("Triggering manquant");
        }

        if (this.collider == null)
        {
            throw new UnityException("Pas de collider pour ce trigger");
        }

        if (!this.collider.isTrigger)
        {
            throw new UnityException("Cet objet est un trigger !");
        }
    }

    void OnTriggerEnter(Collider otherOne)
    {
        if (triggering == null)
            return;

        Triggered t = otherOne.GetComponent<Triggered>();
        if (t != null)
        {            
            triggering.Entered(t, this);
        }
    }

    void OnTriggerExit(Collider otherOne)
    {
        if (triggering == null)
            return;

        Triggered t = otherOne.GetComponent<Triggered>();
        if (t != null)
        {
            triggering.Left(t, this);
        }
    }

    void OnTriggerStay(Collider otherOne)
    {
        if (triggering == null)
            return;

        Triggered t = otherOne.GetComponent<Triggered>();
        if (t != null)
        {
            triggering.Inside(t, this);
        }
    }
}

public abstract class TriggeringTrigger : Trigger, Triggering
{
    public virtual void Entered(Triggered o)
    {

    }

    public virtual void Inside(Triggered o)
    {

    }

    public virtual void Left(Triggered o)
    {

    }

    public virtual void Entered(Triggered o, Trigger t)
    {
        if (t == this) Entered(o);
    }

    public virtual void Inside(Triggered o, Trigger t)
    {
        if (t == this) Inside(o);
    }

    public virtual void Left(Triggered o, Trigger t)
    {
        if (t == this) Left(o);
    }

    public override void Start()
    {
        this.triggering = this;
        base.Start();
    }
}