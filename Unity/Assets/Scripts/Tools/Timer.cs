using UnityEngine;
using CallBack = System.Action;
using System.Collections.Generic;

/**
  * @class Timer
  * @brief Description.
  * @author Sylvain Lafon
  * @see myMonoBehaviour
  */
[AddComponentMenu("Code Tools/Timer")]
public class Timer : myMonoBehaviour {

    private class myTimer {
        public CallBack callback;
		//public CallBack OnUpdate;
        public float remainingTime;
    }

    private static List<myTimer> list;

    public static void AddTimer(float time, CallBack callback)
    {
		myTimer t = new myTimer();
        t.remainingTime = time;
        t.callback = callback;
		
		if(list == null)
			list = new List<myTimer>();
		
        list.Add(t);
    }
       
    void Update()
    {	
		if(list == null)
			list = new List<myTimer>();
		
		bool delete = false;

        // Copies the current list to another one to allows the timers 
        // to modify the tasks list (for instance : adding new tasks)
        List<myTimer> currentList = new List<myTimer>(list);

        foreach (myTimer t in currentList)
            delete = delete || UpdateTimer(t);
		
		if(delete) {
			int l = list.Count;
			for(int i = 0; i < l; i++) {
				myTimer t = list[i];
				
				if(t.callback == null) {
					list.Remove(t);
					i--;
					l--;
				}
			}
		}
    }

    private bool UpdateTimer(myTimer t)
    {
        t.remainingTime -= Time.deltaTime;
        if (t.remainingTime <= 0)
        {
            if (t.callback != null)
                t.callback();
			
			t.callback = null;
			return true;
        }
			
		return false;
    }
}
