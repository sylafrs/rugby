using UnityEngine;
using System.Collections;

namespace Xft
{
    public class XftEvent
    {
        protected XEventType m_type;
        protected XftEventComponent m_owner;
        protected float m_elapsedTime = 0f;
		
		protected bool m_canUpdate = false;
		
		
		protected Camera m_myCamera;
		
		
		public Camera MyCamera
		{
			get{
				if (m_myCamera == null)
				{
					m_myCamera = FindMyCamera();
				}
				
				return m_myCamera;
			}
		}
		
		//in case scene changed.
		public void ResetMyCamera()
		{
			if (m_myCamera == null)
			{
				m_myCamera = FindMyCamera();
			}
		}
		
		protected Camera FindMyCamera()
        {
            int layerMask = 1 << m_owner.gameObject.layer;
            Camera[] cameras = GameObject.FindSceneObjectsOfType(typeof(Camera)) as Camera[];
            for (int i = 0, imax = cameras.Length; i < imax; ++i)
            {
                Camera cam = cameras[i];

                if ((cam.cullingMask & layerMask) != 0)
                {
					return cam;
                }
            }
            Debug.LogError("can't find proper camera for event:" + m_type);
			
			return null;
        }
		
		public bool CanUpdate
		{
			get{
				return m_canUpdate;
			}
			set{
				m_canUpdate = value;
			}
		}
     
        public XftEvent (XEventType type, XftEventComponent owner)
        {
            m_type = type;
            m_owner = owner;
        }

        public virtual void OnBegin ()
        {
			CanUpdate = true;
        }
		
		public virtual void Initialize ()
        {
        }
		
        public virtual void Update (float deltaTime)
        {
        }

        public virtual void Reset ()
        {
			m_elapsedTime = 0f;
			CanUpdate = false;
        }
		
    }
}
