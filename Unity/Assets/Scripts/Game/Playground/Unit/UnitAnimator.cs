using UnityEngine;
using System.Collections.Generic;

/**
  * @class UnitAnimator
  * @brief Description.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Scripts/Animations/Unit Animator (require Unit)"), RequireComponent(typeof(Unit))]
public class UnitAnimator : myMonoBehaviour
{
    private Unit unit;
    public Animator animator;
    	
    public float Speed
    {
        get
        {
            return animator.GetFloat("in_speed");
        }
        set
        {
            animator.SetFloat("in_speed", value);
        }
    }

    public bool Tackled
    {
        get
        {
            return animator.GetBool("in_tackled");
        }
        set
        {
            animator.SetBool("in_tackled", value);
        }
    }

    public bool HasBall
    {
        get
        {
            return animator.GetBool("in_ball");
        }
        set
        {
            animator.SetBool("in_ball", value);
        }
    }

    public bool Pass
    {
        get
        {
            return animator.GetBool("in_pass");
        }
        set
        {
            animator.SetBool("in_pass", value);
        }
    }

    public bool Touch
    {
        get
        {
            return animator.GetBool("in_touch");
        }
        set
        {
            animator.SetBool("in_touch", value);
        }
    }

    public bool BallAtRight
    {
        get
        {
            return animator.GetBool("in_ballRight");
        }
        set
        {
            animator.SetBool("in_ballRight", value);
        }
    }

    public int DELAY_SPEED
    {
        get
        {
            return (int)animator.GetInteger("out_delayPass");
        }
    }

    public int DELAY_PASS
    {
        get
        {
            return (int)animator.GetInteger("out_delayPass");
        }
    }
    
    public void OnTouch()
    {
        if (animator)
        {
            Touch = true;
        }
    }
    
    private int delayStop;
    private int delayPass;

    public void Start()
    {
        unit = this.GetComponent<Unit>();
        if (unit == null)
        {
            throw new UnityException("I need a unit");
        }

        delayStop = DELAY_SPEED;
        delayPass = DELAY_PASS;
    }

    public void Update()
    {
        if (animator)
        {
			float s = unit.nma.desiredVelocity.magnitude;
			if (delayStop > 0 && s == 0)
			{
				delayStop--;
			}
			else if (s != 0)
			{
				this.Speed = s;
				delayStop = DELAY_SPEED;
			}
			else
			{
				this.Speed = 0;
			}
			
			bool hasBall = (unit == unit.game.Ball.Owner);
            HasBall = hasBall;
            
            if (Pass && !hasBall)
            {
                this.delayPass--;
                if (this.delayPass <= 0)
                {
                    Pass = false;
                }
            }
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
        this.delayPass = DELAY_PASS;
        this.Pass = true;
        this.BallAtRight = right;
    }

    public void OnTacklePass()
    {
    }
}
