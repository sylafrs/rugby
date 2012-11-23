using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/Test")]
public class TestBehaviour : MonoBehaviour {

    private GameObject mainPlayer;
    private GameObject otherPlayer;

    public GameObject PlayerPrefab;
    public GameObject Ball;

    public GameObject MainInitialPosition;
    public GameObject OtherInitialPosition;

    public float playerSpeed;

	// Use this for initialization
	void Start () {
        mainPlayer = GameObject.Instantiate(PlayerPrefab) as GameObject;
        Ball.transform.parent = mainPlayer.transform.GetChild(0);
        Ball.transform.localPosition = Vector3.zero;

        mainPlayer.transform.position = MainInitialPosition.transform.position;

        otherPlayer = GameObject.Instantiate(PlayerPrefab) as GameObject;
        otherPlayer.transform.position = OtherInitialPosition.transform.position;
        otherPlayer.transform.Rotate(new Vector3(0, 1, 0), 180);
    
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 movement = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement += mainPlayer.transform.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement -= mainPlayer.transform.forward;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement += mainPlayer.transform.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement -= mainPlayer.transform.right;
        }

        if (!movement.Equals(Vector3.zero))
        {
            mainPlayer.transform.localPosition += (movement / movement.magnitude * Time.deltaTime * playerSpeed);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            GoTo gt = Ball.GetComponent<GoTo>();
            gt.sendTo(otherPlayer.transform.GetChild(0), ballePassee);
            
        }
    }

    void ballePassee()
    {
        GameObject tmp = mainPlayer;
        mainPlayer = otherPlayer;
        otherPlayer = tmp;

        Ball.GetComponent<rotateMe>().rotate(new Vector3(0, 1, 0), 180f);        
    }
}
