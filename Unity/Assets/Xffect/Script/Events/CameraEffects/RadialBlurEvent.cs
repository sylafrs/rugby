using UnityEngine;
using System.Collections;


namespace Xft
{
	 public class RadialBlurEvent : CameraEffectEvent
    {
        protected XftRadialBlur m_radialBlurComp;
		protected bool m_supported = true;

        public RadialBlurEvent(XftEventComponent owner)
            : base(CameraEffectEvent.EType.RadialBlur, owner)
        {

        }
		
		
        public override void ToggleCameraComponent(bool flag)
        {
            m_radialBlurComp = MyCamera.gameObject.GetComponent<XftRadialBlur>();
            if (m_radialBlurComp == null)
            {
                m_radialBlurComp = MyCamera.gameObject.AddComponent<XftRadialBlur>();
            }
			m_radialBlurComp.Init(m_owner.RadialBlurShader);
            m_radialBlurComp.enabled = flag;
        }

        public override void Initialize()
        {
            ToggleCameraComponent(false);
			m_supported = m_radialBlurComp.CheckSupport();
			if (!m_supported)
				Debug.LogWarning("can't support Image Effect: Radial Blur on this device!");
        }

        public override void Reset()
        {
			base.Reset();
            m_radialBlurComp.enabled = false;
			ResetMyCamera();
        }
		
		
		
        public override void Update(float deltaTime)
        {
			if (!m_supported)
			{
				m_owner.enabled = false;
				return;
			}
			
			
			m_elapsedTime += deltaTime;
			m_radialBlurComp.enabled = true;
			Vector3 pos = MyCamera.WorldToScreenPoint(m_owner.RadialBlurObj.position);
			m_radialBlurComp.Center = pos;
			
			float strength = 0f;
			if (m_owner.RBStrengthType == MAGTYPE.Fixed)
				strength = m_owner.RBSampleStrength;
			else
				strength = m_owner.RBSampleStrengthCurve.Evaluate(m_elapsedTime);
			Mathf.Clamp(strength,0.05f,99f);
			m_radialBlurComp.SampleStrength = strength;
			m_radialBlurComp.SampleDist = m_owner.RBSampleDist;
        }
    }
}

