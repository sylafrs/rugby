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
            Speed = unit.nma.velocity.magnitude;

            bool hasBall = (unit == unit.Game.Ball.Owner);
            HasBall = hasBall;
            if (!hasBall)
            {
                Pass = false;
            }
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

    public bool HasBall
    {
        get
        {
            return animator.GetBool("ball");
        }
        set
        {
            animator.SetBool("ball", value);
        }
    }

    public bool Pass
    {
        get
        {
            return animator.GetBool("pass");
        }
        set
        {
            animator.SetBool("pass", value);
        }
    }

    public bool Touch
    {
        get
        {
            return animator.GetBool("touch");
        }
        set
        {
            animator.SetBool("touch", value);
        }
    }

    public bool BallAtRight
    {
        get
        {
            return animator.GetBool("ballRight");
        }
        set
        {
            animator.SetBool("ballRight", value);
        }
    }
    
    public void OnTouch()
    {
        if (animator)
        {
            Touch = true;
        }
    }

    public void OnTouchAction()
    {
        if (animator)
        {
            Touch = false;
        }
    }

    public void OnPass(bool right)
    {
        this.Pass = true;
        this.BallAtRight = right;
    }
}
