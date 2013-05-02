using UnityEngine;
using System.Collections;

public class TackleSystem {
	
	private Unit tackled;
	private Unit tackler;
	private Ball ball;
	
	private float angleOfFOV;
	private float distanceOfTackle;
	
	public TackleSystem()
	{
		Init(null, null, null, 0, 0);
	}
		
	public TackleSystem(Unit tackled)
	{
		Init(tackled, null, null, 0, 0);
	}
			
	public TackleSystem(Unit tackled, Unit tackler)
	{
		Init(tackled, tackler, null, 0, 0);
	}
	
	public TackleSystem(Unit tackled, Unit tackler, Ball b)
	{
		Init(tackled, tackler, b, 0, 0);
	}
	
	public TackleSystem(Unit tackled, Unit tackler, Ball b, float teta)
	{
		Init(tackled, tackler, b, teta, 0);
	}

	public TackleSystem(Unit tackled, Unit tackler, Ball b, float teta, float d)
	{
		Init(tackled, tackler, b, teta, d);
	}
		
	private void Init(Unit tackled, Unit tackler, Ball b, float teta, float d) {
		this.tackled = tackled;
		this.tackler = tackler;
		this.angleOfFOV = teta;
		this.distanceOfTackle = d;
		this.ball = b;
	}
	
	public void Tackle()
	{
		if (canTackle())
		{
			if (IsInRange())
			{
				if (IsCrit())
				{
				//	ball.Owner = tackler;
			//		ball.transform.parent = tackler;                   
				}
				else
				{
					//TODO : Launch CutScene
				/*	if (System.range(0,1) > 0.5f)
					{
						//ball.Owner = tackler;
						//ball.transform.parent = tackler;
					}*/
				}
			}
		}
	}
	
	private bool canTackle()
	{
		return tackled == ball.Owner;
	}
	
	private bool IsCrit()
	{
		return Vector3.Angle(tackled.transform.position - tackler.transform.position, tackler.transform.forward) <= angleOfFOV;
	}
	
	private bool IsInRange()
	{
		return (tackled.transform.position - tackler.transform.position).magnitude <= distanceOfTackle;
	}
	
}
