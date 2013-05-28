using UnityEngine;
using System.Collections;
using System;

public partial class CameraManager{
	
	/*
	 * 
	 * Destination  		: the position to translate to
	 * Delay				: when to do it
	 * BlackscreenDuration	: time of black
	 * Onfinish				: Action to do on finish
	 * 
	 */
	public void transalateWithFade(Vector3 destination,float delay,float fadeiInDuration, float fadeOutDuration,
		float blackScreenDuration, Action Onfinish){
		
		CameraFade.StartAlphaFade(Color.black,false, fadeiInDuration, delay, () => { 
			this.gameCamera.transform.Translate(destination); 
			CameraFade.StartAlphaFade(Color.black,true, fadeOutDuration, blackScreenDuration, () => {
				Onfinish();
			});
		});
	}
	
	public void transalateWithFade(Vector3 destination, Quaternion _rotation, float delay,float fadeiInDuration, float fadeOutDuration,
		float blackScreenDuration, Action Onfinish, Action OnFade){
		
		CameraFade.StartAlphaFade(Color.black,false, fadeiInDuration, delay, () => { 
			OnFade();
			this.gameCamera.transform.Translate(destination); 
			this.gameCamera.transform.rotation = _rotation;
			CameraFade.StartAlphaFade(Color.black,true, fadeOutDuration, blackScreenDuration, () => {
				Onfinish();
			});
		});
	}
	
	public void transalateToWithFade(Vector3 destination, Quaternion _rotation,float delay,float fadeiInDuration, float fadeOutDuration,
		float blackScreenDuration, Action Onfinish){
		
		CameraFade.StartAlphaFade(Color.black,false, fadeiInDuration, delay, () => { 
			this.gameCamera.transform.Translate(destination - this.gameCamera.transform.position, Space.World); 
			this.gameCamera.transform.rotation = _rotation;
			CameraFade.StartAlphaFade(Color.black,true, fadeOutDuration, blackScreenDuration, () => {
				Onfinish();
			});
		});
	}

    public void transalateToWithFade(Vector3 destination, Quaternion _rotation, float delay, float fadeiInDuration, float fadeOutDuration,
        float blackScreenDuration, Action Onfinish, Action OnFade)
    {

        CameraFade.StartAlphaFade(Color.black, false, fadeiInDuration, delay, () =>
        {
            this.gameCamera.transform.Translate(destination - this.gameCamera.transform.position, Space.World);
            this.gameCamera.transform.rotation = _rotation;
			OnFade();
            CameraFade.StartAlphaFade(Color.black, true, fadeOutDuration, blackScreenDuration, () =>
            {
                Onfinish();
            });
        });
    }
}
