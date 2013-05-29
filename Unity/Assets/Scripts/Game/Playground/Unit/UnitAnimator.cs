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
    public Unit unit { get; private set; }
    public Animator animator;

    public const string IdleState = "Idle";
    public const string BallIdleState = "IdleBall";

    public const string lblSpeed = "in_float_speed";
    public const string lblTackled = "in_bool_tackled";
    public const string lblTackling = "in_bool_tackling";
    public const string lblTackleFail = "in_bool_tackleFail";
    public const string lblBall = "in_bool_ball";
    public const string lblPass = "in_bool_pass";
    public const string lblTouch = "in_bool_touch";
    public const string lblPut = "in_bool_put";
    public const string lblDrop = "in_bool_drop";    
    public const string lblBallRight = "in_bool_ballRight";
    public const string lblDelay = "out_int_delaySpeed";
    public const string lblSuper = "in_bool_super";

    public bool Tackling
    {
        get
        {
            return animator.GetBool(lblTackling);
        }
        set
        {
            animator.SetBool(lblTackling, value);
        }
    }

    public bool TackleFail
    {
        get
        {
            return animator.GetBool(lblTackleFail);
        }
        set
        {
            animator.SetBool(lblTackleFail, value);
        }
    }
	
    public float Speed
    {
        get
        {
            return animator.GetFloat(lblSpeed);
        }
        set
        {
            animator.SetFloat(lblSpeed, value);
        }
    }

    public bool Tackled
    {
        get
        {
            return animator.GetBool(lblTackled);
        }
        set
        {
            animator.SetBool(lblTackled, value);
        }
    }

    public bool HasBall
    {
        get
        {
            return animator.GetBool(lblBall);
        }
        set
        {
            animator.SetBool(lblBall, value);
        }
    }

    public bool Pass
    {
        get
        {
            return animator.GetBool(lblPass);
        }
        set
        {
            animator.SetBool(lblPass, value);
        }
    }

    public bool Touch
    {
        get
        {
            return animator.GetBool(lblTouch);
        }
        set
        {
            animator.SetBool(lblTouch, value);
        }
    }

    public bool Put
    {
        get
        {
            return animator.GetBool(lblPut);
        }
        set
        {
            animator.SetBool(lblPut, value);
        }
    }

    public bool Drop
    {
        get
        {
            return animator.GetBool(lblDrop);
        }
        set
        {
            animator.SetBool(lblDrop, value);
        }
    }

    public bool BallAtRight
    {
        get
        {
            return animator.GetBool(lblBallRight);
        }
        set
        {
            animator.SetBool(lblBallRight, value);
        }
    }
	
	public bool Super {
		get
        {
            return animator.GetBool(lblSuper);
        }
        set
        {
            animator.SetBool(lblSuper, value);
        }
	}

    public int DELAY_SPEED
    {
        get
        {
            return (int)animator.GetInteger(lblDelay);
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
    private bool launchUpdate;
    private bool launchSuper;

    private string LayerName;

    public bool isInState(string state)
    {
        return this.animator.GetCurrentAnimatorStateInfo(0).IsName(LayerName + "." + state);
    }

    public void Start()
    {
        unit = this.GetComponent<Unit>();
        if (unit == null)
        {
            throw new UnityException("I need a unit");
        }

        LayerName = animator.GetLayerName(0);

        delayStop = DELAY_SPEED;
        launchUpdate = false;
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

            if (!launchUpdate)
            {
                if (Pass && !hasBall)
                {
                    Pass = false;
                }
            
                if (Put)
                {
                    Put = false;
                }

                if (Drop && !hasBall)
                {
                    Drop = false;
                }

                if (Super && launchSuper)
                {
                    Super = false;
                    launchSuper = false;
                }
            }

            launchUpdate = false;
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
        launchUpdate = true;
        this.Pass = true;
        this.BallAtRight = right;
    }

    public void OnPut()
    {
        launchUpdate = true;
        this.Put = true;
    }

    public void OnDrop()
    {
        launchUpdate = true;
        this.Drop = true;
    }
	
	public void PrepareSuper() {
		Super = true;
	}
	
	public void LaunchSuper() {
		if(!Super) {
			Debug.LogWarning("UnitAnimator : Super was not prepared");	
		}
        launchUpdate = true;
		launchSuper = true;	
	}

    public void OnTacklePass()
    {
    }

    public void OnTackleStart(bool success)
    {
        this.TackleFail = !success;
        this.Tackling = true;        
    }
}
