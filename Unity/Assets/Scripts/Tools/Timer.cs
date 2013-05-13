using UnityEngine;
using CallBack = System.Action;
using System.Collections.Generic;

/**
  * @class Timer
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class Timer : MonoBehaviour {

    private static Timer singleton;

    private struct myTimer {
        public CallBack callback;
        public float remainingTime;
    }

    private static List<myTimer> list = new List<myTimer>();

    public Timer()
    {
        if (singleton != null)
        {
            throw new UnityException("Singleton");
        }

        singleton = this;
    }

    public static void AddTimer(float time, CallBack callback)
    {
        myTimer t = new myTimer();
        t.remainingTime = time;
        t.callback = callback;
        list.Add(t);
    }
       
    public void Update()
    {
        foreach (myTimer t in list)
            UpdateTimer(t);
    }

    private void UpdateTimer(myTimer t)
    {
        t.remainingTime -= Time.deltaTime;
        if (t.remainingTime <= 0)
        {
            if (t.callback != null)
                t.callback();

            list.Remove(t);
        }
    }
}
