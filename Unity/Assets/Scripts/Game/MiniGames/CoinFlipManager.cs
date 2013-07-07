using UnityEngine;
using System.Collections.Generic;

/**
  * @class CoinFlipManager
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class CoinFlipManager : MonoBehaviour {

    public float timeFlipping;
    public Team pile;
    public Team face;

    public System.Action<Team> callBack;

    private Team winner;

    public void OnEnable()
    {
        GameSettings.CoinFlip settings = Game.instance.settings.Global.Game.flipSettings;

        float r = Random.value;

        //MyDebug.Log("Flip began. Value : " + r);

        if (settings == GameSettings.CoinFlip.RAND)
        {
            if (r >= 0.5f)
            {
                winner = face;
            }
            else
            {
                winner = pile;
            }
        }
        else
        {
            if (settings == GameSettings.CoinFlip.JAPAN)
            {
                winner = Game.instance.southTeam;
            }
            else
            {
                winner = Game.instance.northTeam;
            }
        }

        Timer.AddTimer(timeFlipping, () => {
            //MyDebug.Log("Flip finised. Winner : " + winner);
            this.enabled = false;
            if(callBack != null) {
                callBack(winner);
            }
        });
    }
}
