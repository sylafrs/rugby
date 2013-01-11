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
	
	void Update () {
        Vector3 direction = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.G))
        {
            Camera.main.GetComponent<rotateMe>().rotate(new Vector3(0, 1, 0), 180);
        }

        if (Input.GetKey(GameSettings.settings.inputs.up))
        {
            direction += (Camera.main.transform.forward);
        }
        if (Input.GetKey(GameSettings.settings.inputs.down))
        {
            direction -= (Camera.main.transform.forward);
        }
        if (Input.GetKey(GameSettings.settings.inputs.left))
        {
            direction -= (Camera.main.transform.right);
        }
        if (Input.GetKey(GameSettings.settings.inputs.right))
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
