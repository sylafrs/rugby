using UnityEngine;
using System.Collections;


namespace Xft
{
	public class SoundEvent : XftEvent 
	{
		static AudioListener m_Listener;
		public SoundEvent(XftEventComponent owner)
			: base(XEventType.Sound, owner)
		{
			
		}
		
		protected AudioSource PlaySound(AudioClip clip, float volume, float pitch)
		{
			if (clip != null)
        	{
            	if (m_Listener == null)
            	{
                	m_Listener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;

                	if (m_Listener == null)
                	{
                    	Camera cam = Camera.main;
                    	if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
                    	if (cam != null) m_Listener = cam.gameObject.AddComponent<AudioListener>();
                	}
            	}

            	if (m_Listener != null)
            	{
                	AudioSource source = m_Listener.audio;
                	if (source == null) source = m_Listener.gameObject.AddComponent<AudioSource>();
                	source.pitch = pitch;
                	source.PlayOneShot(clip, volume);
                	return source;
            	}
        	}
        	return null;
		}
  
        
        public override void Reset ()
        {
            base.Reset ();
        }
		
		public override void OnBegin ()
		{
			base.OnBegin ();
			PlaySound(m_owner.Clip,m_owner.Volume,m_owner.Pitch);
		}
	}
}

