using UnityEngine;
using System.Collections.Generic;

/**
  * @class Video
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class Video : MonoBehaviour {
    public AudioSource audio;
    public MovieTexture movie;
    public AudioClip clip;

    private XboxInputs inputs;
    public InputTouch touch;

    public void Start()
    {
        inputs = this.gameObject.AddComponent<XboxInputs>();
        inputs.CheckAll();

        //QualitySettings.
        this.movie.loop = true;
        if (this.audio)
        {
            this.audio.loop = true;
            this.audio.clip = this.clip == null ? movie.audioClip : this.clip;
        }

        this.Play();
    }

    public bool PushTouch()
    {
        bool ok = false;
        foreach (var c in inputs.controllers)
        {
            if (c.GetButtonDown(touch.xbox))
            {
                ok = true;
                break;
            }
        }

        if (!ok)
        {
            if (Input.GetKeyDown(touch.keyboardP1) || Input.GetKeyDown(touch.keyboardP2))
            {
                ok = true;
            }
        }

        return ok;
    }

    public void Play()
    {
        this.movie.Play();
        this.audio.Play();
    }

    public void Update()
    {
        if (PushTouch())
        {
            Application.LoadLevel("menu");
        }
    }
}
