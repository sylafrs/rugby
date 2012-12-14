using UnityEngine;
using System.Collections;

public class TEST : MonoBehaviour {

    public Unit agent1;
    public Unit agent2;
    public float epsilon;

    void Start()
    {
        Team rouge = new Team("Rouges", Color.red);
        Team bleue = new Team("Bleus", Color.blue);

        agent1.Team = rouge;
        agent2.Team = bleue;
    }


	void Update () {
        if (agent1 && Input.GetMouseButtonUp(0))
        {
            UpdateAgent(agent1);          
        }
        if (agent1 && Input.GetMouseButtonUp(1))
        {
            UpdateAgent(agent2);
        }
	}

    void UpdateAgent(Unit agent)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit outinfo;
        if (Physics.Raycast(ray, out outinfo, Mathf.Infinity))
        {
            if (outinfo.collider.name.Contains("Joueur"))
            {
                agent.ChangeOrder(Order.OrderFollow(outinfo.collider.GetComponent<Unit>(), Order.TYPE_DEPLACEMENT.MARCHE));
            }
            if (outinfo.collider.name.Equals("Gazon"))
            {
                agent.ChangeOrder(Order.OrderMove(outinfo.point, Order.TYPE_DEPLACEMENT.MARCHE));
            }
        }
    }
}
