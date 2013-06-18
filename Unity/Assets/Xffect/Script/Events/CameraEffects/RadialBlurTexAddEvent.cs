using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Xft
{
    public class RadialBlurTexAddEvent : CameraEffectEvent
    {
        protected XftRadialBlurTexAdd m_radialBlurComp;
        protected bool m_supported = true;
        public RadialBlurTexAddEvent (XftEventComponent owner)
            : base(CameraEffectEvent.EType.RadialBlurMask, owner)
        {

        }
		
		
        public override void ToggleCameraComponent (bool flag)
        {
            m_radialBlurComp = MyCamera.gameObject.GetComponent<XftRadialBlurTexAdd> ();
            if (m_radialBlurComp == null) {
                m_radialBlurComp = MyCamera.gameObject.AddComponent<XftRadialBlurTexAdd> ();
            }
            m_radialBlurComp.Init (m_owner.RadialBlurTexAddShader);
            m_radialBlurComp.enabled = flag;
        }

        public override void Initialize ()
        {
            ToggleCameraComponent (false);
            m_supported = m_radialBlurComp.CheckSupport ();
            if (!m_supported)
                Debug.LogWarning ("can't support Image Effect: Radial Blur Mask on this device!");
        }

        public override void Reset ()
        {
			base.Reset();
            m_radialBlurComp.enabled = false;
            m_elapsedTime = 0f;
			ResetMyCamera();
        }

        public override void Update (float deltaTime)
        {
            if (!m_supported) {
                m_owner.enabled = false;
                return;
            }
            m_elapsedTime += deltaTime;
			
			
			m_radialBlurComp.enabled = true;
			m_radialBlurComp.SampleDist = m_owner.RBMaskSampleDist;
			float strength = 1f;
			if (m_owner.RBMaskStrengthType == MAGTYPE.Fixed)
				strength = m_owner.RBMaskSampleStrength;
			else
				strength = m_owner.RBMaskSampleStrengthCurve.Evaluate(m_elapsedTime);
			m_radialBlurComp.SampleStrength = strength;
			m_radialBlurComp.Mask = m_owner.RadialBlurMask;
		}
    }

}