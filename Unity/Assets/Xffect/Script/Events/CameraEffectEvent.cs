using UnityEngine;
using System.Collections;

namespace Xft
{
	public class CameraEffectEvent : XftEvent 
	{
		
		public enum EType
		{
			RadialBlur,
			RadialBlurMask,
			Glow,
			GlowPerObj,
			ColorInverse,
		}
		protected EType m_effectType;
		public CameraEffectEvent(EType etype, XftEventComponent owner)
            : base(XEventType.CameraEffect, owner)
        {
			m_effectType = etype;
        }
		
		public virtual void ToggleCameraComponent(bool flag)
		{
			
		}
		
		
	}
}

