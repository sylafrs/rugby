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
        float r = Random.value;

        MyDebug.Log("Flip began. Value : " + r);

        if (r >= 0.5f)
        {
            winner = face;
        }
        else
        {
            winner = pile;
        }

        Timer.AddTimer(timeFlipping, () => {
            MyDebug.Log("Flip finised. Winner : " + winner);
            this.enabled = false;
            if(callBack != null) {
                callBack(winner);
            }
        });
    }
}
