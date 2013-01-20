using UnityEngine;
using System.Collections;

/**
 * @class Gamer
 * @brief Représente un Joueur
 * @author Sylvain Lafon
 */
[AddComponentMenu("Scripts/Game/Gamer")]
public class Gamer : MonoBehaviour
{
    public Team team;
    public Unit controlled;
    public Game game;

    public InputSettings inputs;
	
	void Update () {
        Vector3 direction = Vector3.zero;

        if (inputs == null) return;

        if (Input.GetKey(inputs.up))
        {
            direction += (Camera.main.transform.forward);
        }
        if (Input.GetKey(inputs.down))
        {
            direction -= (Camera.main.transform.forward);
        }
        if (Input.GetKey(inputs.left))
        {
            direction -= (Camera.main.transform.right);
        }
        if (Input.GetKey(inputs.right))
        {
            direction += (Camera.main.transform.right);
        }

        if (direction != Vector3.zero)
        {
            controlled.Order = Order.OrderMove(controlled.transform.position + direction.normalized, Order.TYPE_DEPLACEMENT.COURSE);
        }
       
        if (Input.GetKeyDown(inputs.change))
        {
            controlled.Order = Order.OrderGiveBall(game.left[0]);
        }
	}
}
