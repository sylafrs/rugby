using UnityEngine;
using System.Collections.Generic;

/**
  * @class CreditScene
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class CreditScene : MonoBehaviour {

    private XboxInputs inputs;
    public InputTouch touch;
    public GameObject Abtn;
    public MyFade fade;

    public void Start()
    {
        this.gameObject.AddComponent<Timer>();
        inputs = this.gameObject.AddComponent<XboxInputs>();
        inputs.CheckAll();
    }

    public void Update()
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
            this.enabled = false;
            Abtn.SetActive(false);
            fade.Inverse();
            Timer.AddTimer(2, () =>
            {
                Application.LoadLevel("menu");
            });
        }
    }
}
