using UnityEngine;
using System.Collections;

[System.Serializable]
public class Unit : MonoBehaviour {

    public StateMachine sm;
    private Order currentOrder;

	void Start () 
    {
        sm.SetFirstState(new MainState(sm, this));
	}

    void Update()
    {

    }

    public Order GetOrder()
    {
        return currentOrder;
    }
}
