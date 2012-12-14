using UnityEngine;
using System.Collections;

[System.Serializable]
public class Unit : MonoBehaviour {

    public Order firstOrder;
    private Order currentOrder;

	void Start () {
        currentOrder = firstOrder;
	}
	
	void Update () {
	
	}
}
