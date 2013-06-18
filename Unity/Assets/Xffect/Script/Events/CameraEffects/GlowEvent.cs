using UnityEngine;
using System.Collections;

namespace Xft
{
    public class GlowEvent : CameraEffectEvent
    {
        protected XftGlow m_glowComp;
        protected bool m_supported = true;
        
        public GlowEvent (XftEventComponent owner)
            : base(CameraEffectEvent.EType.Glow, owner)
        {

        }
		
		
        public override void ToggleCameraComponent (bool flag)
        {
            m_glowComp = MyCamera.gameObject.GetComponent<XftGlow> ();
            if (m_glowComp == null) {
                m_glowComp = MyCamera.gameObject.AddComponent<XftGlow> ();
            }
            m_glowComp.Init (m_owner);
            m_glowComp.enabled = flag;
        }

        public override void Initialize ()
        {
            ToggleCameraComponent (false);
            m_elapsedTime = 0f;
            m_supported = m_glowComp.CheckSupport ();
            if (!m_supported)
                Debug.LogWarning ("can't support Image Effect: Glow on this device!");
        }

		
        public override void Reset ()
        {
			base.Reset();
            m_glowComp.enabled = false;
			ResetMyCamera();
        }
     
		
        public override void Update (float deltaTime)
        {
            if (!m_supported) {
                m_owner.enabled = false;
                return;
            }
			
			
            m_elapsedTime += deltaTime;
			
			float t = 0f;
			Color tint = Color.clear;
			
			t = m_owner.ColorCurve.Evaluate (m_elapsedTime);
			tint = Color.Lerp (m_owner.GlowColorStart, m_owner.GlowColorEnd, t);

			
			//deprecated, only support curve now.
			/*else
			{
				t = (m_elapsedTime - m_owner.StartTime) / m_owner.GlowColorGradualTime;
				if (t <= 1)
				{
					tint = Color.Lerp (m_owner.GlowColorStart, m_owner.GlowColorEnd, t);
				} 
				else 
				{
					if (m_owner.GlowColorGradualType == COLOR_GRADUAL_TYPE.CLAMP) 
					{
						
					} 
					else if (m_owner.GlowColorGradualType == COLOR_GRADUAL_TYPE.LOOP) 
					{
						tint = Color.Lerp (m_owner.GlowColorStart, m_owner.GlowColorEnd, t - 1);
					} 
					else 
					{
						tint = Color.Lerp (m_owner.GlowColorEnd, m_owner.GlowColorStart, t - 1);
					}
				}
			}*/
			m_glowComp.glowTint = tint;
			m_glowComp.enabled = true;
        }
    }
}

