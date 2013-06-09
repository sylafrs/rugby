using UnityEngine;
using System.Collections.Generic;

/**
  * @class GameReferences
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class GameReferences : myMonoBehaviour {

    [System.Serializable]
    public class Managers
    {
        public CameraManager camera;
        public DropManager drop;
        public IntroManager intro;
        public TackleManager tackle;
        public TouchManager touch;
        public TransformationManager conversion;
        public ScrumManager scrum;
        public UIManager ui;
        public CoinFlipManager coin;
    }

    [System.Serializable]
    public class PlaceHolders
    {
        public Transform scrumPlacement;
        public Transform startPlacement;
        public Transform touchPlacement;
        public Transform conversionPlacement;
    }

    [System.Serializable]
    public class Positions
    {
        public Transform limiteTerrainNordEst;
        public Transform limiteTerrainSudOuest;
        public Transform scrumFieldNE;
        public Transform scrumFieldSW;
		public Transform cameraFirstPosition;
        public Transform fieldCenter;
        public Transform rotationCenter;
    }

    [System.Serializable]
    public class GameObjects
    {
        public Ball ball;
        public Renderer ScrumBloc;
    }

    public Game game;
    public StateMachine stateMachine;
    public Referee Referee;

    public Team north, south;

    public Managers managers;
    public PlaceHolders placeHolders;
    public GameObjects gameObjects;
    public Positions positions;

    public XboxInputs xboxInputs;
}
