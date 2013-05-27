using UnityEngine;
using System.Collections.Generic;

/**
  * @class UnitAnimator
  * @brief Description.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Scripts/Animations/Unit Animator (require Unit)"), RequireComponent(typeof(Unit))]
public class UnitAnimator : myMonoBehaviour {

    private Unit unit;
    public Animator animator;

    public bool Tackled
    {
        get
        {
            return animator.GetBool("tackled"); 
        }

        set
        {
            animator.SetBool("tackled", value); 
        }
    }

    public float Speed
    {
        get
        {
            return animator.GetFloat("speed");
        }

        set
        {
            animator.SetFloat("speed", value);
        }
    }

    public void Start()
    {
        unit = this.GetComponent<Unit>();
        if (unit == null)
        {
            throw new UnityException("I need a unit");
        }
    }

    public void Update()
    {
        if (animator)
        {
            if (unit.game.UseFlorianIA)
            {
                this.Speed = unit.nma.speed;
                if (unit.Order.type == Order.TYPE.NOTHING)
                {
                    this.Speed = 0;
                }
            }
            else
            {
                this.Speed = unit.nma.desiredVelocity.magnitude;
            }
        }
    }
}
