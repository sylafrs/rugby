using UnityEngine;
using System.Collections.Generic;
using System;

/**
  * @class IntroManager
  * @brief Description.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Scripts/MiniGames/Introduction")]
public class IntroManager : myMonoBehaviour {

    private Game game;
    public InputTouch touch;
    public Action OnFinish;

    void Start()
    {
        game = this.GetComponent<Game>();
        if (!game)
            throw new UnityException("I need the Game !");
    }
    
    void Update()
    {
        if ((game.p1.XboxController != null && game.p1.XboxController.GetButtonUp(touch.xbox)) || Input.GetKeyUp(touch.keyboard))
        {
            Finish();
        }        
    }

    void Finish()
    {
        this.enabled = false;
        if (OnFinish != null)
            OnFinish();
    }

}
