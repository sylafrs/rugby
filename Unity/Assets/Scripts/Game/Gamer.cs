using UnityEngine;
using System.Collections;

/**
 * @class Gamer
 * @brief Représente un Joueur
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[AddComponentMenu("Scripts/Game/Gamer")]
public class Gamer : MonoBehaviour
{
    public Team team;
    public Unit controlled;
    public Game game;
	public Vector3 passDirection;
	public float coefficientPressionCapture = 10.0f;
	public float delayMaxOnCapture = 0.5f;

    public InputSettings inputs;

	private bool onActionCapture = false;
	private float timeOnActionCapture = 0.0f;
	
	private bool canMove;
	
	void Start(){
		canMove = true;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	void stopMove(){
		canMove = false;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	void enableMove(){
		canMove = true;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	bool getMoveStatus(){
		return canMove;
	}

   
	void Update () {
        Vector3 direction = Vector3.zero;
		
		if (!canMove) return;		
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
       
        if (Input.GetKeyDown(inputs.drop))
        {
            controlled.Order = Order.OrderDrop(game.left[0]);
        }

		if (Input.GetKeyDown(inputs.plaquer) && controlled.NearUnits.Count > 0)
		{
			controlled.Order = Order.OrderPlaquer(controlled.NearUnits[0]);
		}

        if (game.Ball.Owner == controlled)
        {
            if (Input.GetKeyDown(inputs.passRight))
            {
                onActionCapture = true;
                passDirection = this.transform.right;
                game.Ball.transform.position = controlled.BallPlaceHolderRight.transform.position;
            }

            else if (Input.GetKeyDown(inputs.passLeft))
            {
                onActionCapture = true;
                passDirection = -this.transform.right;
                game.Ball.transform.position = controlled.BallPlaceHolderLeft.transform.position;
            }

            else if (Input.GetKeyUp(inputs.passRight) || Input.GetKeyUp(inputs.passLeft))
            {
                if (controlled == game.Ball.Owner)
                {
                    onActionCapture = false;
                    if (timeOnActionCapture > delayMaxOnCapture)
                        timeOnActionCapture = delayMaxOnCapture;
                    //Debug.DrawRay(this.transform.position, passDirection, Color.red);

                    controlled.Order = Order.OrderPass(controlled.ClosestAlly(), passDirection, timeOnActionCapture * coefficientPressionCapture);
                    passDirection = Vector3.zero;
                }
                else
                {
                    // Changer de perso.
                }
            }
        }

		if (onActionCapture)
		{
			timeOnActionCapture += Time.deltaTime;
		}
		else
		{
			timeOnActionCapture = 0.0f;
		}
	}

}
