using UnityEngine;
using System.Collections.Generic;
using FuncBool = System.Func<bool>;

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

    public const string IdleState = "WithoutBall.Idle";
    public const string BallIdleState = "WithBall.IdleBall";
    public const string SuperState = "Super.Super";
    
    public const string lblSpeed = "in_float_speed";
    public const string lblTackled = "in_bool_tackled";
    public const string lblTackling = "in_bool_tackling";
    public const string lblTackleFail = "in_bool_tackleFail";
    public const string lblBeingTackled = "in_bool_beingtackled";
    public const string lblBall = "in_bool_ball";
    public const string lblPass = "in_bool_pass";
    public const string lblTouch = "in_bool_touch";
    public const string lblPut = "in_bool_put";
    public const string lblDrop = "in_bool_drop";    
    public const string lblBallRight = "in_bool_ballRight";
    public const string lblDelay = "out_int_delaySpeed";
    public const string lblSuper = "in_bool_super";
    public const string lblFxTime = "out_float_fxSuperTime";
    public const string lblDodge = "in_bool_dodge";
	public const string lblDodgeRight = "in_bool_dodge_right";

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

    public bool BeingTackled
    {
        get
        {
            return animator.GetBool(lblBeingTackled);
        }
        set
        {
            animator.SetBool(lblBeingTackled, value);
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

    public bool Dodge
    {
        get
        {
            return animator.GetBool(lblDodge);
        }
        set
        {
            animator.SetBool(lblDodge, value);
        }
    }
	
	public bool DodgeRight
    {
        get
        {
            return animator.GetBool(lblDodgeRight);
        }
        set
        {
            animator.SetBool(lblDodgeRight, value);
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

    public float TIME_SUPER_FX
    {
        get
        {
            return animator.GetFloat(lblFxTime);
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

    private class MyEvent
    {
        public string name;
        public FuncBool callback;
        public string state;
        public float time;
    }

    List<MyEvent> events = new List<MyEvent>();

    private MyEvent GetEventByName(string name)
    {
        foreach (MyEvent e in events)
        {
            if (e.name.Equals(name))
            {
                return e;
            }
        }

        return null;
    }

    public bool AddEvent(string name, FuncBool action, string state, float time)
    {
        if (GetEventByName(name) != null)
        {
            return false;
        }

        MyEvent e = new MyEvent();
        e.callback = action;
        e.state = state;
        e.time = time;
        e.name = name;
        events.Add(e);

        return true;
    }

    public AnimatorStateInfo? GetStateInfo()
    {
        if (this.animator == null)
            return null;

        return this.animator.GetCurrentAnimatorStateInfo(0);
    }

    public float GetTime()
    {
        AnimatorStateInfo? a = this.GetStateInfo();
        if(a == null)
            return -1;
        return ((AnimatorStateInfo)a).normalizedTime;
    }

    public bool isInState(string state)
    {
        AnimatorStateInfo? a = this.GetStateInfo();
        if (a == null)
            return false;

        return ((AnimatorStateInfo)a).IsName(state);
    }

    public void Start()
    {
        unit = this.GetComponent<Unit>();
        if (unit == null)
        {
            throw new UnityException("I need a unit");
        }

        delayStop = DELAY_SPEED;
        launchUpdate = false;
    }

    private void UpdateEvent()
    {
        int n = events.Count;
        for (int i = 0; i < n; i++)
        {
            MyEvent e = events[i];
            AnimatorStateInfo infos = (AnimatorStateInfo)this.GetStateInfo();
            
            if (Mathf.Abs(infos.normalizedTime - e.time) < 0.05 && infos.IsName(e.state))
            {
                if (e.callback())
                {
                    events.RemoveAt(i);
                    n--;
                }
                else
                {
                    i++;
                }
            }
        }
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

                if (Dodge)
                {
                    Dodge = false;                    
                }

                if (BeingTackled)
                {
                    BeingTackled = false;
                }
            }

            launchUpdate = false;

            UpdateEvent();
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

    public void OnDodge()
    {
        this.unit.transform.forward = this.unit.Team.transform.forward;
        this.unit.nma.angularSpeed = 0;
        Timer.AddTimer(1, () =>
        {            
            this.unit.nma.angularSpeed = 5000;
        });

        launchUpdate = true;
        Dodge = true;
		
		if(this.unit.game.northTeam == this.unit.Team)
			DodgeRight = unit.Order.point.x < 0;
		else
			DodgeRight = unit.Order.point.x > 0;
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

    public void OnBeingTackled()
    {
        this.BeingTackled = true;
        this.launchUpdate = true;
    }
}
