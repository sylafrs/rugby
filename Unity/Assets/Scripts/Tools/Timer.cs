using UnityEngine;
using CallBack = System.Action;
using System.Collections.Generic;

/**
  * @class Timer
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class TimerManager : MonoBehaviour {

    private static TimerManager singleton;

    private struct Timer {
        public CallBack callback;
        public float remainingTime;
    }

    private static List<Timer> list = new List<Timer>();

    public TimerManager()
    {
        if (singleton != null)
        {
            throw new UnityException("Singleton");
        }

        singleton = this;
    }

    public static void AddTimer(float time, CallBack callback)
    {
        Timer t = new Timer();
        t.remainingTime = time;
        t.callback = callback;
        list.Add(t);
    }
       
    public void Update()
    {
        foreach (Timer t in list)
            UpdateTimer(t);
    }

    private void UpdateTimer(Timer t)
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
