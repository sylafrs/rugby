using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Game/Game")]
public class Game : MonoBehaviour {

    public GameObject limiteTerrainNordEst;
    public GameObject limiteTerrainSudOuest;

    public Team right;
    public Team left;

    public Ball Ball;
    
	// Use this for initialization
	void Start ()
    {
        right.Game = this;
        left.Game = this;
        right.CreateUnits();
        left.CreateUnits();
	}

    // Update is called once per frame
    void Update()
    {
	
	}
}
