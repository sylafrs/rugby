using UnityEngine;
using System.Collections;

namespace Xft
{
    public class GlowPerObjEvent : CameraEffectEvent
    {
        protected XftGlowPerObj m_glowComp;
        protected bool m_supported = true;
        
        public GlowPerObjEvent (XftEventComponent owner)
            : base(CameraEffectEvent.EType.GlowPerObj, owner)
        {

        }
		
		
        public override void ToggleCameraComponent (bool flag)
        {
            m_glowComp = MyCamera.gameObject.GetComponent<XftGlowPerObj> ();
            if (m_glowComp == null) {
                m_glowComp = MyCamera.gameObject.AddComponent<XftGlowPerObj> ();
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
			ToggleCameraComponent (false);
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
			
			m_glowComp.glowTint = tint;
			m_glowComp.enabled = true;
        }
    }
}

