using UnityEngine;
using System.Collections;

/**
 * @class Team
 * @brief Une equipe (unite)
 * @author Sylvain Lafon
 */
[System.Serializable, AddComponentMenu("Scripts/Models/Team")]
public class Team : MonoBehaviour {

    public Game Game;
    public Color Color;
    public string Name;

    public But But;
    public Zone Zone;

    public int nbPoints = 0;

    private Unit [] units;

    public Unit this[int index]
    {
        get
        {
            if (index < 0 || index >= units.Length)
                return null;

            return units[index];
        }

        private set
        {

        }
    }

    public GameObject Prefab_model;
    
    public int nbUnits;

    public void Start()
    {
        But.Owner = this;
        Zone.Owner = this;
    }

    public void CreateUnits()
    {
        units = new Unit[nbUnits];
        for (int i = 0; i < nbUnits; i++)
        {
            Vector3 pos = this.transform.position + new Vector3((i - (nbUnits / 2.0f)) * 2, 0, 0);

            GameObject o = GameObject.Instantiate(Prefab_model, pos, Quaternion.identity) as GameObject;
            units[i] = o.GetComponent<Unit>();
            units[i].name = Name + " " + (i+1).ToString("D2");
            units[i].transform.parent = this.transform;
            units[i].Team = this;
            units[i].renderer.material.color = Color;

            // PATCH
            StateMachine_debugger d = o.GetComponent<StateMachine_debugger>();
            if (d != null)
            {
                d.r.x = Name.Equals("Bleu") ? 10 : 500;
                d.r.y = i * 100;
            }
        }
    }

    public bool Contains(Unit unit)
    {
        bool trouve = false;
        int i = 0;

        while (!trouve && i < nbUnits)
        {
            trouve = (units[i++] == unit);
        }

        return trouve;
    }
}
