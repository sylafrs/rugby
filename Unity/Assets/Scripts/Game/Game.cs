using UnityEngine;
using System.Collections;

/**
 * @class Game
 * @brief Classe principale du jeu
 * @author Sylvain Lafon
 * @author Guilleminot Florian
 */
[AddComponentMenu("Scripts/Game/Game")]
public class Game : MonoBehaviour {

    public GameSettings settings;

    public GameObject limiteTerrainNordEst;
    public GameObject limiteTerrainSudOuest;

    public Team right;
    public Team left;
       
    public Gamer p1, p2;

    public Ball Ball;
    public GameLog Log;
    
    private Team Owner;
	

	private bool cameraLocked;
    
	void Start ()
    {
        this.Log = this.gameObject.AddComponent<GameLog>();

        right.Game = this;
        left.Game = this;
        right.right = true;
        left.right = false;
        right.CreateUnits();
        left.CreateUnits();

        right.opponent = left;
        left.opponent = right;

        p1 = right.gameObject.AddComponent<Gamer>();
        p1.game = this;
        p1.team = right;
        p1.controlled = right[p1.team.nbUnits/2];
        p1.controlled.IndicateSelected(true);
        p1.inputs = settings.inputs;

        //* 
        p2 = left.gameObject.AddComponent<Gamer>();
        p2.game = this;
        p2.team = left;
        p2.controlled = left[0];
        p2.inputs = settings.inputs2;
        // */

        this.Owner = p1.controlled.Team;
        Ball.Game = this;
        Ball.transform.parent = p1.controlled.BallPlaceHolderRight.transform;
        Ball.transform.localPosition = Vector3.zero;
        Ball.Owner = p1.controlled;        
       
        Camera.mainCamera.transform.rotation = Quaternion.Euler(new Vector3(28.57f, 0f, 0f));
		
		this.cameraLocked = true;
    }

    void Update()
    {
        if(this.cameraLocked)positionneCamera();       
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	public void unlockCamera(){
		this.cameraLocked = false;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	public bool getCameraLocked(){
		return this.cameraLocked;
	}
	
	/*
	 * @ author Maxens Dubois
	 */
	public void lockCamera(){
		this.cameraLocked = true;
	}
	
    void positionneCamera()
    {
        // TODO : Changer de place, rendre customizable.
        // Synopsis : Positionne la caméra derrière le joueur sélectionné par le joueur courant.
        Vector3 ecart = new Vector3(1.32f, 16.91f, -9.73f);
        Vector3 test = new Vector3(
            ecart.x * Camera.mainCamera.transform.forward.x,
            ecart.y * Camera.mainCamera.transform.forward.y,
            -ecart.z * Camera.mainCamera.transform.forward.z
        );

        if(Ball.Owner)
            Camera.mainCamera.transform.position = Ball.Owner.transform.position - test;
        else
            Camera.mainCamera.transform.position = Ball.transform.position - test;
    }

    public void OwnerChanged(Unit before, Unit after)
    {
        if (after != null)
        {
            if (after.Team != Owner)
            {
                Owner = after.Team;
                Camera.mainCamera.GetComponent<rotateMe>().rotate(new Vector3(0, 1, 0), 180);
            }

            // PATCH
            // p1.controlled = after;
            if (after.Team == right)
            {
                p1.controlled.IndicateSelected(false);
                p1.controlled = after;
                p1.controlled.IndicateSelected(true);
            }
            else if (p2 != null)
            {
                p2.controlled = after;
            }

            Log.Add("La balle est attrapee par l'equipe " + after.Team.Name);
        }
        
        this.left.OwnerChanged();
        this.right.OwnerChanged();       
    }

    /**
     * @author Sylvain Lafon
     * @brief Se déclenche quand il y a plaquage
     */
    public void EventTackle(Unit tackler, Unit tackled)
    {
        Ball.EventTackle(tackler, tackled);
    }
}
