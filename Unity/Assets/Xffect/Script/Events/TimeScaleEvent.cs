using UnityEngine;
using System.Collections;

namespace Xft
{
    public class TimeScaleEvent : XftEvent
    {
        public TimeScaleEvent (XftEventComponent owner)
         : base(XEventType.TimeScale, owner)
        {
        }

			
        public override void Reset ()
        {
			base.Reset();
            Time.timeScale = 1f;
        }
  
		
		public override void OnBegin ()
		{
			base.OnBegin ();
			Time.timeScale = m_owner.TimeScale;
		}

        public override void Update (float deltaTime)
        {
            m_elapsedTime += deltaTime;
			
			float elapsed = m_elapsedTime; /*- m_owner.StartTime * m_owner.TimeScale*/
			
			//Debug.LogWarning(elapsed);
			
			if (elapsed / m_owner.TimeScale > m_owner.TimeScaleDuration)
			{
				Time.timeScale = 1f;
			}

        }
    }
}

