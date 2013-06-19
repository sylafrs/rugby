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

    public float timeBeforeLoading;
    public float timeBeforeFading;
    public UITexture background;
    public UITexture pushStart;
    public MyFade fade;

    private XboxInputs inputs;
    
    enum Status
    {
        TITLE,
        CONTROLS
    }

    private Status state;
    public Texture controlBackground;

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

            if (ok)
            {
                background.mainTexture = controlBackground;
                pushStart.gameObject.SetActive(false);
                state = Status.CONTROLS;

                Timer.AddTimer(timeBeforeFading, () =>
                {
                    fade.Inverse();
                });
                Timer.AddTimer(timeBeforeLoading, () =>
                {
                    Application.LoadLevel("terrain");
                });
            }
        }
    }
}
