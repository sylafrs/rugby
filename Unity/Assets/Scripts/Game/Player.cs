using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public Team team;
    public Unit controlled;
    public Game game;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.Z))
        {
            direction += (Camera.main.transform.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction -= (Camera.main.transform.forward);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            direction -= (Camera.main.transform.right);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += (Camera.main.transform.right);
        }

        if (direction != Vector3.zero)
        {
            controlled.ChangeOrder(Order.OrderMove(controlled.transform.position + direction.normalized, Order.TYPE_DEPLACEMENT.COURSE));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            controlled.ChangeOrder(Order.OrderGiveBall(game.left[0]));
        }
	}
}
