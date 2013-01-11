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

        if (Input.GetKey(GameSettings.up))
        {
            direction += (Camera.main.transform.forward);
        }
        if (Input.GetKey(GameSettings.down))
        {
            direction -= (Camera.main.transform.forward);
        }
        if (Input.GetKey(GameSettings.left))
        {
            direction -= (Camera.main.transform.right);
        }
        if (Input.GetKey(GameSettings.right))
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
