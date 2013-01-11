using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Game")]
public class Game : MonoBehaviour {

    public GameObject limiteTerrainNordEst;
    public GameObject limiteTerrainSudOuest;

    public Team right;
    public Team left;

    public Ball Ball;
    public Player p1, p2;
    
	// Use this for initialization
	void Start ()
    {
        right.Game = this;
        left.Game = this;
        right.CreateUnits();
        left.CreateUnits();

        p1 = right.gameObject.AddComponent<Player>();
        p1.game = this;
        p1.team = right;
        p1.controlled = right[0];

        Ball.transform.parent = p1.controlled.BallPlaceHolder.transform;
        Ball.transform.localPosition = Vector3.zero;
        Ball.Owner = p1.controlled;

        Camera.mainCamera.transform.rotation = Quaternion.Euler(new Vector3(28.57f, 0f, 0f));                
	}

    void Update()
    {
        positionneCamera();       
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
}
