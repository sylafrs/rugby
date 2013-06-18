using UnityEngine;
using System.Collections;

namespace Xft
{
    public class ColorInverseEvent : CameraEffectEvent
    {
        protected XftColorInverse m_inverseComp;
        protected bool m_supported = true;
		
		
        public ColorInverseEvent (XftEventComponent owner)
            : base(CameraEffectEvent.EType.ColorInverse, owner)
        {

        }
        public override void ToggleCameraComponent (bool flag)
        {
            m_inverseComp = MyCamera.gameObject.GetComponent<XftColorInverse> ();
            if (m_inverseComp == null) {
                m_inverseComp = MyCamera.gameObject.AddComponent<XftColorInverse> ();
            }
            m_inverseComp.Init (m_owner.ColorInverseShader);
            m_inverseComp.enabled = flag;
        }

        public override void Initialize ()
        {
            ToggleCameraComponent (false);
            m_supported = m_inverseComp.CheckSupport ();
            if (!m_supported)
                Debug.LogWarning ("can't support Image Effect: ColorInverse on this device!");
        }

        public override void Reset ()
        {
			base.Reset();
            m_inverseComp.enabled = false;
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
			float strength = m_owner.CIStrengthCurve.Evaluate(m_elapsedTime);
			m_inverseComp.Strength = strength;
			m_inverseComp.enabled = true;
        }
    }
}

