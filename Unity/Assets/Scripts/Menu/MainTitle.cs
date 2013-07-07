using UnityEngine;
using System.Collections.Generic;

/**
  * @class MainTitle
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class MainTitle : MonoBehaviour {
    public InputTouch touch;
    public InputTouch skip;
    public InputTouch back;
    public InputDirection Move;

    public float timeBeforeLoading;
    public UITexture background;
    public UITexture pushStart;
    public MyFade fade;
    public GameObject AButton;
    public AudioSource validation;
    public AudioSource changement;
    private XboxInputs inputs;

    public UIPanel MainMenu;
    public UITexture Play, Tuto, Selecter;
    public UITexture StartHighlight;
    
    enum Status
    {
        TITLE,
        MENU,
        CONTROLS,
        GOOUT
    }

    private Status state;
    public Texture controlBackground;
    public Texture mainBackground;

    public float speedSelecter;
    private TARGETSELECTER targetSelecter = TARGETSELECTER.PLAY;

    enum TARGETSELECTER
    {
        PLAY, TUTO
    }
    
    public void Start()
    {
        this.gameObject.AddComponent<Timer>();
        inputs = this.gameObject.AddComponent<XboxInputs>();
        inputs.CheckAll();

        state = Status.TITLE;
    }    

    public void Update()
    {
        if (state == Status.TITLE)
        {
            if (PushTouch())
            {
                pushStart.gameObject.SetActive(false);
                StartHighlight.gameObject.SetActive(false);
                
                MainMenu.gameObject.SetActive(true);               
                                
                state = Status.MENU;
                
                validation.Play();
            }
        }
        else if (state == Status.MENU)
        {
            float yPlay = this.Play.transform.position.y;
            float yTuto = this.Tuto.transform.position.y;
            Vector3 pos = this.Selecter.transform.position;

            if (targetSelecter == TARGETSELECTER.PLAY)
            {
                pos.y = Mathf.Lerp(pos.y, yPlay, this.speedSelecter * Time.deltaTime);
            }

            if (targetSelecter == TARGETSELECTER.TUTO)
            {
                pos.y = Mathf.Lerp(pos.y, yTuto, this.speedSelecter * Time.deltaTime);
            }            

            if (GoTop())
            {
                if (targetSelecter != TARGETSELECTER.PLAY)
                {
                    changement.Play();
                }
                targetSelecter = TARGETSELECTER.PLAY;
            }

            if (GoBottom())
            {
                if (targetSelecter != TARGETSELECTER.TUTO)
                {
                    changement.Play();
                }
                targetSelecter = TARGETSELECTER.TUTO;
            }

            if (PushSkip() || PushTouch())
            {              
                if (targetSelecter == TARGETSELECTER.PLAY)
                {
                    state = Status.CONTROLS;

                    background.mainTexture = controlBackground;
                    Selecter.gameObject.SetActive(false);
                    MainMenu.gameObject.SetActive(false);
                    AButton.SetActive(true);

                    validation.Play();
                }
                if (targetSelecter == TARGETSELECTER.TUTO)
                {
                    validation.Play();
                    fade.Inverse();

                    Timer.AddTimer(timeBeforeLoading, () =>
                    {
                        Application.LoadLevel("tuto");
                    });

                    state = Status.GOOUT;
                }
            }

            this.Selecter.transform.position = pos;
        }
        else if (state == Status.CONTROLS)
        {        
            if (PushSkip())
            {
                AButton.SetActive(false);
                fade.Inverse();

                Timer.AddTimer(timeBeforeLoading, () =>
                {
                    Application.LoadLevel("terrain");
                });

                state = Status.GOOUT;
            }
            else if (PushBack())
            {
                background.mainTexture = mainBackground;
                Selecter.gameObject.SetActive(true);
                MainMenu.gameObject.SetActive(true);
                AButton.SetActive(false);

                validation.Play();

                state = Status.MENU;
            }
        }
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

    public bool PushSkip()
    {
        bool ok = false;
        foreach (var c in inputs.controllers)
        {
            if (c.GetButtonDown(skip.xbox))
            {
                ok = true;
                break;
            }
        }

        if (!ok)
        {
            if (Input.GetKeyDown(skip.keyboardP1) || Input.GetKeyDown(skip.keyboardP2))
            {
                ok = true;
            }
        }

        return ok;
    }

    public bool GoTop()
    {
        bool ok = false;
        foreach (var c in inputs.controllers)
        {
            if (c.IsConnected && c.GetDirection(Move.xbox).y > 0.2f)
            {
                ok = true;
                break;
            }
        }

        if (!ok)
        {
            if (Input.GetKeyDown(Move.keyboardP1.up) || Input.GetKeyDown(Move.keyboardP2.up))
            {
                ok = true;
            }
        }

        return ok;
    }

    public bool GoBottom()
    {
        bool ok = false;
        foreach (var c in inputs.controllers)
        {
            if (c.IsConnected && c.GetDirection(Move.xbox).y < -0.2f)
            {
                ok = true;
                break;
            }
        }

        if (!ok)
        {
            if (Input.GetKeyDown(Move.keyboardP1.down) || Input.GetKeyDown(Move.keyboardP2.down))
            {
                ok = true;
            }
        }

        return ok;
    }

    public bool PushBack()
    {
        bool ok = false;
        foreach (var c in inputs.controllers)
        {
            if (c.GetButtonDown(back.xbox))
            {
                ok = true;
                break;
            }
        }

        if (!ok)
        {
            if (Input.GetKeyDown(back.keyboardP1) || Input.GetKeyDown(back.keyboardP2))
            {
                ok = true;
            }
        }

        return ok;
    }
}
